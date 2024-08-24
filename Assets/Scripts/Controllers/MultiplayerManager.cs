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
    private Dictionary<InputDevice, GameObject> deviceToPlayerMap = new Dictionary<InputDevice, GameObject>();

    private void OnEnable()
    {
        // Registrar para recibir eventos cuando se conecte o desconecte un dispositivo
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        // Desregistrar eventos
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void Start()
    {
        // Instanciar el primer jugador (por defecto usa teclado y ratón)
        InstanciarPlayer(null);

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
    [ContextMenu("Instanciar Player")]
    public void InstanciarPlayer()
    {
        InstanciarPlayer(null);
    }
    public void InstanciarPlayer(InputDevice device)
    {
        if (players.Count < playerPrefabs.Count)
        {
            GameObject player = Instantiate(playerPrefabs[players.Count], spawnPoint.position, Quaternion.identity);
            players.Add(player);

            // Intentar obtener el componente PlayerInput
            if (player.TryGetComponent(out PlayerInput playerInput))
            {
                if (device != null)
                {
                    // Asignar el dispositivo al jugador
                    playerInput.SwitchCurrentControlScheme(device);
                    deviceToPlayerMap[device] = player;
                }
                else
                {
                    // El prefab tiene configurado por defecto la opcion para teclado
                }

                // Habilitar acciones
                playerInput.actions.Enable();
            }
        }

    }

    // Manejar eventos de cambio de dispositivo
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                if (device is Gamepad || device is Keyboard || device is Mouse)
                {
                    // Instanciar un nuevo jugador si hay un dispositivo compatible conectado
                    if (players.Count < playerPrefabs.Count)
                    {
                        InstanciarPlayer(device);
                    }
                }
                break;

            case InputDeviceChange.Removed:
                if (device is Gamepad || device is Keyboard || device is Mouse)
                {
                    RemovePlayer(device);
                }
                break;
        }
    }

    private void RemovePlayer(InputDevice device)
    {
        if (deviceToPlayerMap.TryGetValue(device, out GameObject playerToRemove))
        {
            // Desactivar el jugador y eliminar la referencia
            players.Remove(playerToRemove);
            deviceToPlayerMap.Remove(device);
            Destroy(playerToRemove);
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int playerIndex = players.Count;

        // Verificar que no excedamos la cantidad de prefabs disponibles
        if (playerIndex < playerPrefabs.Count)
        {
            // Instanciar el jugador en el spawnPoint
            GameObject player = Instantiate(playerPrefabs[playerIndex], spawnPoint.position, Quaternion.identity);

            // Asignar el PlayerInput al jugador instanciado
            var playerInputComponent = player.GetComponent<PlayerInput>();
            if (playerInputComponent == null)
            {
                playerInputComponent = player.AddComponent<PlayerInput>();
            }

            // Configurar el esquema de control basado en el dispositivo conectado
            if (playerInput.devices.Count > 0)
            {
                var device = playerInput.devices[0];
                if (device is Keyboard || device is Mouse)
                {
                    playerInputComponent.SwitchCurrentControlScheme("Keyboard&Mouse", null);
                }
                else if (device is Gamepad)
                {
                    playerInputComponent.SwitchCurrentControlScheme("Gamepad", null);
                }
            }

            // Habilitar acciones
            playerInputComponent.actions.Enable();

            // Asignar el PlayerInput a la lista
            players.Add(player);
        }
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        // Encontrar el jugador que corresponde al PlayerInput
        GameObject playerToRemove = players.Find(player => player.GetComponent<PlayerInput>() == playerInput);

        if (playerToRemove != null)
        {
            // Destruir el jugador
            players.Remove(playerToRemove);
            Destroy(playerToRemove);
        }
    }
}
