using UnityEngine;

public class Key : MonoBehaviour
{
    public bool iskey = false;

    public void Pickup()
    {
        iskey = true;
        gameObject.SetActive(false);
        Debug.Log("взят ключ");
    }
}
