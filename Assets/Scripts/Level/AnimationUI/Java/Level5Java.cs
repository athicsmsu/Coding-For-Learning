using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Level5Java : MonoBehaviour
{
    public GameObject bgPrefab;
    public GameObject askTxt;
    public GameObject askQuiz;
    public void Correct(string answer, Text askText, PlayerController player)
    {
        Debug.Log("Correct Java C5");
        if (askText != null)
            askText.text = answer;

        TriggerAnimation(player, "Win");
        StartCoroutine(ShowSeconds(5f));
    }

    private IEnumerator ShowSeconds(float seconds)
    {
        bgPrefab.SetActive(true);
        askTxt.SetActive(true);
        askQuiz.SetActive(true);
        yield return new WaitForSeconds(seconds);
        bgPrefab.SetActive(false);
        askTxt.SetActive(false);
        askQuiz.SetActive(false);
    }

    public void Wrong(Text askText, PlayerController player)
    {
        Debug.Log("Wrong Java W5");
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
