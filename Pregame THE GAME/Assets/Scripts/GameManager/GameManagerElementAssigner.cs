using UnityEngine;

public class GameManagerElementAssigner : MonoBehaviour
{
    // This script is glue for the gameManager mainMenu -> gameScene transitions
    [SerializeField] GameObject pause;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject win;

    private void Start()
    {
        if (GameManagerScript.instance == null)
        {
            Debug.LogError("GameManager Missing :("); ;
            return;
        }
        GameManagerScript.instance.AssignElements(pause, gameOver, win); //Assigns gameObjects to gameManager when entering play scene
    }
}
