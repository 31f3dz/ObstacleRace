using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    public static bool inAttack;
    public static bool inDefend;
    bool inDamage;
    bool isDead;

    [SerializeField] GameObject effectDefend;
    [SerializeField] GameObject effectDamage;
    [SerializeField] int hp = 3;
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float sprintSpeed = 10.0f;
    [SerializeField] float jumpHeight = 5.0f;
    [SerializeField] float groundCheckDistance = 0.1f;

    Rigidbody rb;
    Animator anime;
    Quaternion targetRotation;

    float speed;
    bool isGrounded;
    Vector3 move;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anime = GetComponent<Animator>();

        targetRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameState != GameState.playing)
        {
            return;
        }

        if (inAttack || inDamage || isDead) return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
            anime.SetBool("Sprint", true);

        }
        else
        {
            speed = moveSpeed;
            anime.SetBool("Sprint", false);
        }

        float axisH = Input.GetAxisRaw("Horizontal");
        float axisV = Input.GetAxisRaw("Vertical");
        float rotationSpeed = 600 * Time.deltaTime;
        Quaternion axisRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        move = axisRotation * new Vector3(axisH, 0, axisV).normalized;

        if (!inDefend)
        {
            if (move != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(move);
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
        }

        anime.SetBool("Move", move.sqrMagnitude > 0);
        anime.SetFloat("MoveX", axisH);
        anime.SetFloat("MoveZ", axisV);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * Mathf.Abs(Physics.gravity.y));
            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
        }

        anime.SetBool("Ground", isGrounded);

        if (Input.GetButtonDown("Fire1") && isGrounded)
        {
            anime.SetTrigger("Attack");
        }

        if (Input.GetButton("Fire2") && isGrounded)
        {
            inDefend = true;
        }
        else if (Input.GetButtonUp("Fire2") || !isGrounded)
        {
            inDefend = false;
        }

        anime.SetBool("Defend", inDefend);
    }

    void FixedUpdate()
    {
        if (GameController.gameState != GameState.playing)
        {
            return;
        }

        if (inAttack || inDamage) return;

        Vector3 velocity = move * speed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance + 0.1f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Damage") && !inDamage && !isDead)
        {
            GameObject effectHit;

            if (Shield.isHit)
            {
                anime.SetTrigger("Guard");
                effectHit = effectDefend;
            }
            else
            {
                inDefend = false;
                anime.SetBool("Defend", inDefend);

                inDamage = true;
                hp--;

                if (hp <= 0)
                {
                    isDead = true;
                    anime.SetTrigger("Dead");
                    GameController.gameState = GameState.gameover;
                }
                else
                {
                    anime.SetTrigger("Damage");
                }

                StartCoroutine(InDamage());
                effectHit = effectDamage;
            }

            Vector3 midPoint = (transform.position + collision.transform.position) / 2.0f;
            Instantiate(effectHit, midPoint, Quaternion.identity);
        }
    }

    IEnumerator InDamage()
    {
        yield return new WaitForSeconds(0.5f);
        anime.ResetTrigger("Damage");
        inDamage = false;
    }
}
