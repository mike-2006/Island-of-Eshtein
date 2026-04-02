using SimpleFPS;
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
        if (fps != null && bag.Get_Desk() > 0 && i < 4)
        {
            bag.Remove_Desk(1);

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
