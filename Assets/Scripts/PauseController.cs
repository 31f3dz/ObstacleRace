using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    bool inPause;

    [SerializeField] GameObject pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameState != GameState.playing && GameController.gameState != GameState.pause)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!inPause)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    void Pause()
    {
        GameController.gameState = GameState.pause;

        inPause = true;
        pausePanel.SetActive(true);

        Time.timeScale = 0;
    }

    void Resume()
    {
        GameController.gameState = GameState.playing;

        inPause = false;
        pausePanel.SetActive(false);

        Time.timeScale = 1;
    }
}
