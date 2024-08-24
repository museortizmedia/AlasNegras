using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FireController : MonoBehaviour
{
    public string mensaje = "FIRE";
    public UnityEvent OnFire1Event;

    [Header("Statics references")]
    [SerializeField] private List<IInteractiveObject> _currentNearObjects = new();

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(mensaje);
            foreach (var interactiveObject in _currentNearObjects)
            {
                interactiveObject.Interactuar(gameObject);
            }
            OnFire1Event?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractiveObject interactiveObject))
        {
            // Asegurarse de que no se añadan duplicados
            if (!_currentNearObjects.Contains(interactiveObject))
            {
                Debug.Log(other.gameObject.name);
                interactiveObject.OnEnterPlayer();
                _currentNearObjects.Add(interactiveObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractiveObject interactiveObject))
        {
            // Eliminar el objeto de la lista si está presente
            if (_currentNearObjects.Contains(interactiveObject))
            {
                interactiveObject.OnExitPlayer();
                _currentNearObjects.Remove(interactiveObject);
            }
        }
    }
}
