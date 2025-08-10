using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //bool inAttack;
    bool isAttack;
    bool inDamage;
    bool isDead;

    [SerializeField] BoxCollider weapon;
    [SerializeField] GameObject effectHit;
    [SerializeField] GameObject effectExplosion;
    [SerializeField] int hp = 3;

    Animator anime;

    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && hp <= 0)
        {
            isDead = true;
            anime.SetTrigger("Dead");
            StartCoroutine(IsDead());
        }
        else
        {
            if (!isAttack) StartCoroutine(IsAttack());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack") && !inDamage && !isDead)
        {
            inDamage = true;
            hp--;
            anime.SetTrigger("Damage");
            StartCoroutine(InDamage());

            Vector3 midPoint = (transform.position + other.transform.position) / 2.0f;
            Instantiate(effectHit, midPoint, Quaternion.identity);
        }
    }

    IEnumerator IsAttack()
    {
        isAttack = true;
        yield return StartCoroutine(InAttack());
        yield return new WaitForSeconds(5);
        isAttack = false;
    }

    IEnumerator InAttack()
    {
        //inAttack = true;
        anime.SetTrigger("Attack");
        weapon.enabled = true;
        yield return new WaitForSeconds(0.5f);

        //inAttack = false;
        weapon.enabled = false;
    }

    IEnumerator InDamage()
    {
        yield return new WaitUntil(() => PlayerController.inAttack == false);
        inDamage = false;
    }

    IEnumerator IsDead()
    {
        yield return new WaitForSeconds(1);
        Instantiate(effectExplosion, transform.position, Quaternion.Euler(-90, 0, 0));
        Destroy(gameObject);
    }
}
