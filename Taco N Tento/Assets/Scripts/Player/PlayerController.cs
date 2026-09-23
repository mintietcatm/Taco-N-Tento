using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))] //El script no funciona si no tiene character controller el jugador
public class PlayerController : MonoBehaviour
{

    [SerializeField] Transform player;
    #region CharacterMovement

    public float walkSpeed = 5f;
    private float verticalVelocity = 0f;
    public float gravity = -9.81f; //porque usar un rigidbody me rompe todo por alguna razon

    #endregion

    #region Camera

    public Transform cameraTransform;
    public float mouseSensitivity = 2f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float xRotation = 0f;
    #endregion 


    public void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        RotateCamera();
        PlayerMove();
    }

    private void RotateCamera()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue(); //Vector 2. porque guarda DOS valores. ReadValue() lee el valor actual del mouse
     
        float mouseX =mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        player.Rotate(Vector3.up * mouseX);
    }

    private void PlayerMove()
    {
        moveDirection = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            moveDirection += player.forward;

        if (Keyboard.current.sKey.isPressed)
            moveDirection -= player.forward;

        if (Keyboard.current.dKey.isPressed)
            moveDirection += player.right;

        if (Keyboard.current.aKey.isPressed)
            moveDirection -= player.right;

        if (Keyboard.current.leftShiftKey.isPressed)
            walkSpeed = 10f;
        else
            walkSpeed = 5f;

        //Gravedad, para no poner un rigidbody que se rompe todo xd
        verticalVelocity += gravity * Time.deltaTime;

        moveDirection.y = verticalVelocity;


        characterController.Move(moveDirection * walkSpeed * Time.deltaTime);

           }
}
