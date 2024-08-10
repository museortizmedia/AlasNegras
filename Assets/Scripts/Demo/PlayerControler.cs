using System.Collections;
using Cinemachine;
using UnityEngine;

namespace DiegoOrtizLibrary
{
    public interface IPlayerControles
    {
        void MovementHandle();
    }
    public class PlayerControler : MonoBehaviour, IPlayerControles
    {
        public bool IsAndroidMode;
        [SerializeField] CharacterController _controller;
        [Header("Camera Setting")]
        public CinemachineFreeLook CinemachineFreeLookCamera;
        [SerializeField] float _smoothCamera = 1f; // Duración de la transición de giro
        float transitionTimer = 0f;
        bool _isCameraWaiting;
        const float _waitToMoveCamera = 0f; //delay antes de animacion
        public bool IsContinuosCameraToBack;
        public float segundosParaMoverAtrasMientrasCamina = 2f;

        [Header("Controller Settings")]

        public float normalSpeed = 5f;
        public float sprintSpeed = 10f;
        bool _isSprint;
        public float jumpForce = 8f;
        public float groundDistance = 1.2f;
        public LayerMask groundMask, waterMask;
        public float gravity = 20f;
        [SerializeField] private bool isGrounded = false, isWater = false;

        [Header("Graphic Settings")]
        [SerializeField] Transform modelTransform;
        [SerializeField] Animator _animator;
        public float speedWalk = 100f;

        [Header("Android Settings")]
        [SerializeField] Transform UI_Canvas;
        [SerializeField] Joystick _joystick;

        Vector3 cameraForward;
        private Vector3 _velocity;

        void Start()
        {
#if UNITY_ANDROID
            IsAndroidMode = true;
#endif            
        }

        void OnEnable()
        {
            ToogleUI(true);
        }
        private void OnDisable()
        {
            if (_animator != null) { _animator.SetBool("Sprint", false); _animator.SetFloat("Speed", 0); }
            ToogleUI(false);
        }

        void Update()
        {
            CheckGrounded();
            CheckWaterSpace();
            MovementHandle();
            if (Input.GetButtonDown("Jump")) { Jump(); }
            ApplyGravity();
            _controller.Move(_velocity * Time.deltaTime);
        }


        public void ToogleUI(bool state)
        {
            if (IsAndroidMode)
                UI_Canvas.gameObject.SetActive(state);
        }

        float segundosActualesParaMoverAtrasMientrasCamina;
        public void MovementHandle()
        {
            Vector2 joystickPosition = _joystick.GetJoystickPosition();
            float moveHorizontal = Input.GetAxis("Horizontal") + joystickPosition.x;
            float moveVertical = Input.GetAxis("Vertical") + joystickPosition.y;

            // Obtenemos la rotación de la cámara solo mientras no estemos moviendonos (almacena el fordware)
            if (!(moveHorizontal != 0 || moveVertical != 0))
            {
                //si no se está moviendo
                cameraForward = Camera.main.transform.forward;
                cameraForward.y = 0f;


                CameraToBack();
            }
            else //sueltan teclas
            {
                if (IsContinuosCameraToBack)
                {
                    segundosActualesParaMoverAtrasMientrasCamina += Time.deltaTime;
                    if (segundosActualesParaMoverAtrasMientrasCamina >= segundosParaMoverAtrasMientrasCamina)
                    {
                        CameraToBack();
                        segundosActualesParaMoverAtrasMientrasCamina = 0;
                        return;
                    }
                } else {
                    _isCameraWaiting = false;
                    StopAllCoroutines();
                }
            }

            Quaternion cameraRotation = Quaternion.LookRotation(cameraForward);

            // Aplicamos la rotación de la cámara a la dirección de movimiento
            Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
            movement = cameraRotation * movement;
            movement = movement.normalized;

            // Aplicamos la velocidad de movimiento
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) || _isSprint ? sprintSpeed : normalSpeed;
            movement *= currentSpeed * Time.deltaTime;

            // Movemos el personaje
            _controller.Move(transform.TransformDirection(movement));

            // Rotamos el modelo del personaje hacia la dirección de movimiento
            if (movement.magnitude > 0 && modelTransform != null)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
                modelTransform.rotation = Quaternion.RotateTowards(modelTransform.rotation, targetRotation, Time.deltaTime * 720f);
            }

            //ANIM
            _animator.SetBool("Sprint", Input.GetKey(KeyCode.LeftShift) || _isSprint);

