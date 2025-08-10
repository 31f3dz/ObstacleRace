using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    playing,
    gameover,
}

public class GameController : MonoBehaviour
{
    public static GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.playing;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
