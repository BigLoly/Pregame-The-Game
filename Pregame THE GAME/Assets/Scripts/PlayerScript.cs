using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Splines.Interpolators;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float moveSpeed; //Player Stats
    [SerializeField] float mouseSensitivity;
    public float drunkinessLevel = 0f; //Public so animator controller can use this for blendtrees

    [SerializeField] private KeyCode drinkButton; //User Interactions
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerTransform;

    public LiquidScript liquidScript; //liquidScript
    [SerializeField] private float drinkRate;

    public event Action drinking; //For animations
    [SerializeField] private Animator animator;

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

        Vector3 direction = forward * z + right * x;
        transform.position += direction * moveSpeed * Time.deltaTime;

        animator.SetFloat("velocity", new Vector3 (direction.x, 0, direction.z).magnitude); //Sets isMoving float in anim controller

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
        if (Input.GetKey(drinkButton))
        {
            liquidScript.ReduceLiquid(drinkRate * Time.deltaTime);
            liquidScript.onScreen = true;
            moveSpeed = slowMoveSpeed; //Reduces movespeed by float while drinking

            drunkinessMechanic(); //Increases drunkeness float
            animator.SetTrigger("drink"); //sets trigger for animation
        }
        else
        {
            liquidScript.onScreen = false;
            moveSpeed = originalMoveSpeed; //Returns movespeed to original value while not drinking
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


    void Update()
    {
        move();
        rotate();
        drinkAlcohol();
    }
}
