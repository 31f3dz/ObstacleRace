using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    public static bool night;
    bool switching;

    [SerializeField] Light directionalLight;
    [SerializeField] Material daySkybox;
    [SerializeField] Material nightSkybox;
    [SerializeField] Material emission;
    [SerializeField] DayNightTimer dayNightTimer;
    [SerializeField] float switchingTime = 5.0f;

    Color32 defaultColor = new Color32(255, 244, 214, 25);
    //Vector3 euler;

    // Start is called before the first frame update
    void Start()
    {
        night = false;

        emission.color = defaultColor;
        dayNightTimer.SwitchingIcon(night);

        DynamicGI.UpdateEnvironment();

        //euler = directionalLight.transform.eulerAngles;

        //night = !night;
        //switching = true;
        //Night();
        //StartCoroutine(doneSwitching());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameState != GameState.playing)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.C) && !switching)
        {
            night = !night;
            switching = true;

            if (night) Night();
            else Day();

            StartCoroutine(doneSwitching(switchingTime));
            dayNightTimer.SetTimer(switchingTime);
            dayNightTimer.SwitchingIcon(night);
        }
    }

    void Night()
    {
        //directionalLight.transform.eulerAngles = new Vector3(230, euler.y, euler.z);
        directionalLight.color = new Color32(214, 225, 255, 255);
        directionalLight.intensity = 0.5f;

        RenderSettings.skybox = nightSkybox;
        RenderSettings.ambientIntensity = 0.5f;
        RenderSettings.reflectionIntensity = 0.5f;

        emission.color = new Color32(defaultColor.r, defaultColor.g, defaultColor.b, 255);
    }

    void Day()
    {
        //directionalLight.transform.eulerAngles = new Vector3(50, euler.y, euler.z);
        directionalLight.color = new Color32(255, 244, 214, 255);
        directionalLight.intensity = 1.0f;

        RenderSettings.skybox = daySkybox;
        RenderSettings.ambientIntensity = 1.0f;
        RenderSettings.reflectionIntensity = 1.0f;

        emission.color = defaultColor;
    }

    IEnumerator doneSwitching(float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        switching = false;
    }
}
