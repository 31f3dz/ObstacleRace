using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    bool inAttack;
    bool inDamage;
    bool isDead;
    bool night;

    [SerializeField] BoxCollider weapon;
    [SerializeField] Light nightLight;
    [SerializeField] GameObject effectHit;
    [SerializeField] GameObject effectExplosion;
    [SerializeField] int hp = 3;

    Animator anime;
    Vector3 defaultPos;
    Quaternion defaultRot;

    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        defaultPos = transform.position;
        defaultRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameState != GameState.playing)
        {
            return;
        }

        if (DayNightController.night && !night) Night();
        else if (!DayNightController.night && night) Day();
        night = DayNightController.night;

        if (inAttack || inDamage || isDead) return;
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

    public IEnumerator InAttack()
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

    public void Retry()
    {
        transform.position = defaultPos;
        transform.rotation = defaultRot;
        anime.Rebind();
        anime.Update(0);
    }

    void Night()
    {
        nightLight.enabled = true;
    }

    void Day()
    {
        nightLight.enabled = false;
    }
}
