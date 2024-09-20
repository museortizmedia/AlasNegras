using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NavMesh_FollowTarget : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject target;
    public bool seguir;
    public UnityEvent OnStartFollow, OnStopFollow;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (seguir && target != null)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
        seguir = true;
        OnStartFollow?.Invoke();
    }
    public void RemoveTarget()
    {
        target = null;
        seguir = false;
        OnStopFollow?.Invoke();
    }
}
