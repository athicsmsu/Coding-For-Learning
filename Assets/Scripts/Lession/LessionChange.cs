using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class LessionChange : MonoBehaviour
{
    public GameObject JavaEasy, JavaNormal, JavaHard;
    public GameObject PythonEasy, PythonNormal, PythonHard;
    public GameObject LoadingPanel;
    public string currentLession = "JavaEasy";
    int currentUserId = LoginForm.id;

    // เก็บ checkpoint ของแต่ละภาษา (Java, Python)
    private Dictionary<string, int> checkpoints = new Dictionary<string, int>();

    private readonly string[] lessonOrder = {
        "JavaEasy", "JavaNormal", "JavaHard",
        "PythonEasy", "PythonNormal", "PythonHard"
    };

    void Start()
    {
        LoadingPanel.gameObject.SetActive(true);
        Debug.Log($"✅ Login success, uid: {currentUserId}");
        SetLesson(currentLession);

        // โหลด checkpoint ของ Java และ Python พร้อมกัน
        StartCoroutine(GetLatestLevel("Java"));
        StartCoroutine(GetLatestLevel("Python"));
    }


    public void SetCheckpoint(string language, int value)
    {
        checkpoints[language] = value;
        Debug.Log($"✅ Checkpoint for {language} set to: {value}");
    }

    public int GetCheckpoint(string language)
    {
        return checkpoints.ContainsKey(language) ? checkpoints[language] : 0;
    }

    public void ApplyCheckpoint(string lessonName)
    {
        SetLesson(lessonName);

        GameObject group = GetLessonObjectByName(lessonName);
        if (group == null) return;

        int start = 1, end = 10;

        if (lessonName.StartsWith("Java"))
        {
            if (lessonName.Contains("Easy")) { start = 1; end = 10; }
            else if (lessonName.Contains("Normal")) { start = 11; end = 21; }
            else if (lessonName.Contains("Hard")) { start = 22; end = 27; }
        }
        else if (lessonName.StartsWith("Python"))
        {
            if (lessonName.Contains("Easy")) { start = 1; end = 9; }
            else if (lessonName.Contains("Normal")) { start = 10; end = 19; }
            else if (lessonName.Contains("Hard")) { start = 20; end = 25; }
        }

        // เอา checkpoint ตามภาษา
        string langKey = lessonName.StartsWith("Java") ? "Java" : "Python";
        int checkpoint = GetCheckpoint(langKey);

        UpdateGroup(group, start, end, checkpoint);
    }

    private void UpdateGroup(GameObject group, int rangeStart, int rangeEnd, int checkpoint)
    {
        LessonButton[] buttons = group.GetComponentsInChildren<LessonButton>(true);

        int currentLevel = rangeStart;
        foreach (var btn in buttons)
        {
            if (currentLevel > rangeEnd) break;

            bool canPlay = currentLevel <= checkpoint;
            btn.SetStatus(canPlay);

            currentLevel++;
        }
        LoadingPanel.gameObject.SetActive(false);
    }

    public void SetLesson(string lessonName)
    {
        currentLession = lessonName;

        JavaEasy.SetActive(false); JavaNormal.SetActive(false); JavaHard.SetActive(false);
        PythonEasy.SetActive(false); PythonNormal.SetActive(false); PythonHard.SetActive(false);

        GameObject lesson = GetLessonObjectByName(lessonName);
        if (lesson != null) lesson.SetActive(true);
    }

    private GameObject GetLessonObjectByName(string name)
    {
        return name switch
        {
            "JavaEasy" => JavaEasy,
            "JavaNormal" => JavaNormal,
            "JavaHard" => JavaHard,
            "PythonEasy" => PythonEasy,
            "PythonNormal" => PythonNormal,
            "PythonHard" => PythonHard,
            _ => null
        };
    }

    public void ClickArrowLeft()
    {
        LoadingPanel.gameObject.SetActive(true);
        int index = System.Array.IndexOf(lessonOrder, currentLession);
        if (index > 0)
        {
            string newLesson = lessonOrder[index - 1];
            SetLesson(newLesson);
            ApplyCheckpoint(newLesson);
        }
        else
        {
            LoadingPanel.gameObject.SetActive(false);
        }
    }

    public void ClickArrowRight()
    {
        LoadingPanel.gameObject.SetActive(true);
        int index = System.Array.IndexOf(lessonOrder, currentLession);
        if (index < lessonOrder.Length - 1)
        {
            string newLesson = lessonOrder[index + 1];
            SetLesson(newLesson);
            ApplyCheckpoint(newLesson);
        }
        else
        {
            LoadingPanel.gameObject.SetActive(false);
        }
    }

    private IEnumerator GetLatestLevel(string languageKey)
    {
        int language = languageKey == "Java" ? 1 : 0;
        string url = $"https://codingforlearning.onrender.com/gameplay/latest-level/{currentUserId}/{language}";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("🌐 API Error: " + request.error);
            yield break;
        }

        try
        {
            string json = request.downloadHandler.text;
            LatestLevelResponse response = JsonUtility.FromJson<LatestLevelResponse>(json);

            SetCheckpoint(languageKey, response.latestLevel);

            // อัปเดตเฉพาะถ้ากำลังเปิดภาษาเดียวกันอยู่
            if (currentLession.StartsWith(languageKey))
            {
                ApplyCheckpoint(currentLession);
                LoadingPanel.gameObject.SetActive(false);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("❌ JSON Parse Error: " + ex.Message);
        }
    }


    [System.Serializable]
    public class LatestLevelResponse
    {
        public int latestLevel;
    }
}
