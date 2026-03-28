using SimpleFPS;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    protected virtual void OnTriggerEnter(Collider other)
    {
        FirstPersonController fps = other.GetComponent<FirstPersonController>();

        if (fps != null)
        {
            Destroy(gameObject);
            Debug.Log("1");
        }
    }
}
