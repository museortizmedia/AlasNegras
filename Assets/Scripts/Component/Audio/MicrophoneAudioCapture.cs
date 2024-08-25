using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneAudioCapture : MonoBehaviour
{
    public bool PlayOnAwake;
    [Header("Microphone Settings")]
    public string microphoneDeviceName = "";
    public int sampleRate = 44100;
    public float clipDuration = 1f;

    [Header("Sensitivity Settings")]
    public float sensitivity;

    public UnityEvent<AudioSource> OnStartCapture;
    public UnityEvent<float> OnVolumeChanged;

    [Header("Read Only")]
    public string[] microphoneDevices;
    [SerializeField] float scaledVolume;
    [SerializeField] float averageVolume;

    private AudioSource audioSource;
    private AudioClip microphoneClip;
    private float[] audioSamples;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InitializeMicrophoneDevices();
        if(PlayOnAwake){StartMicrophoneCapture();}
    }

    private void InitializeMicrophoneDevices()
    {
        microphoneDevices = Microphone.devices;
    }

    [ContextMenu("Start Capture")]
    private void StartMicrophoneCapture()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
        }

        if (string.IsNullOrEmpty(microphoneDeviceName))
        {
            microphoneDeviceName = Microphone.devices[0];
        }

        microphoneClip = Microphone.Start(microphoneDeviceName, true, Mathf.RoundToInt(clipDuration), sampleRate);

        audioSource.clip = microphoneClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();

        OnStartCapture?.Invoke(audioSource);

        int bufferSize = 2048;
        audioSamples = new float[bufferSize];
    }

    private void Update()
    {
        if (audioSource.isPlaying)
        {
            audioSource.GetOutputData(audioSamples, 0);

            if (audioSamples.Length > 0)
            {
                averageVolume = GetAverageVolume(audioSamples);
                scaledVolume = Mathf.Clamp(averageVolume * sensitivity, 0, 1);

                OnVolumeChanged?.Invoke(scaledVolume); // Emitir el volumen
            }
        }
    }

    private float GetAverageVolume(float[] samples)
    {
        float sum = 0f;
        foreach (float sample in samples)
        {
            sum += Mathf.Abs(sample);
        }
        return sum / samples.Length;
    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }
}
