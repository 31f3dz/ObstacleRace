using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    bool isCheck;

    [SerializeField] Vector3 checkPointPos = new Vector3(0, Vector3.up.y, 20);
    [SerializeField] GameObject MessagePanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isCheck)
        {
            isCheck = true;

            GameController.retryPos = checkPointPos;
            MessagePanel.SetActive(true);

            StartCoroutine(MessageFade());
        }
    }

    IEnumerator MessageFade()
    {
        yield return new WaitForSeconds(2);
        MessagePanel.SetActive(false);
    }
}
