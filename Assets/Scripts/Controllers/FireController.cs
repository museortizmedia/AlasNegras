using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FireController : MonoBehaviour
{
    [Tooltip("Objeto que reportará ante la interacción")]
    public GameObject Reporter;
    public bool IsHit;
    public UnityEvent OnSuccesFireEvent, OnTryFireEvent;

    public UnityEvent OnStartedFireEvent, OnCanceledFireEvent;

    [Header("Statics references")]
    [SerializeField] private List<GameObject> _currentNearObjectsReadOnly = new();
    private List<IInteractiveObject> _currentNearObjects = new();

    private void Start() {
        if(Reporter==null){Reporter = transform.parent.gameObject; }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started){OnStartedFireEvent?.Invoke();}
        if (context.performed)
        {
            foreach (var interactiveObject in _currentNearObjects)
            {
                interactiveObject.Interactuar(Reporter, IsHit);
                OnSuccesFireEvent?.Invoke();
            }
            OnTryFireEvent?.Invoke();
        }
        if (context.canceled){OnCanceledFireEvent?.Invoke();}
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
                _currentNearObjectsReadOnly.Add(other.gameObject);
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
                _currentNearObjectsReadOnly.Remove(other.gameObject);
            }
        }
    }
}
