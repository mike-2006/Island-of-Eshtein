using UnityEngine;

public class Rotating : MonoBehaviour
{
    [SerializeField] private Vector3 speed;

    private void Update()
    {
        transform.Rotate(speed * Time.deltaTime);
    }
}
