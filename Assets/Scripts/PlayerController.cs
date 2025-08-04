using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float sprintSpeed = 10.0f;

    Rigidbody rb;
    Animator anime;
    Quaternion targetRotation;

    float speed;
    Vector3 move;

    bool inDamage;

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

        if (inDamage) return;

        speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
        float axisH = Input.GetAxisRaw("Horizontal");
        float axisV = Input.GetAxisRaw("Vertical");
        Quaternion axisRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        move = axisRotation * new Vector3(axisH, 0, axisV).normalized;
        float rotationSpeed = 600 * Time.deltaTime;

        if (move != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(move);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

        anime.SetBool("Move", move.sqrMagnitude > 0);
        anime.SetFloat("Speed", Mathf.Clamp(move.sqrMagnitude * speed, 0, speed));
    }

    void FixedUpdate()
    {
        if (GameController.gameState != GameState.playing)
        {
            return;
        }

        if (inDamage) return;

        Vector3 velocity = move * speed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }
}
