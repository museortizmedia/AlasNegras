using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorIntSetter : MonoBehaviour
{
    private Animator animator;

    [Tooltip("Nombre del parámetro int en el Animator")]
    [SerializeField]
    private string parameterName;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetInt(int value)
    {
        if (animator != null)
        {
            animator.SetInteger(parameterName, value);
        }
        else
        {
            Debug.LogWarning("Animator no encontrado.");
        }
    }
}