using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(StatController))]
[RequireComponent(typeof(NearTrigger))]
[RequireComponent(typeof(NavMesh_FollowTarget))]
[RequireComponent(typeof(NavMesh_KnockbackAgent))]
[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour, IInteractiveObject
{
    [Header("Components")]
    public StatController enemyStatController;
    public NearTrigger enemyNearTrigger;
    public NavMesh_FollowTarget enemyFollowTarget;
    public NavMesh_KnockbackAgent enemyKnockback;
    public EnemyAttack enemyAttack;
    public NavMeshAgent navMeshAgent;


    [Header("Damagable Zone")]
    public UnityEvent OnReciveDamage;
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;

    private void Awake()
    {
        enemyStatController = GetComponent<StatController>();
        enemyNearTrigger = GetComponent<NearTrigger>();
        enemyFollowTarget = GetComponent<NavMesh_FollowTarget>();
        enemyKnockback = GetComponent<NavMesh_KnockbackAgent>();
        enemyAttack = GetComponent<EnemyAttack>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        // NearTrigger se comunica con Nav Follow para "ver" y "perder de vista" al jugador

        // EnemyAttack está esperando que Folow tenga un objetivo para medir la distancia y atacar al Jugador
    }

    #region ATAQUE DEL ENEMIGO
    #endregion

    #region ATAQUE DEL JUGADOR 
    // SI EL JUGADOR HACE DAÑO AL ENEMIGO
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
            // Inhibir el ataque del enemigo
        }
    }

    // Cuando el player detecta al enemigo
    public void OnEnterPlayer()
    {
        OnPlayerEnter?.Invoke();
    }

    // Cuando el player deja de detectar al enemigo
    public void OnExitPlayer()
    {
        OnPlayerExit?.Invoke();
    }
    #endregion
}