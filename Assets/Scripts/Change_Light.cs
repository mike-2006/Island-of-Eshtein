using UnityEngine;

public class Change_Light : MonoBehaviour
{
    [SerializeField] private Light main_Light;

    [SerializeField] private UI_Time_Changer time_Changer;

    [Header("Skybox Материалы")]
    public Material daySkybox;
    public Material nightSkybox;
    void Update()
    {
        bool isEarlyDay = (time_Changer.CurrentHour > 5 && time_Changer.CurrentHour < 14);
        bool isLateDay = (time_Changer.CurrentHour > 13 && time_Changer.CurrentHour < 20);
        bool isEarlyNight = (time_Changer.CurrentHour > 19);
        bool isLateNight = (time_Changer.CurrentHour < 6);

        Material targetSkybox = isLateNight ? nightSkybox : daySkybox;
        float targetAmbient = isLateNight ? 0.1f : 1;

        float targetIntensivity = isEarlyDay ? 2 : isLateDay ? 1.25f : isEarlyNight ? 0.75f : 0.4f; 
        float targetTemperature = isEarlyDay ? 4000 : isLateDay ? 7000 : isEarlyNight ? 11000 : 14000;

        if (isLateNight)
        {
            RenderSettings.skybox = nightSkybox;
        }
        else
        {
            RenderSettings.skybox = daySkybox;
        }
        RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, targetAmbient, Time.deltaTime * 0.5f);
        main_Light.intensity = Mathf.Lerp(main_Light.intensity, targetIntensivity, Time.deltaTime * 0.4f);
        main_Light.colorTemperature = Mathf.Lerp(main_Light.colorTemperature, targetTemperature, Time.deltaTime * 0.4f);


    }
}
