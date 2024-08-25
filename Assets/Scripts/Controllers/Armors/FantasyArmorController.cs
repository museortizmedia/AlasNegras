using UnityEngine;
using System.Collections.Generic;

public class FantasyArmorController : MonoBehaviour
{
    public ArmorMeshData armorMeshData; // Referencia al ScriptableObject con la lista de Mesh
    public GameObject allArmorMeshes;

    private List<GameObject> originalActiveMeshes = new List<GameObject>(); // Lista para almacenar los GameObject activos originales
    private ArmorMeshData currentArmorData; // Para almacenar el ArmorMeshData actual

    // Método para activar una armadura usando el ArmorMeshData especificado
    public void ActivateArmor(ArmorMeshData newArmorMeshData)
    {
        if (newArmorMeshData != null)
        {
            // Desactivar la armadura actual y restaurar la ropa original si es necesario
            DeactivateArmor();

            // Guardar la ropa original antes de activar la nueva armadura
            originalActiveMeshes.Clear();
            MeshRenderer[] renderers = allArmorMeshes.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();
                if (meshFilter != null && System.Array.Exists(newArmorMeshData.ArmosAssets, mesh => meshFilter.sharedMesh == mesh))
                {
                    if (renderer.gameObject.activeSelf)
                    {
                        originalActiveMeshes.Add(renderer.gameObject); // Guardar el GameObject activo
                    }
                }
            }

            // Desactivar la ropa original
            foreach (GameObject activeMesh in originalActiveMeshes)
            {
                activeMesh.SetActive(false);
            }

            // Activar los nuevos MeshRenderers para la nueva armadura
            MeshRenderer[] newRenderers = allArmorMeshes.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in newRenderers)
            {
                MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();
                if (meshFilter != null && System.Array.Exists(newArmorMeshData.ArmosAssets, mesh => meshFilter.sharedMesh == mesh))
                {
                    renderer.gameObject.SetActive(true);
                }
            }

            // Actualizar el ArmorMeshData actual
            currentArmorData = newArmorMeshData;
        }
    }

    // Método para desactivar la armadura actual
    public void DeactivateArmor()
    {
        if (currentArmorData != null)
        {
            // Desactivar los MeshRenderers actuales de la armadura
            MeshRenderer[] renderers = allArmorMeshes.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();
                if (meshFilter != null && System.Array.Exists(currentArmorData.ArmosAssets, mesh => meshFilter.sharedMesh == mesh))
                {
                    renderer.gameObject.SetActive(false);
                }
            }

            // Restaurar los GameObject activos originales
            foreach (GameObject originalActiveMesh in originalActiveMeshes)
            {
                originalActiveMesh.SetActive(true);
            }

            // Limpiar la lista de ropa original
            originalActiveMeshes.Clear();
        }
    }
}
