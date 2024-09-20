//#define DEBUG
using UnityEngine;
using UnityEngine.Events;
public interface IInteractiveObject
{
    public void Interactuar(GameObject from, bool IsHit);
    public void OnEnterPlayer();
    public void OnExitPlayer();
}
public class InteractiveObject : MonoBehaviour, IInteractiveObject
{
    public UnityEvent<GameObject> OnInteractive;
    public UnityEvent OnPlayerArrive;
    public UnityEvent OnPlayerLeave;

    public void Interactuar(GameObject from, bool IsHit)
    {
        #if DEBUG
        Debug.Log($"{from.name} interactuó conmigo!", transform);
        #endif
        if(gameObject.activeSelf)
        {
            OnInteractive?.Invoke(from);
        }
    }

    public void OnEnterPlayer()
    {
        #if DEBUG
        Debug.Log($"Aqui estoy", transform);
        #endif
        OnPlayerArrive?.Invoke();
    }

    public void OnExitPlayer()
    {
        #if DEBUG
        Debug.Log($"Me voy", transform);
        #endif
        OnPlayerLeave?.Invoke();
    }
}
