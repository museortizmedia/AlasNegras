using UnityEngine;
using UnityEngine.Events;
public interface IInteractiveObject
{
    public void Interactuar(GameObject from);
    public void OnEnterPlayer();
    public void OnExitPlayer();
}
public class InteractiveObject : MonoBehaviour, IInteractiveObject
{
    public UnityEvent<GameObject> OnInteractive;
    public UnityEvent OnPlayerArrive;
    public UnityEvent OnPlayerLeave;

    public void Interactuar(GameObject from)
    {
        Debug.Log($"{from.name} interactuó conmigo!", transform);
    }

    public void OnEnterPlayer()
    {
        Debug.Log($"Aqui estoy", transform);
        OnPlayerArrive?.Invoke();
    }

    public void OnExitPlayer()
    {
        Debug.Log($"Me voy", transform);
        OnPlayerLeave?.Invoke();
    }
}
