using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    bool isMove = true; // 移動可能フラグ
    bool isReverse; // 方向反転フラグ

    [SerializeField] Vector3 move = new Vector3(-15.0f, 2.0f, 15.0f); // 移動距離
    [SerializeField] float time = 3.0f; // 移動時間
    [SerializeField] float wait = 1.0f; // 到着～方向反転のインターバル

    Vector3 startPos; // 初期位置
    Vector3 endPos; // 移動位置

    float distance; // 初期位置と移動位置の差
    float secondDistance; // 1秒あたりの移動距離
    float frameDistance; // 1フレームあたりの移動距離
    float movePercentage; // 移動位置までの進捗割合

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = startPos + move;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            distance = Vector3.Distance(startPos, endPos);
            secondDistance = distance / time;
            frameDistance = secondDistance * Time.deltaTime;
            movePercentage += frameDistance / distance;

            if (!isReverse)
            {
                transform.position = Vector3.Lerp(startPos, endPos, movePercentage);
            }
            else
            {
                transform.position = Vector3.Lerp(endPos, startPos, movePercentage);
            }

            if (movePercentage >= 1.0f)
            {
                isMove = false;
                isReverse = !isReverse;
                movePercentage = 0;

                StartCoroutine(Move());
            }
        }
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(wait);
        isMove = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
