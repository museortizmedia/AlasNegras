using UnityEngine;
using UnityEngine.Events;

public class InteractiveEnemie : MonoBehaviour, IInteractiveObject
{
    public UnityEvent<GameObject> OnInteractive;
    public UnityEvent OnPlayerArrive;
    public UnityEvent OnPlayerLeave;

    public void Interactuar(GameObject from)
    {
        Debug.Log("Me están golpeando", transform);
        gameObject.GetComponent<StatController>().GetStat<HealthStat>()?.MakeDamage(from.GetComponent<StatController>().GetStat<DamageStat>().CurrentValue);
    }

    public void OnEnterPlayer()
    {
        OnPlayerArrive?.Invoke();
    }

    public void OnExitPlayer()
    {
        OnPlayerLeave?.Invoke();
    }
}