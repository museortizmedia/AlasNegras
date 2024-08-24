using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FireController : MonoBehaviour
{
    public string mensaje = "FIRE";
    public UnityEvent OnFire1Event;

    [Header("Statics references")]
    [SerializeField] private List<GameObject> _currentNearObjects = new List<GameObject>();

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(mensaje);
            foreach (var nearObject in _currentNearObjects)
            {
                if (nearObject.TryGetComponent(out InteractiveObject interactiveObject))
                {
                    interactiveObject.Interactuar(gameObject);
                }
            }
            OnFire1Event?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Asegurarse de que no se añadan duplicados
        if (!_currentNearObjects.Contains(other.gameObject))
        {
            Debug.Log(other.gameObject.name);
            _currentNearObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Eliminar el objeto de la lista si está presente
        if (_currentNearObjects.Contains(other.gameObject))
        {
            _currentNearObjects.Remove(other.gameObject);
        }
    }
}
