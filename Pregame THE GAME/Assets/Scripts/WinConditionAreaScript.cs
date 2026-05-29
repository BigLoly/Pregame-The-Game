using UnityEngine;

public class WinConditionAreaScript : MonoBehaviour
{
    private BoxCollider boxCollider;

    private MeshRenderer debugRender; //For debugging
    private void Start()
    {
        debugRender = GetComponent<MeshRenderer>(); //Hides rendered cube during playtime
        debugRender.enabled = false;
    }

    private void PlayerTouched() //Sets GameState to win
    {
        GameManagerScript.instance.SetState(GameManagerScript.GameState.Win);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        { 
            PlayerTouched();
        }
    }
}
