using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Splines.Interpolators;
using UnityEngine.Timeline;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float moveSpeed; //Player Stats
    [SerializeField] float mouseSensitivity;
    [SerializeField] float drunkinessLevel = 0f; //Public so animator controller can use this for blendtrees

    [SerializeField] KeyCode drinkButton; //User Interactions
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform playerTransform;

    public LiquidScript liquidScript; //liquidScript found in canvas > AlcoholMeter
    [SerializeField] float drinkRate;

    [SerializeField] Animator animator; //For animations

    private void Start()
    {
        cursorLock();
        originalMoveSpeed = moveSpeed;
        slowMoveSpeed = originalMoveSpeed * .50f;
    }

    float originalMoveSpeed;
    float slowMoveSpeed;
    void move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 direction = (forward * z + right * x).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        animator.SetFloat("moveX", x); //Sets floats in animator controller
        animator.SetFloat("moveZ", z); //ANIMATIOR CONTROLLER CAN BE POLISHED WITH LEFT AND RIGHT WALKING ANIMATIONS

    } //Simple WASD movement

    void rotate()
    {
        Vector3 dir = cameraTransform.forward;
        dir.y = 0f;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        playerTransform.rotation = Quaternion.Slerp(
            playerTransform.rotation,
            targetRot,
            5 * Time.deltaTime
            );
    } //Rotates player witht camera (Player always looks forward)

    void drinkAlcohol() //Reduces alcohol level when press drink button
    {
        if (Input.GetKey(drinkButton) && liquidScript.liquidAmount > 0)
        {
            liquidScript.ReduceLiquid(drinkRate * Time.deltaTime);
            liquidScript.onScreen = true;
            moveSpeed = slowMoveSpeed; //Reduces movespeed by float while drinking

            drunkinessMechanic(); //Increases drunkeness float
            animator.SetBool("isDrinking", true); //Plays animation
        }
        else
        {
            liquidScript.onScreen = false;
            moveSpeed = originalMoveSpeed; //Returns movespeed to original value while not drinking
            animator.SetBool("isDrinking", false); //Doesn't play animation
        }
    }

    public float drunkinessMechanic()
    {
        drunkinessLevel += drinkRate * Time.deltaTime;
        float normalizedDrunkLevel = drunkinessLevel / 100; // divide by 100 to normalize float to 0-1 scale.
        animator.SetFloat("drunkiness", normalizedDrunkLevel);
        return normalizedDrunkLevel; 
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

    void pauseGame() //Press escape to pause/resume game
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManagerScript.instance.GetGameState() == GameManagerScript.GameState.Paused) //If in pause menu and press escape again it exits back to playing state
            {
                cursorLock();
                GameManagerScript.instance.SetState(GameManagerScript.GameState.Playing);
            }
            else
            {
                cursorUnlock();
                GameManagerScript.instance.SetState(GameManagerScript.GameState.Paused); //Else it pauses game
            }
        }
    }

    void Update()
    {
        move();
        rotate();
        drinkAlcohol();
        pauseGame();
    }
}
