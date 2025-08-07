using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    bool inDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Attack") && !inDamage)
        {
            Debug.Log("敵に攻撃が当たった");
            inDamage = true;
            StartCoroutine(InDamage());
        }
    }

    IEnumerator InDamage()
    {
        yield return new WaitUntil(() => PlayerController.inAttack == false);
        inDamage = false;
    }
}
