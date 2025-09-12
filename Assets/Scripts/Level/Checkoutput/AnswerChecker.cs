using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.UI;
public class AnswerChecker : MonoBehaviour
{
    public CountdownTimer countdownTimer;
    public StarAnimator starAnimator;

    [Header("UI References")]
    public Text outputText;
    public Text scoreText;
    public GameObject nextLevelBG;
    public AnswerUIManager answerUIManager;
    public Text codeforCheck;

    [Header("Audio")]
    public AudioSource voiceGood;
    public AudioSource voiceBad;
    public AudioSource loseSound;

    [HideInInspector] public int currentScore = 100;
    public Action<int> onScoreChanged;

    void Start()
    {
        nextLevelBG?.SetActive(false);
        UpdateScoreUI();
    }

    public bool isAnswerCorrect = false;

    public IEnumerator CheckAnswer(string correctAnswer)
    {
        string userAnswer = outputText.text.Trim();

        // 🔁 แปลง \n ที่ถูกพิมพ์ใน Inspector ให้เป็น newline จริง
        correctAnswer = correctAnswer.Replace("\\n", "\n");

        // ✅ Normalize function: ตัด space ต่อบรรทัด + รวมใหม่
        string Normalize(string text)
        {
            string[] lines = text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim(); // ตัด space แต่ละบรรทัด
            }
            return string.Join("\n", lines).Trim(); // รวมกลับ
        }

        string cleanUserAnswer = Normalize(userAnswer);
        string cleanCorrectAnswer = Normalize(correctAnswer);

        if (cleanUserAnswer == cleanCorrectAnswer)
        {
            isAnswerCorrect = true;
            // countdownTimer.StopCountdown();
            // countdownTimer.SetTimeTextColorGreen();
            if (voiceGood != null)
            {
                voiceGood?.Play();
            }
        }
        else
        {
            isAnswerCorrect = false;
            loseSound?.Play();
            if (voiceBad != null) { 
                voiceBad?.Play();
            }
            UpdateScoreUI();
            onScoreChanged?.Invoke(currentScore);

            // 👇 Debug log ช่วยตรวจสอบความต่าง
            Debug.Log("❌ คำตอบไม่ถูก");
            Debug.Log($"[User]\n{cleanUserAnswer}");
            Debug.Log($"[Expected]\n{cleanCorrectAnswer}");
        }

        yield return null;
    }

    public void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = currentScore.ToString();
    }
}

