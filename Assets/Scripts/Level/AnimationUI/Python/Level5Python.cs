using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Level5Python : MonoBehaviour
{
        public GameObject bgPrefab;
    public GameObject askTxt;
    public GameObject askQuiz;
    public void Correct(string answer, Text askText, PlayerController player)
    {
        Debug.Log("Correct Python C5");

        if (askText != null)
        {
            if (!string.IsNullOrEmpty(answer))
            {
                // ลบ <, >, ' ทุกตัวใน answer
                string cleanAnswer = answer.Replace("<", "").Replace(">", "").Replace("'", "").Trim();

                // ถ้าเริ่มต้นด้วย "class " ให้ตัด "class " ออก
                if (cleanAnswer.StartsWith("class "))
                {
                    cleanAnswer = cleanAnswer.Substring(6).Trim();
                }

                askText.text = cleanAnswer;
            }
            else
            {
                askText.text = answer;
            }
        }

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
        Debug.Log("Wrong Python W5");
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
