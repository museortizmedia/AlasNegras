using UnityEngine;
using UnityEngine.EventSystems;

namespace DiegoOrtizLibrary{
    public class Joystick : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("JoystickDrag")]
        public RectTransform Shaft;
        public float maxDistance = 50f;

        private Vector2 joystickPosition;
        private RectTransform Rect;

        void Start()
        {
            Rect = GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdateJoystickPosition(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetJoystickPosition();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            UpdateJoystickPosition(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ResetJoystickPosition();
        }

        private void UpdateJoystickPosition(Vector2 inputPosition)
        {
            // Obtener la posición del puntero en relación con el centro de la imagen
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Rect, inputPosition, null, out Vector2 localPointerPosition);

            // Limitar la posición dentro del círculo
            joystickPosition = Vector2.ClampMagnitude(localPointerPosition, maxDistance);

            // Asignar la posición al Shaft
            Shaft.anchoredPosition = joystickPosition;
        }
        private void ResetJoystickPosition()
        {
            // Devolver el Shaft al centro
            Shaft.anchoredPosition = Vector2.zero;

            // Reiniciar la posición del joystick
            joystickPosition = Vector2.zero;
        }

        public Vector2 GetJoystickPosition()
        {
            // Normalizar la posición del joystick y ajustar el rango de valores
            float horizontal = Mathf.Clamp(joystickPosition.x / maxDistance, -1f, 1f);
            float vertical = Mathf.Clamp(joystickPosition.y / maxDistance, -1f, 1f);

            return new Vector2(horizontal, vertical);
        }
    }
}
