using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    public int playSceneID = 1; //Change this in insepctor for debugging
    private GameObject PauseUI;
    private GameObject GameOverUI;
    private GameObject WinUI;
    public float drunkFloat = 0f;

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        Win
    }

    public GameState CurrentState;

    private void Awake()
    {
        if (instance != null && instance != this) //Makes sure theres only one GameManager when playing
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // cursorUnlock();
        // GameUnfreeze();
        // //SetState(GameState.MainMenu);
        // SetState(CurrentState);]
        StartGame();
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                ResetElements();
                SceneManager.LoadScene(0);
                cursorUnlock();
                break;

            case GameState.Playing:
                ResetElements();
                cursorLock();
                break;

            case GameState.Paused:
                ResetElements();
                Time.timeScale = 0f;
                PauseUI.SetActive(true);
                cursorUnlock();
                break;

            case GameState.GameOver:
                ResetElements();
                GameOverUI.SetActive(true);
                cursorUnlock();
                GameFreeze();
                break;

            case GameState.Win:
                ResetElements();
                gameWinUpdate();
                WinUI.SetActive(true);
                cursorUnlock();
                break;
        }
    }

    public GameState GetGameState()
    {
        return CurrentState;
    }

    public void AssignElements(GameObject pause, GameObject gameOver, GameObject win)
    {
        PauseUI = pause;
        GameOverUI = gameOver;
        WinUI = win;
    }

    private void ResetElements()
    {
        Time.timeScale = 1f;
        if (PauseUI) PauseUI.SetActive(false);
        if (GameOverUI) GameOverUI.SetActive(false);
        if (WinUI) WinUI.SetActive(false);
    }
    void cursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    } //Centers cursor into middle of screen always

    void cursorUnlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void GameFreeze()
    {
        Time.timeScale = 0;
    }
    void GameUnfreeze()
    {
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;

        SceneManager.LoadScene(playSceneID);
    }


    void gameWinUpdate()
    {
        TextMeshProUGUI winText = WinUI.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        string message = $"You pregamed {drunkFloat * 100:F0}% of the game!";
        winText.text = message;
    }
}
