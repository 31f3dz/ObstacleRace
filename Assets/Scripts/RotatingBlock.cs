using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBlock : MonoBehaviour
{
    [SerializeField] float rotateSpeed = -36.0f;
    [SerializeField] float maxTiltAngle = 30.0f;
    [SerializeField] float followSpeed = 5.0f;

    Rigidbody playerRb;

    bool isOnCube;
    Vector3 localPosOnCube;

    void Start()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotateSpeed) * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (!isOnCube) return;

        float tiltZ = Mathf.Abs(Mathf.DeltaAngle(0, transform.eulerAngles.z));

        if (tiltZ <= maxTiltAngle)
        {
            //Quaternion targetRot = Quaternion.Euler(0, playerRb.rotation.eulerAngles.y, transform.eulerAngles.z);
            //playerRb.MoveRotation(Quaternion.Slerp(playerRb.rotation, targetRot, followSpeed * Time.fixedDeltaTime));

            Vector3 targetWorldPos = transform.TransformPoint(localPosOnCube);
            Vector3 newPos = Vector3.Lerp(playerRb.position, targetWorldPos, followSpeed * Time.fixedDeltaTime);
            playerRb.MovePosition(newPos);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOnCube = true;

            localPosOnCube = transform.InverseTransformPoint(playerRb.position);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            localPosOnCube = transform.InverseTransformPoint(playerRb.position);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOnCube = false;
        }
    }
}
