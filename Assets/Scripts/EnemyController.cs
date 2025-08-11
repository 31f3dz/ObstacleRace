using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    bool isAttack;
    bool inAttack;
    bool inDamage;
    bool isDead;

    [SerializeField] BoxCollider weapon;
    [SerializeField] GameObject effectHit;
    [SerializeField] GameObject effectExplosion;
    [SerializeField] int hp = 3;

    GameObject target;
    NavMeshAgent agent;
    Animator anime;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameState != GameState.playing)
        {
            return;
        }

        if (inAttack || inDamage || isDead) return;

        agent.SetDestination(target.transform.position);
        anime.SetBool("Move", agent.velocity.sqrMagnitude > 0);

        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance <= agent.stoppingDistance)
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

            if (hp <= 0)
            {
                isDead = true;
                anime.SetTrigger("Dead");
                StartCoroutine(IsDead());
            }
            else
            {
                anime.SetTrigger("Damage");
            }

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
        inAttack = true;
        anime.SetTrigger("Attack");
        weapon.enabled = true;
        yield return new WaitForSeconds(0.5f);

        weapon.enabled = false;
        inAttack = false;
    }

    IEnumerator InDamage()
    {
        yield return new WaitUntil(() => PlayerController.inAttack == false);
        anime.ResetTrigger("Damage");
        inDamage = false;
    }

    IEnumerator IsDead()
    {
        yield return new WaitForSeconds(1);
        Instantiate(effectExplosion, transform.position, Quaternion.Euler(-90, 0, 0));
        Destroy(gameObject);
    }
}
