using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
     public float floatHeight = 2f; // Altura a la que flota el canvas
    public float floatSpeed = 1f; // Velocidad de flotación
    public float floatRange = 0.5f; // Rango de flotación

    private Vector3 startPos;
    private float floatTime;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        floatTime += Time.deltaTime * floatSpeed;
        float yOffset = Mathf.Sin(floatTime) * floatRange;
        transform.position = startPos + new Vector3(0, yOffset + floatHeight, 0);
    }
}
