using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScene : MonoBehaviour
{
    [SerializeField] Image fadeImage;

    Color startColor;
    Color endColor;

    // Start is called before the first frame update
    void Start()
    {
        startColor = fadeImage.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeOut(float fadeTime)
    {
        fadeImage.enabled = true;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeTime);
            fadeImage.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        fadeImage.color = endColor;
        yield return StartCoroutine(FadeIn(fadeTime));
    }

    IEnumerator FadeIn(float fadeTime)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeTime);
            fadeImage.color = Color.Lerp(endColor, startColor, t);
            yield return null;
        }

        fadeImage.color = startColor;
        fadeImage.enabled = false;
    }
}
