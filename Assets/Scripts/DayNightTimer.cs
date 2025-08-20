using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightTimer : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] Image icon;
    [SerializeField] Sprite nightIcon;
    [SerializeField] Sprite dayIcon;

    // Start is called before the first frame update
    void Start()
    {
        fill.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTimer(float coolTime)
    {
        fill.fillAmount = 1;
        StartCoroutine(CountTimer(coolTime));
    }

    IEnumerator CountTimer(float coolTime)
    {
        float count = coolTime;

        while (count >= 0)
        {
            count -= Time.deltaTime;
            fill.fillAmount = count / coolTime;
            yield return null;
        }
    }

    public void SwitchingIcon(bool night)
    {
        if (night)
        {
            icon.sprite = nightIcon;
        }
        else
        {
            icon.sprite = dayIcon;
        }
    }
}
