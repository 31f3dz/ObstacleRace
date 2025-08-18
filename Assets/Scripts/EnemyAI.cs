using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum State
{
    patrol,
    chase,
}

public class EnemyAI : MonoBehaviour
{
    bool isAttack;
    bool night;

    [SerializeField] EnemyController enemyController;
    [SerializeField] float detectionRange = 10.0f;
    [SerializeField] float patrolRadius = 5.0f;
    [SerializeField] float patrolInterval = 3.0f;
    [SerializeField] float attackDuration = 5.0f;

    GameObject target;
    NavMeshAgent agent;
    Animator anime;
    State currentState;
    Vector3 startPosition;
    float patrolTimer;
    float defSpeed;
    float defAngularSpeed;
    float defAcceleration;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        anime = GetComponent<Animator>();

        currentState = State.patrol;
        startPosition = transform.position;
        patrolTimer = patrolInterval;

        defSpeed = agent.speed;
        defAngularSpeed = agent.angularSpeed;
        defAcceleration = agent.acceleration;
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

        float distance = Vector3.Distance(target.transform.position, transform.position);

        Vector3 direction = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float rotationSpeed = 600 * Time.deltaTime;

        if (distance <= agent.stoppingDistance)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

            if (!isAttack) StartCoroutine(IsAttack());
        }

        if (distance <= detectionRange) currentState = State.chase;
        else currentState = State.patrol;

        if (currentState == State.patrol) Patrol();
        else if (currentState == State.chase) Chase();

        anime.SetBool("Move", agent.velocity.sqrMagnitude > 0);
    }

    void Patrol()
    {
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolInterval)
        {
            Vector3 randomPos = RandomNavMeshLocation(patrolRadius);
            agent.SetDestination(randomPos);
            patrolTimer = 0;
        }
    }

    void Chase()
    {
        agent.SetDestination(target.transform.position);
    }

    Vector3 RandomNavMeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius + startPosition;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    IEnumerator IsAttack()
    {
        isAttack = true;
        yield return StartCoroutine(enemyController.InAttack());
        yield return new WaitForSeconds(attackDuration);
        isAttack = false;
    }

    void Night()
    {
        attackDuration /= 2;
        agent.speed = defSpeed * 2;
        agent.angularSpeed = defAngularSpeed * 2;
        agent.acceleration = defAcceleration * 2;
    }

    void Day()
    {
        attackDuration *= 2;
        agent.speed = defSpeed;
        agent.angularSpeed = defAngularSpeed;
        agent.acceleration = defAcceleration;
    }
}
