using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorBoolSetter : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Método para establecer un parámetro booleano en true
    public void SetTrue(string parameterName)
    {
        if (animator != null)
        {
            animator.SetBool(parameterName, true);
            Debug.Log($"{parameterName} en true");
        }
        else
        {
            Debug.LogWarning("Animator no encontrado.");
        }
    }

    // Método para establecer un parámetro booleano en false
    public void SetFalse(string parameterName)
    {
        if (animator != null)
        {
            animator.SetBool(parameterName, false);
            Debug.Log($"{parameterName} en false");
        }
        else
        {
            Debug.LogWarning("Animator no encontrado.");
        }
    }
}
