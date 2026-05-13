using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float mouseSensitivity;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform playerTransform;

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
    }

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
    }

    void cursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        cursorLock();
    }
    void Update()
    {
        move();
        rotate();
    }
}
