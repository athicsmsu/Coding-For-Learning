using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;
using System.Collections;
using UnityEngine.Networking;
public class ChangeScenes : MonoBehaviour
{
    public static Stack<string> sceneStack = new Stack<string>(); // Stack สำหรับเก็บชื่อซีน
    public static string Language = ""; // ภาษาเริ่มต้น
    public static string Level = ""; // เลเวลเริ่มต้น
    public static int checkpoint; // เช็คพ้อยเริ่มต้น
    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        // ถ้า Stack ว่าง ให้เพิ่มซีนแรกเข้าไป
        if (sceneStack.Count == 0)
        {
            sceneStack.Push(currentScene);
        }

        Debug.Log("Start : " + currentScene);
    }
    public void CheckStatusLogin(string sceneName)
    {
        // เช็คสถานะการล็อกอิน
        if (LoginForm.LoginStatus)
        {
            LoadScene(sceneName);
        }
        else
        {
            Debug.Log("Please login first.");
            LoginForm.ToggleLoginForm(); // เรียกใช้ฟังก์ชัน ToggleLoginForm เพื่อแสดงฟอร์มล็อกอิน
        }
    }
    private IEnumerator FetchLatestLevelThenLoadScene()
    {
        yield return StartCoroutine(GetLatestLevel());
        LoadScene("Level");
    }
    private IEnumerator GetLatestLevel()
    {
        int language = 0;    // 0 = Python // 1 = Java
        if (Language == "Python")
        {
            language = 0;
        }
        else if (Language == "Java")
        {
            language = 1;
        }
        string url = $"https://codingforlearning.onrender.com/gameplay/latest-level/{LoginForm.id}/{language}";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("API Error: " + request.error);
        }
        else
        {
            // แปลง JSON และดึง level
            try
            {
                string jsonText = request.downloadHandler.text;
                LatestLevelResponse response = JsonUtility.FromJson<LatestLevelResponse>(jsonText);
                int latestLevel = response.latestLevel;

                SetCheckpoint(latestLevel);
                Debug.Log("Latest Level: " + latestLevel);  // แสดงผลลัพธ์ที่ได้จาก API
            }
            catch (System.Exception ex)
            {
                Debug.LogError("JSON Parse Error: " + ex.Message);
            }
        }
    }
    [System.Serializable]
    public class LatestLevelResponse
    {
        public int latestLevel;
    }


    public void LoadScene(string sceneName)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        sceneStack.Push(sceneName);

        Debug.Log("Switching from " + currentScene + " to " + sceneName);

        // โหลดซีนใหม่
        SceneManager.LoadScene(sceneName);
    }

    public void BackScene()
    {
        if (sceneStack.Count > 1) // เช็คว่ามีซีนก่อนหน้าใน Stack
        {
            // นำซีนปัจจุบันออกจาก Stack
            sceneStack.Pop();

            // โหลดซีนก่อนหน้า
            string previousScene = sceneStack.Peek();
            Debug.Log("Back to: " + previousScene);
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.LogWarning("No more scenes to go back to.");
        }
    }

    public void BackToMain()
    {
        sceneStack.Clear(); // ล้าง Stack ทั้งหมด
        SceneManager.LoadScene("Main");
    }
    public void ExitGame()
    {
        sceneStack.Clear();
        Application.Quit(); // ปิดโปรแกรม
        Debug.Log("Game is exiting...");
    }

    public void SetLanguage(string language)
    {
        Language = language;
        Debug.Log("Language set to: " + Language);
        StartCoroutine(FetchLatestLevelThenLoadScene());
    }

    public void SetLevel(string level)
    {
        Level = level;
        Debug.Log("Level set to: " + Level);
    }
    public void SetCheckpoint(int checkpointValue)
    {
        // เช็คพ้อยเริ่มต้น ต้องโหลดจากดาต้าเบสใช้ methdod นี้ในการโหลด
        checkpoint = checkpointValue;
        Debug.Log("Checkpoint set to: " + checkpoint);
    }
    public void ChangeToProfileScene()
    {
        if (LoginForm.LoginStatus)
        {
            LoadScene("Profile");
        }
    }
    public void PlayAgain()
    {
        // ลบ scene ล่าสุด 2 ตัว เพื่อย้อนกลับก่อนเข้าเล่นซ้ำ
        if (sceneStack.Count >= 2)
        {
            sceneStack.Pop(); // ปัจจุบัน
        }

        Debug.Log("Replay current level at checkpoint: " + checkpoint);

        // เรียกด่านใหม่ตาม checkpoint เดิม (สุ่มใหม่ถ้าเป็น Random)
        ChangeSceneByCheckPointToPlay();
    }

    public void GoToNextLevel()
    {
        // ✅ ลบ scene ออก 2 หน้า (ถ้ามีพอ)
        if (sceneStack.Count >= 2)
        {
            sceneStack.Pop(); // หน้าปัจจุบัน
            sceneStack.Pop(); // หน้าก่อนหน้า
        }

        // ✅ เพิ่ม checkpoint
        checkpoint += 1;

        // ✅ ตรวจสอบ max checkpoint ตาม Level
        int maxCheckpoint = 0;
        if (Language == "Java")
        {
            if (Level == "Easy") maxCheckpoint = 10;
            else if (Level == "Normal") maxCheckpoint = 21;
            else if (Level == "Hard") maxCheckpoint = 27;
        }
        else if (Language == "Python")
        {
            if (Level == "Easy") maxCheckpoint = 9; // ปรับตาม Python
            else if (Level == "Normal") maxCheckpoint = 19; // ปรับตาม Python
            else if (Level == "Hard") maxCheckpoint = 25; // ปรับตาม Python
        }
        // ✅ ถ้าเกิน max checkpoint ให้กลับไปหน้าแรก
        if (checkpoint > maxCheckpoint)
        {
            StartCoroutine(CheckQuestionnaireThenDecide());
            return;
        }

        Debug.Log("Going to next level. New checkpoint: " + checkpoint);
        ChangeSceneByCheckPointToInfo();
    }

    public void ChangeScene(string sceneName)
    {
        LoadScene(sceneName);
    }
    public void ChangeScenesRandom(string scene1, string scene2)
    {
        int randomIndex = Random.Range(0, 2);
        string selectedScene = "";

        switch (randomIndex)
        {
            case 0:
                selectedScene = scene1;
                break;
            case 1:
                selectedScene = scene2;
                break;
        }

        LoadScene(selectedScene);
    }
    public void ChangeScenesRandom(string scene1, string scene2, string scene3)
    {
        int randomIndex = Random.Range(0, 3);
        string selectedScene = "";

        switch (randomIndex)
        {
            case 0:
                selectedScene = scene1;
                break;
            case 1:
                selectedScene = scene2;
                break;
            case 2:
                selectedScene = scene3;
                break;
        }

        LoadScene(selectedScene);
    }
    public void ChangeSceneByCheckPointToPlay()
    {
        if (Language == "Java")
        {
            if (Level == "Easy")
            {
                switch (checkpoint)
                {
                    case 1:
                        ChangeScene("JavaLevel1-1");
                        break;
                    case 2:
                        ChangeScenesRandom("JavaLevel2-1", "JavaLevel2-2", "JavaLevel2-3");
                        break;
                    case 3:
                        ChangeScenesRandom("JavaLevel3-1", "JavaLevel3-2", "JavaLevel3-3");
                        break;
                    case 4:
                        ChangeScenesRandom("JavaLevel4-1", "JavaLevel4-2");
                        break;
                    case 5:
                        ChangeScenesRandom("JavaLevel5-1", "JavaLevel5-2", "JavaLevel5-3");
                        break;
                    case 6:
                        ChangeScenesRandom("JavaLevel6-1", "JavaLevel6-2", "JavaLevel6-3");
                        break;
                    case 7:
                        ChangeScenesRandom("JavaLevel7-1", "JavaLevel7-2", "JavaLevel7-3");
                        break;
                    case 8:
                        ChangeScenesRandom("JavaLevel8-1", "JavaLevel8-2", "JavaLevel8-3");
                        break;
                    case 9:
                        ChangeScenesRandom("JavaLevel9-1", "JavaLevel9-2", "JavaLevel9-3");
                        break;
                    case 10:
                        ChangeScenesRandom("JavaLevel10-1", "JavaLevel10-2", "JavaLevel10-3");
                        break;
                    default:
                        Debug.Log("Invalid checkpoint value: " + checkpoint);
                        break;
                }
            }
            else if (Level == "Normal")
            {
                switch (checkpoint)
                {
                    case 11:
                        ChangeScenesRandom("JavaLevel11-1", "JavaLevel11-2", "JavaLevel11-3");
                        break;
                    case 12:
                        ChangeScenesRandom("JavaLevel12-1", "JavaLevel12-2", "JavaLevel12-3");
                        break;
                    case 13:
                        ChangeScenesRandom("JavaLevel13-1", "JavaLevel13-2", "JavaLevel13-3");
                        break;
                    case 14:
                        ChangeScenesRandom("JavaLevel14-1", "JavaLevel14-2", "JavaLevel14-3");
                        break;
                    case 15:
                        ChangeScenesRandom("JavaLevel15-1", "JavaLevel15-2", "JavaLevel15-3");
                        break;
                    case 16:
                        ChangeScenesRandom("JavaLevel16-1", "JavaLevel16-2");
                        break;
                    case 17:
                        ChangeScenesRandom("JavaLevel17-1", "JavaLevel17-2");
                        break;
                    case 18:
                        ChangeScenesRandom("JavaLevel18-1", "JavaLevel18-2", "JavaLevel18-3");
                        break;
                    case 19:
                        ChangeScenesRandom("JavaLevel19-1", "JavaLevel19-2", "JavaLevel19-3");
                        break;
                    case 20:
                        ChangeScenesRandom("JavaLevel20-1", "JavaLevel20-2", "JavaLevel20-3");
                        break;
                    case 21:
                        ChangeScenesRandom("JavaLevel21-1", "JavaLevel21-2", "JavaLevel21-3");
                        break;
                    default:
                        Debug.Log("Invalid checkpoint value: " + checkpoint);
                        break;
                }
            }
            else if (Level == "Hard")
            {
                switch (checkpoint)
                {
                    case 22:
                        ChangeScenesRandom("JavaLevel22-1", "JavaLevel22-2");
                        break;
                    case 23:
                        ChangeScenesRandom("JavaLevel23-1", "JavaLevel23-2");
                        break;
                    case 24:
                        ChangeScenesRandom("JavaLevel24-1", "JavaLevel24-2");
                        break;
                    case 25:
                        ChangeScenesRandom("JavaLevel25-1", "JavaLevel25-2");
                        break;
                    case 26:
                        ChangeScenesRandom("JavaLevel26-1", "JavaLevel26-2");
                        break;
                    case 27:
                        ChangeScenesRandom("JavaLevel27-1", "JavaLevel27-2");
                        break;
                    default:
                        Debug.Log("Invalid checkpoint value: " + checkpoint);
                        break;
                }
            }
        }
        else if (Language == "Python")
        {
            if (Level == "Easy")
            {
                switch (checkpoint)
                {
                    case 1:
                        ChangeScene("PythonLevel1-1");
                        break;
                    case 2:
                        ChangeScenesRandom("PythonLevel2-1", "PythonLevel2-2", "PythonLevel2-3");
                        break;
                    case 3:
                        ChangeScenesRandom("PythonLevel3-1", "PythonLevel3-2", "PythonLevel3-3");
                        break;
                    case 4:
                        ChangeScenesRandom("PythonLevel4-1", "PythonLevel4-2");
                        break;
                    case 5:
                        ChangeScenesRandom("PythonLevel5-1", "PythonLevel5-2", "PythonLevel5-3");
                        break;
                    case 6:
                        ChangeScenesRandom("PythonLevel6-1", "PythonLevel6-2", "PythonLevel6-3");
                        break;
                    case 7:
                        ChangeScenesRandom("PythonLevel7-1", "PythonLevel7-2", "PythonLevel7-3");
                        break;
                    case 8:
                        ChangeScenesRandom("PythonLevel8-1", "PythonLevel8-2", "PythonLevel8-3");
                        break;
                    case 9:
                        ChangeScenesRandom("PythonLevel9-1", "PythonLevel9-2", "PythonLevel9-3");
                        break;
                    default:
                        Debug.Log("Invalid checkpoint value: " + checkpoint);
                        break;
                }
            }
            else if (Level == "Normal")
            {
                switch (checkpoint)
                {
                    case 10:
                        ChangeScenesRandom("PythonLevel10-1", "PythonLevel10-2", "PythonLevel10-3");
                        break;
                    case 11:
                        ChangeScenesRandom("PythonLevel11-1", "PythonLevel11-2", "PythonLevel11-3");
                        break;
                    case 12:
                        ChangeScenesRandom("PythonLevel12-1", "PythonLevel12-2", "PythonLevel12-3");
                        break;
                    case 13:
                        ChangeScenesRandom("PythonLevel13-1", "PythonLevel13-2", "PythonLevel13-3");
                        break;
                    case 14:
                        ChangeScenesRandom("PythonLevel14-1", "PythonLevel14-2");
                        break;
                    case 15:
                        ChangeScenesRandom("PythonLevel15-1", "PythonLevel15-2");
                        break;
                    case 16:
                        ChangeScenesRandom("PythonLevel16-1", "PythonLevel16-2", "PythonLevel16-3");
                        break;
                    case 17:
                        ChangeScenesRandom("PythonLevel17-1", "PythonLevel17-2", "PythonLevel17-3");
                        break;
                    case 18:
                        ChangeScenesRandom("PythonLevel18-1", "PythonLevel18-2", "PythonLevel18-3");
                        break;
                    case 19:
                        ChangeScenesRandom("PythonLevel19-1", "PythonLevel19-2", "PythonLevel19-3");
                        break;
                    default:
                        Debug.Log("Invalid checkpoint value: " + checkpoint);
                        break;
                }
            }
            else if (Level == "Hard")
            {
                switch (checkpoint)
                {
                    case 20:
                        ChangeScenesRandom("PythonLevel20-1", "PythonLevel20-2");
                        break;
                    case 21:
                        ChangeScenesRandom("PythonLevel21-1", "PythonLevel21-2");
                        break;
                    case 22:
                        ChangeScenesRandom("PythonLevel22-1", "PythonLevel22-2");
                        break;
                    case 23:
                        ChangeScenesRandom("PythonLevel23-1", "PythonLevel23-2");
                        break;
                    case 24:
                        ChangeScenesRandom("PythonLevel24-1", "PythonLevel24-2");
                        break;
                    case 25:
                        ChangeScenesRandom("PythonLevel25-1", "PythonLevel25-2");
                        break;
                    default:
                        Debug.Log("Invalid checkpoint value: " + checkpoint);
                        break;
                }
            }
        }
    }
    public void ChangeSceneByCheckPointToInfo()
    {
        if (Language == "Java")
        {
            if (Level == "Easy")
            {
                switch (checkpoint)
                {
                    case 1:
                        ChangeScene("JavaInfoLevel1");
                        break;
                    case 2:
                        ChangeScene("JavaInfoLevel2");
                        break;
                    case 3:
                        ChangeScene("JavaInfoLevel3");
                        break;
                    case 4:
                        ChangeScene("JavaInfoLevel4");
                        break;
                    case 5:
                        ChangeScene("JavaInfoLevel5");
                        break;
                    case 6:
                        ChangeScene("JavaInfoLevel6");
                        break;
                    case 7:
                        ChangeScene("JavaInfoLevel7");
                        break;
                    case 8:
                        ChangeScene("JavaInfoLevel8");
                        break;
                    case 9:
                        ChangeScene("JavaInfoLevel9");
                        break;
                    case 10:
                        ChangeScene("JavaInfoLevel10");
                        break;
                    default:
                        StartCoroutine(CheckQuestionnaireThenDecideForInfo());
                        Debug.Log("Invalid checkpoint value: " + checkpoint);
                        break;
                }
            }
            else if (Level == "Normal")
            {
                switch (checkpoint)
                {
                    case 11:
                        ChangeScene("JavaInfoLevel11");
                        break;
                    case 12:
                        ChangeScene("JavaInfoLevel12");
                        break;
                    case 13:
                        ChangeScene("JavaInfoLevel13");
                        break;
                    case 14:
                        ChangeScene("JavaInfoLevel14");
                        break;
                    case 15:
                        ChangeScene("JavaInfoLevel15");
                        break;
                    case 16:
                        ChangeScene("JavaInfoLevel16");
                        break;
                    case 17:
                        ChangeScene("JavaInfoLevel17");
                        break;
                    case 18:
                        ChangeScene("JavaInfoLevel18");
                        break;
                    case 19:
                        ChangeScene("JavaInfoLevel19");
                        break;
                    case 20:
                        ChangeScene("JavaInfoLevel20");
                        break;
                    case 21:
                        ChangeScene("JavaInfoLevel21");
                        break;
                    default:
                        StartCoroutine(CheckQuestionnaireThenDecideForInfo());
                        Debug.Log("Invalid checkpoint value: " + checkpoint);
                        break;
                }
            }
            else if (Level == "Hard")
            {
                switch (checkpoint)
                {
                    case 22:
                        ChangeScene("JavaInfoLevel22");
                        break;
                    case 23:
                        ChangeScene("JavaInfoLevel23");
                        break;
                    case 24:
                        ChangeScene("JavaInfoLevel24");
                        break;
                    case 25:
                        ChangeScene("JavaInfoLevel25");
                        break;
                    case 26:
                        ChangeScene("JavaInfoLevel26");
                        break;
                    case 27:
                        ChangeScene("JavaInfoLevel27");
                        break;
                    default:
                        StartCoroutine(CheckQuestionnaireThenDecideForInfo());
                        Debug.Log("Invalid checkpoint value: " + checkpoint);
                        break;
                }
            }
        }
        else if (Language == "Python")
        {
            if (Level == "Easy")
            {
                switch (checkpoint)
                {
                    case 1:
                        ChangeScene("PythonInfoLevel1");
                        break;
                    case 2:
                        ChangeScene("PythonInfoLevel2");
                        break;
                    case 3:
                        ChangeScene("PythonInfoLevel3");
                        break;
                    case 4:
                        ChangeScene("PythonInfoLevel4");
                        break;
                    case 5:
                        ChangeScene("PythonInfoLevel5");
                        break;
                    case 6:
                        ChangeScene("PythonInfoLevel6");
                        break;
                    case 7:
                        ChangeScene("PythonInfoLevel7");
                        break;
                    case 8:
                        ChangeScene("PythonInfoLevel8");
                        break;
                    case 9:
                        ChangeScene("PythonInfoLevel9");
                        break;
                    default:
                        StartCoroutine(CheckQuestionnaireThenDecideForInfo());
                        Debug.Log("Invalid checkpoint value: " + checkpoint);
                        break;
                }
            }
            else if (Level == "Normal")
            {
                switch (checkpoint)
                {
                    case 10:
                        ChangeScene("PythonInfoLevel10");
                        break;
                    case 11:
                        ChangeScene("PythonInfoLevel11");
                        break;
                    case 12:
                        ChangeScene("PythonInfoLevel12");
                        break;
                    case 13:
                        ChangeScene("PythonInfoLevel13");
                        break;
                    case 14:
                        ChangeScene("PythonInfoLevel14");
                        break;
                    case 15:
                        ChangeScene("PythonInfoLevel15");
                        break;
                    case 16:
                        ChangeScene("PythonInfoLevel16");
                        break;
                    case 17:
                        ChangeScene("PythonInfoLevel17");
                        break;
                    case 18:
                        ChangeScene("PythonInfoLevel18");
                        break;
                    case 19:
                        ChangeScene("PythonInfoLevel19");
                        break;
                    default:
                        StartCoroutine(CheckQuestionnaireThenDecideForInfo());
                        Debug.Log("Invalid checkpoint value: " + checkpoint);
                        break;
                }
            }
            else if (Level == "Hard")
            {
                switch (checkpoint)
                {
                    case 20:
                        ChangeScene("PythonInfoLevel20");
                        break;
                    case 21:
                        ChangeScene("PythonInfoLevel21");
                        break;
                    case 22:
                        ChangeScene("PythonInfoLevel22");
                        break;
                    case 23:
                        ChangeScene("PythonInfoLevel23");
                        break;
                    case 24:
                        ChangeScene("PythonInfoLevel24");
                        break;
                    case 25:
                        ChangeScene("PythonInfoLevel25");
                        break;
                    default:
                        StartCoroutine(CheckQuestionnaireThenDecideForInfo());
                        Debug.Log("Invalid checkpoint value: " + checkpoint);
                        break;
                }
            }
        }
    }
    private IEnumerator CheckQuestionnaireThenDecide()
    {
        string url = $"https://codingforlearning.onrender.com/question/questionnaire/check/{LoginForm.id}/{Language}{Level}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("API Error: " + request.error);
            StartCoroutine(FetchLatestLevelThenLoadScene());
            yield break;
        }

        try
        {
            string jsonText = request.downloadHandler.text;
            QuestionnaireResponse response = JsonUtility.FromJson<QuestionnaireResponse>(jsonText);

            if (response.answered)
            {
                StartCoroutine(FetchLatestLevelThenLoadScene());
            }
            else
            {
                LoadScene("LevelComplete");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("JSON Parse Error: " + ex.Message);
            StartCoroutine(FetchLatestLevelThenLoadScene());
        }
    }

    [System.Serializable]
    public class QuestionnaireResponse
    {
        public bool answered;
    }
    private IEnumerator CheckQuestionnaireThenDecideForInfo()
    {
        string url = $"https://codingforlearning.onrender.com/question/questionnaire/check/{LoginForm.id}/{Language}{Level}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("API Error: " + request.error);
            yield break;
        }

        try
        {
            string jsonText = request.downloadHandler.text;
            QuestionnaireResponse response = JsonUtility.FromJson<QuestionnaireResponse>(jsonText);

            if (response.answered)
            {
                LoadScene("Lession");
            }
            else
            {
                LoadScene("LevelComplete");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("JSON Parse Error: " + ex.Message);
        }
    }

}
