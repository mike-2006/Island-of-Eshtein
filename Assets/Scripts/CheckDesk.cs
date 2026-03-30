using SimpleFPS;
using UnityEngine;

public class CheckDesk : MonoBehaviour
{
    [SerializeField] private Bag bag;
    [SerializeField] private Material[] material;


    private int i = 0;
    private void OnTriggerEnter(Collider other)
    {
        FirstPersonController fps = other.GetComponent<FirstPersonController>();

        if (fps != null && bag.Get_Desk() > 0 && i < 4)
        {
            bag.Remove_Desk(1);
            material[i].SetFloat("_Surface", 0f);
            i ++;

        }
    }
}
