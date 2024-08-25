using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FireController : MonoBehaviour
{
    [Tooltip("Objeto que reportará ante la interacción")]
    public GameObject Reporter; 
    public UnityEvent OnSuccesFireEvent, OnTryFireEvent;

    [Header("Statics references")]
    [SerializeField] private List<IInteractiveObject> _currentNearObjects = new();

    private void Start() {
        if(Reporter==null){Reporter = transform.parent.gameObject; }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            foreach (var interactiveObject in _currentNearObjects)
            {
                interactiveObject.Interactuar(Reporter);
                OnSuccesFireEvent?.Invoke();
            }
            OnTryFireEvent?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractiveObject interactiveObject))
        {
            // Asegurarse de que no se añadan duplicados
            if (!_currentNearObjects.Contains(interactiveObject))
            {
                //Debug.Log(other.gameObject.name);
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
