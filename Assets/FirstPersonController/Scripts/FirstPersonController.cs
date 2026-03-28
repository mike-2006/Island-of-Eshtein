using System;
using UnityEngine;

namespace SimpleFPS
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] public bool m_IsWalking;
        [SerializeField] public float m_WalkSpeed;
        [SerializeField] public float m_RunSpeed;
        [SerializeField][Range(0f, 1f)] public float m_RunstepLenghten;
        [SerializeField] public float m_JumpSpeed;
        [SerializeField] public float m_StickToGroundForce;
        [SerializeField] public float m_GravityMultiplier;
        [SerializeField] public MouseLook m_MouseLook;
        [SerializeField] public bool m_UseFovKick;
        [SerializeField] public FOVKick m_FovKick = new FOVKick();
        [SerializeField] public bool m_UseHeadBob;
        [SerializeField] public CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] public LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] public float m_StepInterval;
        [SerializeField] public AudioClip[] m_FootstepSounds;
        [SerializeField] public AudioClip m_JumpSound;
        [SerializeField] public AudioClip m_LandSound;
        [SerializeField] public float m_LandingSoundDelay = 2f;

        // === ПЛАВНОСТЬ ДВИЖЕНИЯ ===
        [Header("Smooth Movement")]
        [SerializeField] private float moveSmoothTime = 0.1f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 15f;
        [SerializeField] private float verticalSmoothTime = 0.05f;

        // === BUNNYHOP СИСТЕМА ===
        [Header("Bunnyhop Settings")]
        [SerializeField] private bool m_EnableBunnyhop = true;
        [SerializeField] private float m_BunnyhopSpeedBonus = 1.15f;      // множитель скорости за стаки
        [SerializeField] private float m_BunnyhopTimingWindow = 0.15f;    // окно для идеального прыжка (сек)
        [SerializeField] private int m_MaxBunnyhopStacks = 5;             // макс бонусов
        [SerializeField] private float m_BunnyhopGroundGrace = 0.1f;      // время после приземления для бхопа

        private Camera m_Camera;
        public bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector2 m_SmoothInput;
        private Vector3 m_MoveDir = Vector3.zero;
        private Vector3 _currentVelocity;
        private Vector3 _smoothMoveVelocity;
        private float _verticalVelocity;
        private float _verticalVelocitySmooth;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;
        private float m_LandingTimeLeft;
        private bool m_LandingSoundAvailable;

        // === BUNNYHOP ПЕРЕМЕННЫЕ ===
        private int m_BunnyhopStacks = 0;
        private float m_LastJumpTime = -999f;
        private float m_LandTime = -999f;
        private bool m_CanBunnyhop = false;
        private float m_BunnyhopTimer = 0f;

        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, m_Camera.transform);
            m_LandingTimeLeft = m_LandingSoundDelay;
            m_LandingSoundAvailable = true;
            _currentVelocity = Vector3.zero;
            _smoothMoveVelocity = Vector3.zero;
            _verticalVelocity = 0f;
            m_SmoothInput = Vector2.zero;
        }

        private void Update()
        {
            RotateView();

            // === HOLD-TO-JUMP + BUNNYHOP ЛОГИКА ===
            bool jumpHeld = Input.GetButton("Jump");

            if (jumpHeld && !m_Jump)
            {
                m_Jump = true; // Первый нажим
            }
            else if (!jumpHeld)
            {
                m_Jump = false; // Отпустили кнопку
            }

            // Отслеживание приземления для бхопа
            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                m_LandTime = Time.time;
                m_CanBunnyhop = true;

                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                _verticalVelocity = 0f;
                m_Jumping = false;
            }

            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            // Таймер для окна бхопа
            if (m_CanBunnyhop && Time.time - m_LandTime > m_BunnyhopGroundGrace)
            {
                m_CanBunnyhop = false;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;

            // Decay бхант-стаков
            if (m_EnableBunnyhop && m_BunnyhopStacks > 0 && !m_CharacterController.isGrounded)
            {
                m_BunnyhopTimer += Time.deltaTime;
                if (m_BunnyhopTimer >= 1f)
                {
                    m_BunnyhopStacks = Mathf.Max(0, m_BunnyhopStacks - 1);
                    m_BunnyhopTimer = 0f;
                }
            }

            m_LandingTimeLeft -= Time.deltaTime;

            if (m_LandingTimeLeft < 0)
            {
                m_LandingTimeLeft = m_LandingSoundDelay;
                m_LandingSoundAvailable = true;
            }
        }

        private void PlayLandingSound()
        {
            if (m_LandingSoundAvailable)
            {
                m_AudioSource.clip = m_LandSound;
                m_AudioSource.Play();
                m_NextStep = m_StepCycle + .5f;
                m_LandingSoundAvailable = false;
            }
        }

        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);

            Vector3 desiredMove = transform.forward * m_SmoothInput.y + transform.right * m_SmoothInput.x;

            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                                m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            // === ПЛАВНОЕ ДВИЖЕНИЕ ===
            Vector3 targetVelocity = new Vector3(
                desiredMove.x * speed,
                _verticalVelocity,
                desiredMove.z * speed
            );

            _currentVelocity = Vector3.SmoothDamp(
                _currentVelocity,
                targetVelocity,
                ref _smoothMoveVelocity,
                moveSmoothTime
            );

            if (m_CharacterController.isGrounded)
            {
                _verticalVelocity = -m_StickToGroundForce;

                // === JUMP LOGIC (HOLD + BUNNYHOP) ===
                if (m_Jump)
                {
                    // Проверка на бхант
                    if (m_EnableBunnyhop && m_CanBunnyhop &&
                        Time.time - m_LastJumpTime <= m_BunnyhopTimingWindow)
                    {
                        // Идеальный бхант!
                        m_BunnyhopStacks = Mathf.Min(m_MaxBunnyhopStacks, m_BunnyhopStacks + 1);
                        m_BunnyhopTimer = 0f;
                    }

                    _verticalVelocity = m_JumpSpeed;
                    _currentVelocity.y = _verticalVelocity;
                    PlayJumpSound();
                    m_LastJumpTime = Time.time;
                    m_CanBunnyhop = false;
                    m_Jumping = true;

                    // Не сбрасываем m_Jump для hold-то-джамп
                    // Он сбросится в Update когда отпустят кнопку
                }
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * m_GravityMultiplier * Time.fixedDeltaTime;

                _currentVelocity.y = Mathf.SmoothDamp(
                    _currentVelocity.y,
                    _verticalVelocity,
                    ref _verticalVelocitySmooth,
                    verticalSmoothTime
                );
            }

            // === ПРИМЕНЕНИЕ BUNNYHOP БОНУСА ===
            float bhopMultiplier = 1f;
            if (m_EnableBunnyhop && m_BunnyhopStacks > 0)
            {
                bhopMultiplier = 1f + (m_BunnyhopSpeedBonus - 1f) * (m_BunnyhopStacks / (float)m_MaxBunnyhopStacks);
            }

            Vector3 finalVelocity = _currentVelocity;
            if (bhopMultiplier > 1f && !m_CharacterController.isGrounded)
            {
                // Увеличиваем только горизонтальную скорость
                finalVelocity.x *= bhopMultiplier;
                finalVelocity.z *= bhopMultiplier;
            }

            m_CollisionFlags = m_CharacterController.Move(finalVelocity * Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }

        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }

        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                                Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }

        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }

            int n = UnityEngine.Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }

        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                        (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }

        private void GetInput(out float speed)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);

            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // === ПЛАВНЫЙ ИНПУТ ===
            float targetX = horizontal;
            float targetY = vertical;

            if (Mathf.Abs(horizontal) > 0.1f)
                m_SmoothInput.x = Mathf.MoveTowards(m_SmoothInput.x, targetX, acceleration * Time.deltaTime);
            else
                m_SmoothInput.x = Mathf.MoveTowards(m_SmoothInput.x, 0, deceleration * Time.deltaTime);

            if (Mathf.Abs(vertical) > 0.1f)
                m_SmoothInput.y = Mathf.MoveTowards(m_SmoothInput.y, targetY, acceleration * Time.deltaTime);
            else
                m_SmoothInput.y = Mathf.MoveTowards(m_SmoothInput.y, 0, deceleration * Time.deltaTime);

            if (m_SmoothInput.sqrMagnitude > 1)
            {
                m_SmoothInput.Normalize();
            }

            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }

        private void RotateView()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;

            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }

    }
}