using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public void resume()
    {
        GameManagerScript.instance.SetState(GameManagerScript.GameState.Playing);
    }

    public void mainMenu()
    {
        GameManagerScript.instance.SetState(GameManagerScript.GameState.MainMenu);
    }
    public void loadPlayScene()
    {
        //SceneManager.LoadScene(GameManagerScript.instance.playSceneID);
        GameManagerScript.instance.StartGame();
    }

    public void quitGame()
    { 
        Application.Quit();
    }
}
