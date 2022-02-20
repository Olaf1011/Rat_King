using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MENU,
        PLAYING,
        PAUSED,
        GAMEOVER
    };

    public enum SubState
    {
        TUTORIAL,
        MAIN_GAME
    }

    public static GameManager Instance => instance;
    public GameState sGameState = GameState.PLAYING;
    public SubState sSubState = SubState.TUTORIAL;
    public GameObject pauseUI;
    public GameObject gameOverUI;

    public GameObject blueWinsImg;
    public GameObject redWinsImg;

    private static GameManager instance;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        Time.timeScale = 1.0f;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(sGameState == GameState.PLAYING)
            {
                PauseGame();
            }
            else if(sGameState == GameState.PAUSED)
            {
                PlayGame();
            }
        }
    }

    public void PlayGame()
    {
        LevelScript ls = GameObject.FindGameObjectWithTag("LevelScript").GetComponent<LevelScript>();
        SoundManager sm = SoundManager.Instance;

        sm.UnPauseAll();
        ls.SetBackgroundVolume(1);
        sm.PlaySound(SoundManager.SoundNames.gameUnpaused);

        sGameState = GameState.PLAYING;
        Time.timeScale = 1.0f;
        pauseUI.SetActive(false);
    }

    public void PauseGame()
    {
        LevelScript ls = GameObject.FindGameObjectWithTag("LevelScript").GetComponent<LevelScript>();
        SoundManager sm = SoundManager.Instance;
        sm.PauseAll();
        sm.PlaySound(SoundManager.SoundNames.gamePaused);
        //ls.StopBackgroundNoise(); //If you do notwant background on pause
        ls.SetBackgroundVolume(0.1f);

        sGameState = GameState.PAUSED;
        Time.timeScale = 0.0f;
        pauseUI.SetActive(true);

    }

    public void EndGame(bool leftSideWon)
    {
        if (sGameState == GameState.GAMEOVER)
        {
            return;
        }

        SoundManager sm = SoundManager.Instance;
        LevelScript ls = GameObject.FindGameObjectWithTag("LevelScript").GetComponent<LevelScript>();
        sm.StopAll();
        sm.PlaySound(SoundManager.SoundNames.postGameSummary);

        sGameState = GameState.GAMEOVER;
        Time.timeScale = 0.0f;


        gameOverUI.SetActive(true);
        if(leftSideWon)
        {
            blueWinsImg.SetActive(true);
        }
        else
        {
            redWinsImg.SetActive(true);
        }
        
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        sGameState = GameState.PLAYING;
        SoundManager.Instance.StopAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Credits()
    {
        SoundManager.Instance.StopAll();
        SceneManager.LoadScene("Credits");
    }
    
        
    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
    }
    
}
