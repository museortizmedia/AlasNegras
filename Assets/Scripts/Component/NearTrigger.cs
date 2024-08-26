using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class NearTrigger : MonoBehaviour
{
    public bool Enabled {get; set;}
    public string targetTag = "Player";
    public float triggerRadius = 5f;
    public UnityEvent<GameObject> OnEnterTrigger;
    public UnityEvent<GameObject> OnExitTrigger;
    public List<GameObject> nearObjects = new();

    private SphereCollider sphereCollider;

    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = triggerRadius;
        Enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (Enabled && other.CompareTag(targetTag))
        {
            nearObjects.Add(other.gameObject);
            OnEnterTrigger?.Invoke(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (Enabled && other.CompareTag(targetTag))
        {
            nearObjects.Remove(other.gameObject);
            OnExitTrigger?.Invoke(other.gameObject);
        }
    }
}
