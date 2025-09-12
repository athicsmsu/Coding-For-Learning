using UnityEngine;
using UnityEngine.UI;

public class Level6Python : MonoBehaviour
{
    public void Correct(string answer, Text askText, PlayerController player)
    {
        Debug.Log("Correct Python C6");
        if (askText != null)
            askText.text = answer;

        TriggerAnimation(player, "Win");
    }

    public void Wrong(Text askText, PlayerController player)
    {
        Debug.Log("Wrong Python W6");
        if (askText != null)
            askText.text = "Lose!";

        TriggerAnimation(player, "Lose");
    }

    private void TriggerAnimation(PlayerController player, string trigger)
    {
        if (player != null && player.CurrentCharacter != null)
        {
            Animator animator = player.CurrentCharacter.GetComponent<Animator>();
            if (animator != null && animator.runtimeAnimatorController != null)
            {
                animator.ResetTrigger("Win");
                animator.ResetTrigger("Lose");
                animator.ResetTrigger("Idle");

                animator.SetTrigger(trigger);
            }
        }
    }
}
