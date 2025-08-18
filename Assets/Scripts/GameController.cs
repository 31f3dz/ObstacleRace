using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    playing,
    gameover,
    fade,
}

public class GameController : MonoBehaviour
{
    bool isOver;
    bool isFade;

    public static GameState gameState;
    public static Vector3 retryPos = Vector3.up;

    [SerializeField] FadeScene fadeScene;
    [SerializeField] float fadeTime = 0.5f;

    GameObject player;
    GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.playing;

        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.gameover && !isOver)
        {
            isOver = true;

            StartCoroutine(GameOver());
        }

        if (gameState == GameState.fade && !isFade)
        {
            isFade = true;

            StartCoroutine(fadeScene.FadeOut(fadeTime));
            StartCoroutine(Fade(fadeTime));
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2.0f);
        gameState = GameState.fade;
        isOver = false;
    }

    IEnumerator Fade(float fadeTime)
    {
        yield return new WaitForSeconds(fadeTime);
        player.GetComponent<PlayerController>().Retry();

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<EnemyController>().Retry();
            }
        }

        yield return new WaitForSeconds(fadeTime);
        gameState = GameState.playing;
        isFade = false;
    }
}
