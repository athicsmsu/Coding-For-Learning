using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic; // ✅ ต้องมีอันนี้
public class LevelGameplayManager : MonoBehaviour
{
    public LevelConfig levelData;
    public AnswerChecker answerChecker;
    private string Language = "";
    private int UIcheckpoint = 0;

    void Start()
    {
        UIcheckpoint = 9;
        Language = "Java";

        answerChecker.currentScore = levelData.startScore;
        answerChecker.countdownTimer.onCountdownEnd.AddListener(OnTimeEnd);
        answerChecker.countdownTimer.onScorePenalty.AddListener(HandleScorePenalty);
        answerChecker.countdownTimer.StartCountdown(180);

    }

    public void CheckOutput()
    {
        StartCoroutine(CheckAnswerAndSendIfCorrect());
    }

    private IEnumerator CheckAnswerAndSendIfCorrect()
    {
        yield return StartCoroutine(answerChecker.CheckAnswer(levelData.correctAnswer));

        if (answerChecker.isAnswerCorrect)
        {
            // ✅ ตรวจโครงสร้างโค้ด ถ้ามี
            string userCode = answerChecker.codeforCheck.text;
            string feedback = GetStructureCheckFeedback(userCode);

            if (feedback == "✅")
            {
                Debug.Log("✅ ผ่านเงื่อนไขทั้งหมด");
                // ✅ เงื่อนไขครบ: คำตอบถูก + โครงสร้างผ่าน
                //แสดงท่าทางชนะ
                answerChecker.answerUIManager.ShowCorrectAnswer(answerChecker.outputText.text);

                // ถ้าเป็น Level 9 Java ให้รอ 6 วินาทีเพื่อให้เห็นการเปลี่ยนแปลง
                // if (Language == "Java" && UIcheckpoint == 9)
                // {
                //     yield return new WaitForSeconds(6f);
                // }
                // else
                // {
                //     yield return new WaitForSeconds(3f);
                // }
                answerChecker.countdownTimer.StopCountdown();
                answerChecker.countdownTimer.SetTimeTextColorGreen();
                yield return new WaitForSeconds(5f);

                answerChecker.nextLevelBG?.SetActive(true);
                answerChecker.answerUIManager.HideCharacter();
                answerChecker.starAnimator?.gameObject.SetActive(true);
                answerChecker.starAnimator?.ShowStars(answerChecker.currentScore);
                StartCoroutine(SendGameplayData());
            }
            else
            {
                Debug.Log("⚠️ ไม่ผ่าน:\n" + feedback);
                // ❌ ยังไม่ส่งข้อมูล เพราะโครงสร้างไม่ผ่าน
                //แสดงท่าทางแพ้
                answerChecker.answerUIManager.ShowWrongAnswer(answerChecker.outputText.text);
            }
        }
        else
        {
            Debug.Log("❌ คำตอบไม่ถูก");
            // ❌ แสดงแพ้ (output ผิด)
            answerChecker.answerUIManager.ShowWrongAnswer(answerChecker.outputText.text);
        }
    }


    void OnTimeEnd()
    {
        answerChecker.currentScore = levelData.minScore;
        answerChecker.UpdateScoreUI();
    }

    void HandleScorePenalty(int elapsedTime)
    {
        if (answerChecker.currentScore > levelData.minScore)
        {
            answerChecker.currentScore -= levelData.scorePenalty;
            if (answerChecker.currentScore < levelData.minScore)
                answerChecker.currentScore = levelData.minScore;

            answerChecker.UpdateScoreUI();
        }
    }
    private string GetStructureCheckFeedback(string playerCode)
    {
        if (levelData.codeChecks == null || levelData.codeChecks.Length == 0)
            return "✅";  // ไม่มีให้เช็ค = ผ่านเลย

        List<string> messages = new List<string>();

        foreach (var check in levelData.codeChecks)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(playerCode, check.pattern))
            {
                messages.Add("❌ " + check.failMessage);
            }
        }

        return messages.Count == 0 ? "✅" : string.Join("\n", messages);
    }


    private IEnumerator SendGameplayData()
    {
        string url = "https://codingforlearning.onrender.com/gameplay/add-gameplay";
        int uid = LoginForm.id;
        int language = ChangeScenes.Language == "Java" ? 1 : 0;
        int level = levelData.level;
        int mission = levelData.mission;
        int score = answerChecker.currentScore;

        string json = JsonUtility.ToJson(new GameplayData
        {
            uid = uid,
            language = language,
            level = level,
            mission = mission,
            score = score
        });

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Gameplay sent!");
        }
        else
        {
            Debug.LogError("❌ Failed to send gameplay data: " + request.error);
        }
    }

    [System.Serializable]
    public class GameplayData
    {
        public int uid;
        public int language;
        public int level;
        public int mission;
        public int score;
    }
}
