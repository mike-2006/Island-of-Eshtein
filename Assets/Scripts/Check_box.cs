using UnityEngine;

public class Check_box : MonoBehaviour
{
    public GameObject openVuisual;
    public GameObject closeVuisual;

    private bool isopen = false;

    private void OnInteraction()
    {
        if (isopen)
        {
            openVuisual.SetActive(false);
            closeVuisual.SetActive(true);
            isopen = false;
        }
        else
        {
            closeVuisual.SetActive(false);
            openVuisual.SetActive(true);
            isopen = true;
        }
    }
}
