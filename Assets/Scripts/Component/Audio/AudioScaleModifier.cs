using UnityEngine;

public class AudioScaleModifier : MonoBehaviour
{
    [Header("Scale Settings")]
    public Vector3 minScale = new Vector3(1f, 1f, 1f);
    public Vector3 maxScale = new Vector3(2f, 2f, 2f);

    [Header("Power Vector")]
    public Vector3 scalePower = new Vector3(1f, 1f, 1f);

    [Header("Sensitivity Settings")]
    public float sensitivity = 1f;
    public float maxNormalizedAmplitude = 1f;

    private Vector3 initialScale;
    private bool isScaling = false;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    public void StartScaling()
    {
        isScaling = true;
    }

    public void StopScaling()
    {
        isScaling = false;
        transform.localScale = initialScale;
    }

    public void UpdateScaleFromVolume(float volume)
    {
        if (!isScaling)
        {
            return;
        }

        // Normalizar el volumen a un rango entre 0 y 1 y aplicar la sensibilidad
        float normalizedAmplitude = Mathf.Clamp01(volume * sensitivity / maxNormalizedAmplitude);

        // Interpolación de la escala entre el mínimo y el máximo basándose en la amplitud normalizada
        Vector3 targetScale = Vector3.Lerp(minScale, maxScale, normalizedAmplitude);

        // Aplicar el vector de potencia a la escala
        Vector3 scaledScale = Vector3.Scale(targetScale, scalePower);

        // Aplicar la escala al GameObject
        transform.localScale = initialScale + scaledScale;
    }

}
