using TMPro;
using UnityEngine;

public class UI_Time_Changer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ui_time;
    [SerializeField] private float deltatime;

    private float current_time;
    private int time = 10;

    void Start()
    {
        current_time = Time.time;
    }

    void Update()
    {
        if (Time.time >= current_time + deltatime)
        {
            time += 1;
            ui_time.text = time.ToString() + ":00";
            current_time = Time.time;
        }
    }
}
