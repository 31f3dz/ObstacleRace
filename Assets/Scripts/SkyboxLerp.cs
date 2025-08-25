using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxLerp : MonoBehaviour
{
    [SerializeField] Light directionalLight;

    [SerializeField] Material daySkybox; // 昼用の.mat
    [SerializeField] Material nightSkybox; // 夜用の.mat

    [Range(0f, 1f)]
    [SerializeField] float blend = 0f; // 0=昼, 1=夜

    void Update()
    {
        directionalLight.transform.Rotate(new Vector3(-18, 0, 0) * Time.deltaTime);

        // blend を時間で変化させる
        blend = Mathf.PingPong(Time.time * 0.05f, 1f);

        // 2つのマテリアルを補間
        RenderSettings.skybox.Lerp(daySkybox, nightSkybox, blend);

        // 反映を即時更新
        // DynamicGI.UpdateEnvironment();
    }
}
