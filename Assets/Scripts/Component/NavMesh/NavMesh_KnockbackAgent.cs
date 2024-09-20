using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh_KnockbackAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody rb;

    // Configuración del retroceso
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    // Método para aplicar el retroceso
    public void ApplyKnockback(Vector3 hitDirection)
    {
        StartCoroutine(KnockbackRoutine(hitDirection));
    }
    public void ApplyKnockback()
    {
        StartCoroutine(KnockbackRoutine(-transform.forward));
    }

    private IEnumerator KnockbackRoutine(Vector3 hitDirection)
    {
        // Deshabilitar temporalmente el NavMeshAgent
        agent.enabled = false;

        // Aplicar la fuerza de retroceso
        rb.AddForce(-hitDirection.normalized * knockbackForce, ForceMode.Impulse);

        // Esperar un momento antes de reactivar el agente
        yield return new WaitForSeconds(knockbackDuration);

        // Detener el movimiento del Rigidbody y reactivar el NavMeshAgent
        rb.velocity = Vector3.zero;
        agent.enabled = true;
    }
}

