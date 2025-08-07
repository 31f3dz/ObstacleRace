using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    BoxCollider swordCollider;

    // Start is called before the first frame update
    void Start()
    {
        swordCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        swordCollider.enabled = PlayerController.inAttack ? true : false;
    }
}
