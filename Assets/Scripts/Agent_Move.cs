using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent_Move : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float updateInterval = 1f;
    [SerializeField] private float speed = 15f;

    private NavMeshAgent agent;
    private float _nextUpdateTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.position);
        _nextUpdateTime = Time.time + updateInterval;
    }

    void Update()
    {
        // === ГЛАВНОЕ: НЕ ВЫЗЫВАЕМ SetDestination ЕСЛИ ПУТЬ УЖЕ СЧИТАЕТСЯ! ===
        if (agent.pathPending)
        {
            agent.isStopped = true;

            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                transform.position += direction * speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(direction);
            }

            return; // Выходим, не вызываем SetDestination пока считается
        }
        if (agent.hasPath)
        {
            agent.isStopped = false;

            // Обновляем цель с интервалом
            if (Time.time >= _nextUpdateTime)
            {
                agent.SetDestination(player.position);
                _nextUpdateTime = Time.time + updateInterval;
            }
        }


    }

    private void OnDrawGizmosSelected()
    {
        if (agent == null) return;

        // Рисуем весь путь
        Vector3[] pathCorners = agent.path.corners;

        for (int i = 0; i < pathCorners.Length - 1; i++)
        {
            // Линия пути
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pathCorners[i], pathCorners[i + 1]);

            // Точки (углы)
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pathCorners[i], 0.3f);
        }

        // Последняя точка
        if (pathCorners.Length > 0)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pathCorners[pathCorners.Length - 1], 0.5f);
        }

        // Текущая цель
        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.position, 1f);
        }
    }
}
