using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Level26Java : MonoBehaviour
{
    public GameObject fireArrowPrefab;   // เดิม lightningPrefab
    public GameObject ironArrowPrefab;   // เดิม fireballPrefab

    void Start()
    {
        if (fireArrowPrefab != null) fireArrowPrefab.SetActive(false);
        if (ironArrowPrefab != null) ironArrowPrefab.SetActive(false);
    }

    public void Correct(string answer, Text askText, PlayerController player)
    {
        Debug.Log("✅ Correct Java C26");

        if (askText != null)
            askText.text = answer;

        if (player != null)
            TriggerAnimation(player, "Win");

        StartCoroutine(ShowFireArrowForSeconds(5f));
    }

    private IEnumerator ShowFireArrowForSeconds(float seconds)
{
    fireArrowPrefab.SetActive(true);
    yield return new WaitForSeconds(seconds);
    fireArrowPrefab.SetActive(false);
}

    public void Wrong(Text askText, PlayerController player)
    {
        Debug.Log("❌ Wrong Java C26");

        if (askText != null)
            askText.text = "Lose!";

        if (player != null)
            TriggerAnimation(player, "Lose");

        StartCoroutine(ShowIronArrowThenIdle(player));
    }

    private IEnumerator ShowIronArrowThenIdle(PlayerController player)
    {
        if (ironArrowPrefab != null)
            ironArrowPrefab.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        if (player != null)
            TriggerAnimation(player, "Idle");

        if (ironArrowPrefab != null)
            ironArrowPrefab.SetActive(false);
    }

    private void TriggerAnimation(PlayerController player, string trigger)
    {
        if (player != null && player.CurrentCharacter != null)
        {
            Animator animator = player.CurrentCharacter.GetComponent<Animator>();
            if (animator != null)
            {
                animator.ResetTrigger("Win");
                animator.ResetTrigger("Lose");
                animator.ResetTrigger("Idle");
                animator.SetTrigger(trigger);
            }
        }
    }
}
