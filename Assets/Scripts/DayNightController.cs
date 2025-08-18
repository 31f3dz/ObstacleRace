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

    Color32 defaultColor = new Color32(255, 244, 214, 25);
    //Vector3 euler;

    // Start is called before the first frame update
    void Start()
    {
        emission.color = defaultColor;

        //euler = directionalLight.transform.eulerAngles;

        //night = !night;
        //switching = true;
        //Night();
        //StartCoroutine(doneSwitching());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C) && !switching)
        {
            night = !night;
            switching = true;

            if (night) Night();
            else Day();

            StartCoroutine(doneSwitching());
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

    IEnumerator doneSwitching()
    {
        yield return new WaitForSeconds(5);
        switching = false;

        Debug.Log("昼夜の切り替えが可能になった");
    }
}
