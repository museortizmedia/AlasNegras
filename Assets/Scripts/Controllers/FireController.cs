using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FireController : MonoBehaviour
{
    public string mensaje = "FIRE";
    public UnityEvent OnFire1Event;
    [Header("Statics references")]
    [SerializeField] InteractiveObject _currentNearObject;


    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(mensaje);
            _currentNearObject?.Interacturar(gameObject);
            OnFire1Event?.Invoke();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out InteractiveObject interactiveObject))
        {
            _currentNearObject = interactiveObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _currentNearObject)
        {
            _currentNearObject = null;
        }
    }
}
