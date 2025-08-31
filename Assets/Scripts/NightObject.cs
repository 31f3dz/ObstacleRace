using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NightObject : MonoBehaviour
{
    BoxCollider boxCollider;
    NavMeshObstacle obstacle;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        obstacle = GetComponent<NavMeshObstacle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DayNightController.night)
        {
            boxCollider.enabled = true;

            if (obstacle != null)
            {
                obstacle.enabled = true;
            }
        }
        else
        {
            boxCollider.enabled = false;

            if (obstacle != null)
            {
                obstacle.enabled = false;
            }
        }
    }
}
