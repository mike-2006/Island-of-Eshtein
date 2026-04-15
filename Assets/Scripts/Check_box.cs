using UnityEngine;

public class Check_box : MonoBehaviour
{
    public GameObject openVisual;
    public GameObject closeVisual;

    public bool isopen = false;

    private void Start()
    {
        UpdateVisual();
    }
    public void OnInteraction()
    {
        isopen = !isopen;

        UpdateVisual();

        Debug.Log(isopen ? "ящик открыт!" : "ящик закрыт!");
    }
    private void UpdateVisual()
    {
        if (isopen)
        {
            openVisual.SetActive(true);
            closeVisual.SetActive(false);
        }
        else
        {
            openVisual.SetActive(false);
            closeVisual.SetActive(true);
           
        }
    }
}
