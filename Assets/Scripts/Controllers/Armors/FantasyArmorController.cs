//#define DEBUG_ARMOR

using UnityEngine;

public class FantasyArmorController : MonoBehaviour
{
    public ArmorMeshData armorData;
    public bool hasClothes;
    [SerializeField] private SkinnedMeshRenderer[] allMeshRenderers;
    [SerializeField] private SkinnedMeshRenderer[] activeMeshRenderers;

    private void Start()
    {
        allMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);
        
        #if DEBUG_ARMOR
        Debug.Log($"Found {allMeshRenderers.Length} SkinnedMeshRenderers in the character.");
        #endif
    }

    public void EquipEspecificArmor(ArmorMeshData specificArmor){
        armorData = specificArmor;
        EquipArmor();
    }

    [ContextMenu("Equipar")]
    public void EquipArmor()
    {
        // Almacena los SkinnedMeshRenderers que estaban activos antes de equipar la armadura
        activeMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        
        #if DEBUG_ARMOR
        Debug.Log($"Found {activeMeshRenderers.Length} active SkinnedMeshRenderers before equipping armor.");
        #endif

        // Oculta todos los GameObjects de los SkinnedMeshRenderers activos
        foreach (var renderer in activeMeshRenderers)
        {
            #if DEBUG_ARMOR
            Debug.Log($"Disabling SkinnedMeshRenderer on: {renderer.gameObject.name}");
            #endif
            renderer.gameObject.SetActive(hasClothes);
        }

        // Activa los GameObjects que contienen los Meshes de la armadura
        foreach (var mesh in armorData.ArmosAssets)
        {
            bool meshFound = false;

            foreach (var renderer in allMeshRenderers)
            {
                var meshFilter = renderer.sharedMesh;
                if (meshFilter != null)
                {
                    // Compara los Meshes para activar el correspondiente
                    if (meshFilter == mesh)
                    {
                        #if DEBUG_ARMOR
                        Debug.Log($"Enabling GameObject for armor on: {renderer.gameObject.name}");
                        #endif
                        renderer.gameObject.SetActive(true);  // Activar el GameObject que contiene el SkinnedMeshRenderer
                        meshFound = true;
                    }
                }
            }

            if (!meshFound)
            {
                #if DEBUG_ARMOR
                Debug.LogWarning($"Mesh {mesh.name} from ArmorData not found in character's SkinnedMeshRenderers.");
                #endif
            }
        }
    }

    [ContextMenu("Quitar")]
    public void UnequipArmor()
    {
        // Desactiva los GameObjects que contienen los SkinnedMeshRenderers de la armadura
        foreach (var mesh in armorData.ArmosAssets)
        {
            foreach (var renderer in allMeshRenderers)
            {
                var meshFilter = renderer.sharedMesh;
                if (meshFilter != null)
                {
                    // Compara los Meshes para desactivar el correspondiente
                    if (meshFilter == mesh)
                    {
                        #if DEBUG_ARMOR
                        Debug.Log($"Disabling GameObject for armor on: {renderer.gameObject.name}");
                        #endif
                        renderer.gameObject.SetActive(false);  // Desactivar el GameObject que contiene el SkinnedMeshRenderer
                    }
                }
            }
        }

        // Reactiva los GameObjects que contenían los SkinnedMeshRenderers activos antes de equipar la armadura
        foreach (var renderer in activeMeshRenderers)
        {
            #if DEBUG_ARMOR
            Debug.Log($"Re-enabling original GameObject on: {renderer.gameObject.name}");
            #endif
            renderer.gameObject.SetActive(true);  // Reactivar el GameObject que contiene el SkinnedMeshRenderer
        }
    }
}
