using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public AudioSource countdownSound;
    public int startTime = 120;

    public UnityEvent onCountdownEnd; // เรียกเมื่อหมดเวลา
    public UnityEvent<int> onScorePenalty; // เรียกเมื่อถึง 20s, 40s ฯลฯ

    private Coroutine countdownCoroutine;
    private int remainingTime;
    private int initialTime;

    public void StartCountdown(int seconds)
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);

        remainingTime = seconds;
        initialTime = seconds;
        countdownCoroutine = StartCoroutine(Countdown());
    }

    public void StopCountdown()
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);
    }

    private IEnumerator Countdown()
    {
        while (remainingTime > 0)
        {
            if (remainingTime >= 11)
            {
                timeText.text = remainingTime.ToString();
                timeText.enabled = true;
            }
            else
            {
                timeText.text = remainingTime.ToString();
                timeText.enabled = true;
                timeText.color = Color.red;

                if (remainingTime == 10 && countdownSound != null)
                {
                    countdownSound.Play();
                }
            }

            yield return new WaitForSeconds(1f);
            remainingTime--;

            int elapsed = initialTime - remainingTime;
            if (elapsed % 20 == 0 && elapsed != 0)
            {
                onScorePenalty.Invoke(elapsed); // ส่งเหตุการณ์เพื่อหักคะแนน
            }
        }

        timeText.text = "0";
        timeText.enabled = true;

        onCountdownEnd.Invoke(); // แจ้งว่าเวลาหมด
    }

    // เรียกใช้เมื่อผู้เล่นตอบถูก
    public void SetTimeTextColorGreen()
    {
        if (timeText != null)
        {
            timeText.color = Color.green;
        }
    }

}
