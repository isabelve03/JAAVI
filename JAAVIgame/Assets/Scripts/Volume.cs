using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;  // The AudioSource component to control the volume
    public Slider volumeSlider;      // The UI Slider for controlling the volume

    void Start()
    {
        // Ensure the slider's value is set to the current volume of the AudioSource when the game starts
        if (volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
        }
    }

    // This method will be called when the slider value changes
    void UpdateVolume(float value)
    {
        audioSource.volume = value;
    }
}
