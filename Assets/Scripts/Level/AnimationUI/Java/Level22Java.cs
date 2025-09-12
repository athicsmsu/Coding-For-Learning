using UnityEngine;
using UnityEngine.UI;

public class Level22Java : MonoBehaviour
{
    public GameObject hobbitPrefab;
    public void Correct(string answer, Text askText, Text askText2, PlayerController player)
{
    if (askText != null)
        askText.text = answer;
    if (askText2 != null)
        askText2.text = "Hi";

    TriggerAnimation(player, "Win");

    Invoke(nameof(HideHobbit), 5f);
}

private void HideHobbit()
{
    hobbitPrefab.SetActive(false);
}

    public void Wrong(Text askText, Text askText2, PlayerController player)
    {
        Debug.Log("Wrong Java W2");
        if (askText != null)
            askText.text = "Lose!";
        if (askText2 != null)
            askText2.text = "Lose!";
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
