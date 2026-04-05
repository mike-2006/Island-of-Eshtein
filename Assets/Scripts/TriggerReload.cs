using SimpleFPS;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerReload : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FirstPersonController fps = other.GetComponent<FirstPersonController>();

        if (fps != null)
        {
            SceneManager.LoadScene(0);
        }
    }
}
