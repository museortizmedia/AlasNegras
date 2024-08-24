using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float timeToRun = .5f;
    public float gravity = -9.81f;
    public Animator animator;

    [Header("Model Settings")]
    public GameObject model;

    private Vector2 movementInput;
    private CharacterController characterController;
    private float moveTimer;
    private float verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void Update() {
        if (movementInput.magnitude > 0)
        {
            moveTimer += Time.deltaTime;
        }
        else
        {
            moveTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        float currentSpeed = walkSpeed;

        if (movementInput.magnitude > 0 && moveTimer > timeToRun)
        {
            currentSpeed = runSpeed;
            animator.SetFloat("Speed", 2f); // 2 para correr
        }
        else if (movementInput.magnitude > 0)
        {
            animator.SetFloat("Speed", 1f); // 1 para caminar
        }
        else
        {
            animator.SetFloat("Speed", 0f); // 0 para idle
        }

        // Movimiento horizontal
        Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y) * currentSpeed;
        // Convertir a la dirección local del personaje
        move = transform.TransformDirection(move);

        // Aplicar gravedad
        if (characterController.isGrounded)
        {
            verticalVelocity = 0f;
        }
        else
        {
            verticalVelocity += gravity * Time.fixedDeltaTime;
        }

        move.y = verticalVelocity;

        // Mover al personaje
        characterController.Move(move * Time.fixedDeltaTime);

        // Rotar el modelo hacia la dirección del movimiento
        if (movementInput.magnitude > 0)
        {
            // Calcular la rotación hacia la dirección del movimiento
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(move.x, 0f, move.z));
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
    }
}
