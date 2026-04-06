using SimpleFPS;
using System.Linq;
using UnityEngine;

public class CheckDesk : MonoBehaviour
{
    [SerializeField] private Bag bag;
    [SerializeField] private Renderer[] material;

   
    private int i = 0;
    private void OnTriggerEnter(Collider other)
    {
        FirstPersonController fps = other.GetComponent<FirstPersonController>();
        Debug.Log(bag.Get_Desk() > 0);
        if (fps != null && bag.Get_Desk() > 0 && i < material.Length)
        {
            bag.Remove_Desk(1);

           material[i].gameObject.GetComponent<BoxCollider>().enabled = true;
           SetMaterialOpaque(material[i]);

            Debug.Log("Изменил материал");
            i ++;

        }
    }

    private void SetMaterialOpaque(Renderer renderer)
    {
        Material mat = renderer.material;

        mat.SetFloat("_Surface", 0f);

        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.renderQueue = 2000;

        renderer.material = mat;
    }
}
