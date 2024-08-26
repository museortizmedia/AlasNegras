using UnityEngine;
using UnityEngine.Events;

public class InteractiveEnemie : MonoBehaviour, IInteractiveObject
{
    
    [Header("Damagable Zone")]
    public UnityEvent OnReciveDamage;
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;

    public void Interactuar(GameObject from, bool IsHit)
    {
        // Se realiza daño al enemigo
        if (IsHit)
        {
            // Cálculo de daño: Daño - Defensa
            float damage = from.GetComponent<StatController>().GetStat<DamageStat>().CurrentValue;
            float defense = from.GetComponent<StatController>().GetStat<DefenseStat>().CurrentValue;

            gameObject.GetComponent<StatController>().GetStat<HealthStat>()?.MakeDamage(damage - defense);
            OnReciveDamage?.Invoke();
        }
        
        // No realiza daño
        if(!IsHit)
        {
            // 
        }
    }

    public void OnEnterPlayer()
    {
        OnPlayerEnter?.Invoke();
    }

    public void OnExitPlayer()
    {
        OnPlayerExit?.Invoke();
    }
}