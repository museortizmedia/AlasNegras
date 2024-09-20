using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyController))]
public class EnemyAttack : MonoBehaviour
{
    public float attackCooldown = 2f;  // Tiempo entre ataques
    public float proximityRange = 1f;  // Rango de proximidad para activar el ataque
    private float nextAttackTime = 0f;  // Tiempo hasta el próximo ataque

    public UnityEvent OnAttack;

    [SerializeField] EnemyController enemyController;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] NavMesh_FollowTarget followTarget;

    private bool hasReachedProximity = false;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        agent = enemyController.navMeshAgent;
        followTarget = enemyController.enemyFollowTarget;
    }

    void Update()
    {
        if (followTarget!=null && followTarget.seguir)
        {
            // Verificar si el agente ha alcanzado su destino y se ha detenido
            if (!agent.pathPending && HasReachedDestination())
            {
                // Solo permitir el ataque si ya ha alcanzado la proximidad
                if (hasReachedProximity)
                {
                    if (Time.time >= nextAttackTime)
                    {
                        AttackPlayer();
                        nextAttackTime = Time.time + attackCooldown;
                    }
                }
                else
                {
                    hasReachedProximity = true;  // Marca que ha alcanzado la proximidad una vez
                }
            }
            else
            {
                hasReachedProximity = false;  // Resetea si se mueve de nuevo
            }
        }
    }

    bool HasReachedDestination()
    {
        // Comprobar si el agente está lo suficientemente cerca del destino
        if (agent.remainingDistance <= agent.stoppingDistance + proximityRange)
        {
            // Verificar si la velocidad del agente es cercana a cero
            if (agent.velocity.sqrMagnitude < 0.01f)
            {
                return true;
            }
        }
        return false;
    }

    void AttackPlayer()
    {
        GameObject player = followTarget.target;
        if(player.TryGetComponent<StatController>(out StatController playerStatController))
        {
            // Hacer daño = daño enemigo - defensa player
            float EnemyDamage = GetComponent<StatController>().GetStat<DamageStat>().CurrentValue;
            float PlayerDefense = playerStatController.GetStat<DefenseStat>().CurrentValue;

            playerStatController.GetStat<HealthStat>().MakeDamage( EnemyDamage - PlayerDefense );
        }
        OnAttack?.Invoke();
    }
}
