using UnityEngine;

public class Change_Light : MonoBehaviour
{
    [SerializeField] private Light sunLight;
    [SerializeField] private Light moonLight;

    [SerializeField] private UI_Time_Changer time_Changer;


    // Update is called once per frame
    void Update()
    {
        bool isDay = (time_Changer.CurrentHour >= 12 && time_Changer.CurrentHour < 19);
        bool isLateDay = (time_Changer.CurrentHour > 18);
        bool isEarlyNight = (time_Changer.CurrentHour < 7);

        if (isDay)
        {
            sunLight.enabled = true;
            sunLight.intensity = Mathf.Lerp(sunLight.intensity, 2, Time.deltaTime * 0.6f);

            moonLight.enabled = false;
            moonLight.intensity = 1;
        }
        else if (isLateDay)
        {
            sunLight.enabled = true;
            sunLight.intensity = Mathf.Lerp(sunLight.intensity, 0.5f, Time.deltaTime * 0.6f);

            moonLight.enabled = false;
            moonLight.intensity = 1;
        }
        else if (isEarlyNight)
        {
            moonLight.enabled = true;
            moonLight.intensity = Mathf.Lerp(moonLight.intensity, 0.3f, Time.deltaTime * 0.6f);

            sunLight.enabled = false;
            sunLight.intensity = 0.5f;
        }
        else
        {
            moonLight.enabled = true;
            moonLight.intensity = Mathf.Lerp(moonLight.intensity, 1f, Time.deltaTime * 0.6f);

            sunLight.enabled = false;
            sunLight.intensity = 0.5f;
        }
    }
}
