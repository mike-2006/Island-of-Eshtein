using Unity.VisualScripting;
using UnityEngine;

public class Night_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawn_object;
    [SerializeField] private UI_Time_Changer time;

    private int lastHour = -1;

    private void Update()
    {
        int currenthour = time.CurrentHour;

        if (currenthour == lastHour) return;

        lastHour = currenthour;

        bool isNight = (currenthour < 6 && currenthour > 0);

        spawn_object.SetActive(isNight);
    }
}
