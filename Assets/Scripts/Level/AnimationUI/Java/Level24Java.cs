using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Level24Java : MonoBehaviour
{
    public GameObject lightningPrefab;
    public GameObject fireballPrefab;

    void Start()
    {
        // ซ่อนไว้ตอนเริ่มเกม
        if (lightningPrefab != null) lightningPrefab.SetActive(false);
        if (fireballPrefab != null) fireballPrefab.SetActive(false);
    }

    public void Correct(string answer, Text askText, PlayerController player)
    {
        Debug.Log("✅ Correct Java C24");

        if (askText != null)
            askText.text = answer;

        if (player != null)
            TriggerAnimation(player, "Win");

        if (answer.Trim().ToLower() == "lightning" && lightningPrefab != null)
            lightningPrefab.SetActive(true);
            // lightningPrefab.transform.position = new Vector3(-3.5f, 1.2f, 0f);
            // lightningPrefab.transform.localScale = new Vector3(2f, 2f, 2f);  
    }

    public void Wrong(string answer, Text askText, PlayerController player)
    {
        Debug.Log("❌ Wrong Java C24");

        if (askText != null)
            askText.text = "Lose!";

        if (player != null)
            TriggerAnimation(player, "Lose");

        if (answer.Trim().ToLower() == "fireball")
            StartCoroutine(ShowFireballThenIdle(player));
    }

    private IEnumerator ShowFireballThenIdle(PlayerController player)
    {
        if (fireballPrefab != null)
            fireballPrefab.SetActive(true);
            // fireballPrefab.transform.position = new Vector3(-3.5f, 1.2f, 0f);
            // fireballPrefab.transform.localScale = new Vector3(2f, 2f, 2f);

        yield return new WaitForSeconds(1.5f); // แสดงลูกไฟซักพัก

        if (player != null)
            TriggerAnimation(player, "Idle");

        if (fireballPrefab != null)
            fireballPrefab.SetActive(false);
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
