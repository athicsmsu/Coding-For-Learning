using UnityEngine;

public class PlayVoiceBehaviour : StateMachineBehaviour
{
    public string voiceName; // เช่น "Run", "Attack" ฯลฯ

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        VoiceInteractionController voiceController = animator.GetComponent<VoiceInteractionController>();
        if (voiceController != null)
        {
            voiceController.PlayVoice(voiceName);
        }
    }
}
