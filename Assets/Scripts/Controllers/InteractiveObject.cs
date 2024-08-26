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
        //Debug.Log($"{from.name} interactuó conmigo!", transform);
        OnInteractive?.Invoke(from);
    }

    public void OnEnterPlayer()
    {
        //Debug.Log($"Aqui estoy", transform);
        OnPlayerArrive?.Invoke();
    }

    public void OnExitPlayer()
    {
        //Debug.Log($"Me voy", transform);
        OnPlayerLeave?.Invoke();
    }
}
