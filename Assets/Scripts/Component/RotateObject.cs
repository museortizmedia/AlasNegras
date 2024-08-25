using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Velocidad de rotación en grados por segundo
    public Vector3 rotationSpeed = new Vector3(30, 60, 90);

    void Update()
    {
        // Aplicar la rotación en cada frame
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
