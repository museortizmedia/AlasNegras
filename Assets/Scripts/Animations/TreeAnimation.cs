using UnityEngine;

public class WindAnimation : MonoBehaviour
{
    [SerializeField] private float swayAmount = 1f; // Amplitud del movimiento
    [SerializeField] private float swaySpeed = 1f;  // Velocidad de oscilación
    [SerializeField] private float swayOffset = 0f; // Desfase para variar el inicio de la oscilación

    private void Start()
    {
        // Añadimos un pequeño desfase aleatorio para que el movimiento sea más natural
        swayOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    private void Update()
    {
        // Calculamos la oscilación usando la función seno para un movimiento suave
        float sway = Mathf.Sin(Time.time * swaySpeed + swayOffset) * swayAmount;
        
        // Aplicamos la rotación al eje Y (o X/Z dependiendo de cómo esté orientado tu árbol)
        transform.rotation = Quaternion.Euler(0f, 0f, sway);
    }
}
