using UnityEngine;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour
{
    public Button button;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    private int currentClipIndex = 0;

    void Start()
    {
        // Attach the button click listener
        button.onClick.AddListener(PlayCurrentAudio);
    }

    void PlayCurrentAudio()
    {
        // Check if the audio source and clip array are assigned
        if (audioSource != null && audioClips != null && audioClips.Length > 0)
        {
            // Stop the audio playback if it's already playing
            audioSource.Stop();

            // Set the current audio clip and play it
            audioSource.clip = audioClips[currentClipIndex];
            audioSource.Play();
        }
        else
        {
            Debug.Log("AudioSource or AudioClip array not assigned!");
        }
    }

    // Method to manually switch to the next audio clip
    public void SwitchToNextAudio()
    {
        // Increment the index for the next audio clip
        currentClipIndex = (currentClipIndex + 1) % audioClips.Length;

        // Play the newly selected audio clip
        PlayCurrentAudio();
    }
}