using UnityEngine;
using UnityEngine.Events;
public interface IInteractiveObject
{
    public void Interactuar(GameObject from);
}
public class InteractiveObject : MonoBehaviour, IInteractiveObject
{
    public UnityEvent<GameObject> OnInteractive;

    public void Interactuar(GameObject from)
    {
        Debug.Log($"{from.name} interactuó conmigo!", transform);
    }
}
