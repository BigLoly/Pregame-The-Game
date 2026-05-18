using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Splines.Interpolators;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float moveSpeed; //User Interactions
    [SerializeField] float mouseSensitivity;
    [SerializeField] KeyCode drinkButton;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform playerTransform;

    public LiquidScript liquidScript; //liquidScript
    [SerializeField] float drinkRate;

    float originalMoveSpeed;
    float slowMoveSpeed;

    private void Start()
    {
        cursorLock();
        originalMoveSpeed = moveSpeed;
        slowMoveSpeed = originalMoveSpeed * .50f;
    }

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
        }
        else
        {
            liquidScript.onScreen = false;
            moveSpeed = originalMoveSpeed; //Returns movespeed to original value while not drinking
        }
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
