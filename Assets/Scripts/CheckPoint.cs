using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] Vector3 checkPointPos = new Vector3(0, Vector3.up.y, 20);

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
        GameController.retryPos = checkPointPos;
        Debug.Log("チェックポイントに到達");
    }
}
