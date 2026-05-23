using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private Animator animator;

    void OnEnable()
    {
        playerScript.drinking += playDrinkAnim;
    }

    void OnDisable()
    {
        playerScript.drinking -= playDrinkAnim;
    }

    void playDrinkAnim()
    {
        animator.Play("Drinking");
    }
}
