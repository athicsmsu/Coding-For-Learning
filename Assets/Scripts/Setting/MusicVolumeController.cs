using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicVolumeController : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        audioSource.volume = SettingManager.GetMusicVolume();
    }
}
