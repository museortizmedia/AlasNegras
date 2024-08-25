using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInput))]
public class DodgeMovement : MonoBehaviour
{
    public float dodgeDistanceMultiplier = 4f; // Cuántas veces su distancia se moverá durante la esquiva
    public float dodgeDuration = 0.1f; // Tiempo que durará la esquiva en segundos

    public UnityEvent OnInitDodge, OnExitDodge;

    private CharacterController characterController;
    private PlayerMovement movementComponent;
    private PlayerInput playerInput;

    private bool isDodging = false;
    private float dodgeTime = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        movementComponent = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        // Suscribir al evento de input para esquivar
        playerInput.actions["Dodge"].performed += OnDodge;
    }

    private void OnDisable()
    {
        // Desuscribir al evento de input para esquivar
        playerInput.actions["Dodge"].performed -= OnDodge;
    }

    private void Update()
    {
        if (isDodging)
        {
            dodgeTime += Time.deltaTime;
            if (dodgeTime < dodgeDuration)
            {
                // Realizar la esquiva en un solo movimiento rápido
                Vector3 dodgeMovement = dodgeDirection * (dodgeDistanceMultiplier / dodgeDuration) * Time.deltaTime;
                characterController.Move(dodgeMovement);
            }
            else
            {
                // Terminar la esquiva
                isDodging = false;
                movementComponent.CanMove = true; // Reactivar movimiento normal

                OnExitDodge?.Invoke();
            }
        }
    }

    private Vector3 dodgeDirection;
    public void OnDodge(InputAction.CallbackContext context)
    {
        Vector2 inputDirection = playerInput.actions["Move"].ReadValue<Vector2>();
        if (movementComponent.CanMove && (inputDirection.x!= 0 || inputDirection.y!=0))
        {
             // Asumiendo que usas "Move" para obtener la dirección
            dodgeDirection = new Vector3(inputDirection.x, 0, inputDirection.y).normalized;

            // Comenzar la esquiva
            isDodging = true;
            dodgeTime = 0f;
            movementComponent.CanMove = false; // Desactivar movimiento normal durante la esquiva

            OnInitDodge?.Invoke();
        }
    }
}
