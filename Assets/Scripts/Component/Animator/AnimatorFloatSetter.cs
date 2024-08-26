using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorFloatSetter : MonoBehaviour
{
    private Animator animator;

    [Tooltip("Nombre del parámetro float en el Animator")]
    [SerializeField]
    private string parameterName;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Método para establecer un parámetro float en el Animator
    public void SetFloat(float value)
    {
        if (animator != null)
        {
            animator.SetFloat(parameterName, value);
        }
        else
        {
            Debug.LogWarning("Animator no encontrado.");
        }
    }
}
