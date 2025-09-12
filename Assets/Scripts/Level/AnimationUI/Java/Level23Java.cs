using UnityEngine;
using UnityEngine.UI;

public class Level23Java : MonoBehaviour
{
    public void Correct(string answer, Text askText, Text askText2, PlayerController player, PlayerController player2)
    {
        Debug.Log("✅ Correct Java C23");

        if (askText != null)
            askText.text = "Old Name\nKing Smite";

        if (askText2 != null)
            askText2.text = "New Name\nMonkey King";

        TriggerAnimation(player, "Win");
        TriggerAnimation(player2, "Win");
    }

    public void Wrong(Text askText, Text askText2, PlayerController player, PlayerController player2)
    {
        Debug.Log("❌ Wrong Java W23");

        if (askText != null)
            askText.text = "Lose";

        if (askText2 != null)
            askText2.text = "Lose";

        TriggerAnimation(player, "Lose");
        TriggerAnimation(player2, "Lose");
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
