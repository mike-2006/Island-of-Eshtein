using TMPro;
using UnityEngine;

public class UI_Time_Changer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ui_time;
    [SerializeField] private float deltatime;

    public int time = 6;

    private float current_time;

    public int CurrentHour => time;
    void Start()
    {
        current_time = Time.time;
    }

    void Update()
    {
        if (Time.time >= current_time + deltatime)
        {
            time += 1;
            if(time > 24)
            {
                time = 1;
            }
            ui_time.text = time.ToString() + ":00";
            current_time = Time.time;
               
        }
    }
}
