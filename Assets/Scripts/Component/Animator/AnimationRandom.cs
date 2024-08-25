using UnityEngine;
using UnityEditor.Animations;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class AnimationRandom : MonoBehaviour
{
    [Tooltip("Nombre del BlendTree")]
    public string blendTreeName = "RandomAttack";
    [Tooltip("Nombre del trigger del animator que activará el BlendTree")]
    public string attackTrigger = "Slash";
    [Tooltip("Nombre del float del animator que es parámetro del BlendTree")]
    public string attackIndexParameter = "SlashIndex";

    [Header("Control de Animación")]
    [Tooltip("Determina si el script debe esperar a que termine la animación antes de activar otra.")]
    public bool waitForAnimationToEnd = true;

    private Animator anim;
    private List<AnimationClip> animationClips = new List<AnimationClip>();
    private List<float> animationDurations = new List<float>();
    private bool isAnimationPlaying = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        // Obtener las animaciones y sus duraciones en el Blend Tree
        GetAnimationsAndDurations(anim, blendTreeName);
    }

    private void GetAnimationsAndDurations(Animator animator, string blendTreeName)
    {
        AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;

        if (controller == null)
        {
            Debug.LogError("No se encontró un AnimatorController.");
            return;
        }

        foreach (var layer in controller.layers)
        {
            foreach (var state in layer.stateMachine.states)
            {
                if (state.state.motion is BlendTree blendTree && state.state.name == blendTreeName)
                {
                    foreach (var child in blendTree.children)
                    {
                        if (child.motion is AnimationClip clip)
                        {
                            animationClips.Add(clip);
                            animationDurations.Add(clip.length);
                        }
                    }
                    return;
                }
            }
        }

        Debug.LogError($"No se encontró un Blend Tree con el nombre {blendTreeName}.");
    }

    public void TriggerRandomAnimation()
    {
        if (animationClips.Count == 0)
        {
            Debug.LogWarning("No se encontraron animaciones en el Blend Tree.");
            return;
        }

        // Verificar si la animación actual está en curso y si debe esperar
        if (isAnimationPlaying && waitForAnimationToEnd)
        {
            Debug.Log("La animación anterior aún está en curso.");
            return;
        }

        // Seleccionar aleatoriamente un índice de ataque
        int randomIndex = Random.Range(0, animationClips.Count);
        Debug.Log(randomIndex);

        // Configurar el parámetro de índice en el Animator
        anim.SetFloat(attackIndexParameter, randomIndex + 1);

        // Activar el trigger de ataque
        anim.SetTrigger(attackTrigger);

        // Obtener la duración de la animación seleccionada
        float animationDuration = animationDurations[randomIndex];

        if (waitForAnimationToEnd)
        {
            isAnimationPlaying = true;
            Invoke(nameof(ResetAnimationFlag), animationDuration);
        }
    }

    private void ResetAnimationFlag()
    {
        isAnimationPlaying = false;
    }
}
