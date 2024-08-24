using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    [Header("Player Settings")]
    public List<GameObject> playerPrefabs; // Lista de prefabs de los jugadores
    public Transform spawnPoint; // Punto de spawn para los jugadores

    [Header("Camera Settings")]
    public CinemachineVirtualCamera virtualCamera; // Referencia a la cámara virtual de Cinemachine
    public Transform cameraTarget; // Referencia al objeto que la cámara seguirá

    private List<GameObject> players = new List<GameObject>();

    void Start()
    {
        // Intancia el primer player
        InstanciarPlayer();


        // Asignar el target de la cámara
        virtualCamera.Follow = cameraTarget;
        virtualCamera.LookAt = cameraTarget;
    }

    void Update()
    {
        if (players.Count == 0) return;

        Vector3 averagePosition = Vector3.zero;

        // Calcular la posición media de todos los jugadores
        foreach (GameObject player in players)
        {
            averagePosition += player.transform.position;
        }

        averagePosition /= players.Count;

        // Mover el CameraTarget a la posición media
        cameraTarget.position = averagePosition;
    }
    [ContextMenu("Intanciar Jugador")]
    public void InstanciarPlayer()
    {
        if(players.Count < playerPrefabs.Count){
            GameObject player = Instantiate(playerPrefabs[players.Count], spawnPoint.position, Quaternion.identity);
            players.Add(player);
        }
    }
    // Este método es llamado automáticamente por PlayerInputManager cuando se conecta un nuevo controlador
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int playerIndex = players.Count;

        // Verificar que no excedamos la cantidad de prefabs disponibles
        if (playerIndex < playerPrefabs.Count)
        {
            // Instanciar el jugador en el spawnPoint
            GameObject player = Instantiate(playerPrefabs[playerIndex], spawnPoint.position, Quaternion.identity);

            // Asignar el PlayerInput al jugador instanciado
            playerInput.transform.SetParent(player.transform);
            players.Add(player);
        }
    }

    // Este método es llamado automáticamente por PlayerInputManager cuando se desconecta un controlador
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        // Encontrar el jugador que corresponde al PlayerInput
        GameObject playerToRemove = players.Find(player => player.GetComponentInChildren<PlayerInput>() == playerInput);

        if (playerToRemove != null)
        {
            // Desactivar o eliminar el jugador
            players.Remove(playerToRemove);
            playerToRemove.SetActive(false); // También puedes usar Destroy(playerToRemove) si prefieres eliminarlo
        }
    }
}
