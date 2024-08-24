using UnityEngine;
using UnityEngine.Events;
public interface IInteractiveObject
{
    public void Interacturar(GameObject from);
}
public class InteractiveObject : MonoBehaviour, IInteractiveObject
{
    public UnityEvent<GameObject> OnInteractive;

    public void Interacturar(GameObject from)
    {
        Debug.Log($"{from.name} interactuó conmigo!", transform);
    }
}