            // Actualizamos la animación de acuerdo a la velocidad de movimiento
            if (_animator != null)
            {
                _animator.SetFloat("Speed", movement.magnitude * speedWalk);
            }
        }

        public RaycastHit hitFront, hitBack;

        void CheckGrounded()
        {
            bool isGroundedFront = Physics.Raycast(transform.position + transform.forward * 0.5f, Vector3.down, out hitFront, groundDistance, groundMask);
            bool isGroundedBack = Physics.Raycast(transform.position - transform.forward * 0.5f, Vector3.down, out hitBack, groundDistance, groundMask);
            bool isGroundedRight = Physics.Raycast(transform.position + transform.right * 0.5f, Vector3.down, out hitBack, groundDistance, groundMask);
            bool isGroundedLeft = Physics.Raycast(transform.position - transform.right * 0.5f, Vector3.down, out hitBack, groundDistance, groundMask);


            isGrounded = isGroundedFront || isGroundedBack || isGroundedRight || isGroundedLeft;
            Debug.DrawRay(transform.position + transform.forward * 0.5f, Vector3.down * groundDistance, isGrounded ? Color.blue : Color.green, 1f);
            Debug.DrawRay(transform.position - transform.forward * 0.5f, Vector3.down * groundDistance, isGrounded ? Color.blue : Color.green, 1f);
            Debug.DrawRay(transform.position + transform.right * 0.5f, Vector3.down * groundDistance, isGrounded ? Color.blue : Color.green, 1f);
            Debug.DrawRay(transform.position - transform.right * 0.5f, Vector3.down * groundDistance, isGrounded ? Color.blue : Color.green, 1f);
            if (isGrounded) { _animator.SetBool("Jump", false); }
        }

        void ApplyGravity()
        {
            if (_controller.isGrounded && _velocity.y < 0)
                _velocity.y = -2f;
            else
                _velocity.y -= gravity * Time.deltaTime;
        }
        public void Jump()
        {
            if (isGrounded)
            {

                _velocity.y = Mathf.Sqrt(2 * jumpForce * gravity);
                Invoke(nameof(LateJump), 0.05f);

            }
        }
        public void LateJump()
        {
            _animator.SetBool("Jump", true);
        }

        public void ApplySprintSpeed(bool state)
        {
            _isSprint = state;
        }

        //water
        void CheckWaterSpace()
        {
            isWater = Physics.Raycast(transform.position, Vector3.down, groundDistance, waterMask);
            if (_animator != null) { _animator.SetBool("Swiming", isWater); }
        }

        //CAMERA

        void CameraToBack()
        {
            if (!_isCameraWaiting)
            {
                Invoke(nameof(Animate), _waitToMoveCamera);
                _isCameraWaiting = true;
            }
        }
        void Animate()
        {
            StartCoroutine(AnimateCamera());
        }
        public void Animate(float angle)
        {
            StartCoroutine(AnimateCamera(angle));
        }


        IEnumerator AnimateCamera(float? especificAngle = null)
        {
            float currentBias = CinemachineFreeLookCamera.m_Heading.m_Bias;
            float targetBias = especificAngle == null ? modelTransform.rotation.eulerAngles.y : (float)especificAngle;

            // Normaliza los ángulos entre -180 y 180
            currentBias = NormalizeAngle(currentBias);
            targetBias = NormalizeAngle(targetBias);

            // Calcula la menor diferencia de ángulo
            float angleDifference = Mathf.DeltaAngle(currentBias, targetBias);

            // Calcula el ángulo objetivo real para el lerp
            float realTargetBias = currentBias + angleDifference;

            // Restablece el temporizador de transición
            transitionTimer = 0f;

            // Incrementa el temporizador de transición en cada frame
            while (transitionTimer < _smoothCamera)
            {
                // Calcula la interpolación lineal entre el valor actual y el objetivo
                float biasValue = Mathf.Lerp(currentBias, realTargetBias, transitionTimer / _smoothCamera);

                // Asigna el valor interpolado a m_Bias
                CinemachineFreeLookCamera.m_Heading.m_Bias = NormalizeAngle(biasValue);

                // Incrementa el temporizador de transición
                transitionTimer += Time.deltaTime;

                // Espera al siguiente frame
                yield return null;
            }

            // Asigna el ángulo final normalizado
            CinemachineFreeLookCamera.m_Heading.m_Bias = targetBias;

            // Restablece la bandera de espera de la cámara
            _isCameraWaiting = false;
        }

        // Función para normalizar los ángulos entre -180 y 180
        float NormalizeAngle(float angle)
        {
            angle %= 360;
            if (angle > 180)
            {
                angle -= 360;
            }
            else if (angle < -180)
            {
                angle += 360;
            }
            return angle;
        }

    }
}