using System.ComponentModel;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            GameManagerScript.instance.SetState(GameManagerScript.GameState.GameOver);
            //GameManagerScript.instance.StartGame();
        }
    }
}
