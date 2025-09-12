using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class VoiceInteractionController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private AudioSource voicePlayer;

    [Header("Animation Voice Clips")]
    public AudioClip idleVoice;
    public AudioClip runVoice;
    public AudioClip attackVoice;
    public AudioClip jumpVoice;
    public AudioClip hurtVoice;
    

    [Header("UI Voice Clips")]
    public AudioClip voiceOnHover;
    public AudioClip voiceOnClick;

    private void Awake()
    {
        voicePlayer = GetComponent<AudioSource>();
        voicePlayer.playOnAwake = false;
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        voicePlayer.volume = SettingManager.GetSoundVolume();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("VoiceInteractionController: OnPointerEnter called");
        UpdateVolume();
        if (voiceOnHover != null)
        {
            voicePlayer.PlayOneShot(voiceOnHover);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("VoiceInteractionController: OnPointerClick called");
        UpdateVolume();
        if (voiceOnClick != null)
        {
            voicePlayer.PlayOneShot(voiceOnClick);
        }
    }

    public void PlayVoice(string animationName)
    {
        UpdateVolume();

        AudioClip clipToPlay = null;
        bool shouldLoop = false;

        switch (animationName)
        {
            case "Idle":
                clipToPlay = idleVoice;
                shouldLoop = true;
                break;
            case "Run":
                clipToPlay = runVoice;
                shouldLoop = true;
                break;
            case "Attack":
                clipToPlay = attackVoice;
                break;
            case "Jump":
                clipToPlay = jumpVoice;
                break;
            case "Hurt":
                clipToPlay = hurtVoice;
                break;
        }

        if (clipToPlay != null)
        {
            if (shouldLoop)
            {
                if (voicePlayer.clip != clipToPlay)
                {
                    voicePlayer.clip = clipToPlay;
                    voicePlayer.loop = true;
                    voicePlayer.Play();
                }
            }
            else
            {
                if (voicePlayer.loop && voicePlayer.isPlaying)
                {
                    voicePlayer.Stop();
                    voicePlayer.loop = false;
                    voicePlayer.clip = null;
                }
                voicePlayer.PlayOneShot(clipToPlay);
            }
        }
    }
}
