using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public static bool isHit;

    BoxCollider shieldCollider;

    // Start is called before the first frame update
    void Start()
    {
        shieldCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        shieldCollider.enabled = PlayerController.inDefend ? true : false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            isHit = true;
            StartCoroutine(IsHit());
        }
    }

    IEnumerator IsHit()
    {
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }
}
