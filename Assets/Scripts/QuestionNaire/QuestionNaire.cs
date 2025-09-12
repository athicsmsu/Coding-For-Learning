using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Question
{
    public int qid;
    public string question;
    public string correct;
    public string choiceA;
    public string choiceB;
    public string choiceC;
    public string choiceD;
}

[System.Serializable]
public class QuestionResponse
{
    public string message;
    public Question[] questions;
}

[System.Serializable]
public class AnswerData
{
    public int qid;
    public string answer;
    public int point;

    public AnswerData(int qid, string answer, int point)
    {
        this.qid = qid;
        this.answer = answer;
        this.point = point;
    }
}

[System.Serializable]
public class AnswerPayload
{
    public int uid;
    public AnswerData[] answers;

    public AnswerPayload(int uid, AnswerData[] answers)
    {
        this.uid = uid;
        this.answers = answers;
    }
}

public class QuestionNaire : MonoBehaviour
{
    [Header("UI Elements")]
    public Text numberText;
    public Text questionText;
    public Text choiceAText;
    public Text choiceBText;
    public Text choiceCText;
    public Text choiceDText;
    public Button choiceAButton;
    public Button choiceBButton;
    public Button choiceCButton;
    public Button choiceDButton;

    [Header("Panels")]
    public GameObject panelCorrect;
    public GameObject panelWrong;

    private Question[] questions;
    private int currentIndex = 0;
    private int startQid = 0;
    private int endQid = 0;
    private int language = 0;
    private string level = "";

    private List<AnswerData> answerList = new List<AnswerData>();

    void Start()
    {
        Debug.Log("id: " + LoginForm.id);
        if (panelCorrect != null) panelCorrect.SetActive(false);
        if (panelWrong != null) panelWrong.SetActive(false);

        language = (ChangeScenes.Language == "Python") ? 0 : 1;
        level = ChangeScenes.Level;

        // กำหนดช่วง QID
        if (language == 1 && level == "Easy") { startQid = 1; endQid = 10; }
        else if (language == 1 && level == "Normal") { startQid = 11; endQid = 20; }
        else if (language == 1 && level == "Hard") { startQid = 21; endQid = 30; }
        else if (language == 0 && level == "Easy") { startQid = 31; endQid = 40; }
        else if (language == 0 && level == "Normal") { startQid = 41; endQid = 50; }
        else if (language == 0 && level == "Hard") { startQid = 51; endQid = 60; }

        StartCoroutine(ShowLoadingAndLoad());

        choiceAButton.onClick.AddListener(() => CheckAnswer("A"));
        choiceBButton.onClick.AddListener(() => CheckAnswer("B"));
        choiceCButton.onClick.AddListener(() => CheckAnswer("C"));
        choiceDButton.onClick.AddListener(() => CheckAnswer("D"));
    }

    private IEnumerator ShowLoadingAndLoad()
    {
        QuestionLoader.ShowLoadingScreen();
        yield return null;
        yield return StartCoroutine(LoadQuestionsFromAPI());
    }

    private IEnumerator LoadQuestionsFromAPI()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("https://codingforlearning.onrender.com/question"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching questions: " + request.error);
            }
            else
            {
                QuestionResponse response = JsonUtility.FromJson<QuestionResponse>(request.downloadHandler.text);
                questions = System.Array.FindAll(response.questions, q => q.qid >= startQid && q.qid <= endQid);

                if (questions != null && questions.Length > 0)
                {
                    currentIndex = 0;
                    DisplayQuestion(currentIndex);
                }
                else
                {
                    Debug.LogWarning("No questions found.");
                }
            }
        }
    }

    void DisplayQuestion(int index)
    {
        if (index < 0 || index >= questions.Length) return;

        Question q = questions[index];
        numberText.text = (index + 1).ToString();
        questionText.text = q.question;
        choiceAText.text = q.choiceA;
        choiceBText.text = q.choiceB;
        choiceCText.text = q.choiceC;
        choiceDText.text = q.choiceD;

        LayoutRebuilder.ForceRebuildLayoutImmediate(questionText.rectTransform);
    }

    void CheckAnswer(string selectedChoice)
    {
        Question currentQuestion = questions[currentIndex];
        bool isCorrect = selectedChoice == currentQuestion.correct;
        int point = isCorrect ? 10 : 0;

        answerList.Add(new AnswerData(currentQuestion.qid, selectedChoice, point));

        if (isCorrect)
            panelCorrect.SetActive(true);
        else
            panelWrong.SetActive(true);
    }

    public void NextQuestion()
    {
        panelCorrect.SetActive(false);
        panelWrong.SetActive(false);

        currentIndex++;
        if (currentIndex < questions.Length)
            DisplayQuestion(currentIndex);
        else
            StartCoroutine(SendAnswersToDatabase());
    }

    private IEnumerator SendAnswersToDatabase()
    {
        string url = "https://codingforlearning.onrender.com/question/questionnaire/submit";

        AnswerPayload payload = new AnswerPayload(LoginForm.id, answerList.ToArray());
        string jsonData = JsonUtility.ToJson(payload);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            ChangeScenes.sceneStack.Pop();
            ChangeScenes.sceneStack.Pop();
            SceneManager.LoadScene("Level");
        }
        else
        {
            Debug.LogError("❌ ส่งคำตอบไม่สำเร็จ: " + request.error);
        }
    }
}
