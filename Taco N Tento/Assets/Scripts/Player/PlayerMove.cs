using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public sealed class PlayerMove : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField, Min(0f)] private float velocidadNormal = 4f;
    [SerializeField, Min(0f)] private float velocidadRapida = 7f;

    [Header("Salto y gravedad")]
    [SerializeField, Min(0f)] private float alturaSalto = 1.2f;
    [SerializeField] private float gravedad = -20f;
    [SerializeField] private float fuerzaPegadoAlSuelo = -2f;

    private CharacterController controlador;
    private InputAction movimientoAction;
    private InputAction saltoAction;
    private InputAction sprintAction;
    private float velocidadVertical;

    private void Awake()
    {
        controlador = GetComponent<CharacterController>();

        movimientoAction = new InputAction(
            name: "Movimiento",
            type: InputActionType.Value,
            expectedControlType: "Vector2");

        movimientoAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");
        movimientoAction.AddBinding("<Gamepad>/leftStick");

        saltoAction = new InputAction(name: "Salto", type: InputActionType.Button);
        saltoAction.AddBinding("<Keyboard>/space");
        saltoAction.AddBinding("<Gamepad>/buttonSouth");

        sprintAction = new InputAction(name: "Sprint", type: InputActionType.Button);
        sprintAction.AddBinding("<Keyboard>/leftShift");
        sprintAction.AddBinding("<Gamepad>/leftStickPress");
    }

    private void OnEnable()
    {
        movimientoAction?.Enable();
        saltoAction?.Enable();
        sprintAction?.Enable();
    }

    private void OnDisable()
    {
        movimientoAction?.Disable();
        saltoAction?.Disable();
        sprintAction?.Disable();
    }

    private void OnDestroy()
    {
        movimientoAction?.Dispose();
        saltoAction?.Dispose();
        sprintAction?.Dispose();
    }

    private void Update()
    {
        bool estaEnSuelo = controlador.isGrounded;
        if (estaEnSuelo && velocidadVertical < 0f)
        {
            velocidadVertical = fuerzaPegadoAlSuelo;
        }

        Vector2 entrada = Vector2.ClampMagnitude(movimientoAction.ReadValue<Vector2>(), 1f);
        Vector3 direccion = transform.right * entrada.x + transform.forward * entrada.y;
        float velocidad = sprintAction.IsPressed() ? velocidadRapida : velocidadNormal;

        if (estaEnSuelo && saltoAction.WasPressedThisFrame())
        {
            velocidadVertical = Mathf.Sqrt(alturaSalto * -2f * gravedad);
        }

        velocidadVertical += gravedad * Time.deltaTime;
        Vector3 desplazamiento = direccion * velocidad + Vector3.up * velocidadVertical;
        controlador.Move(desplazamiento * Time.deltaTime);
    }
}