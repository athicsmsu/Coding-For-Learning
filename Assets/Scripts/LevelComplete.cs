using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ScoreSummary : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI LvlText; 

    [System.Serializable]
    public class ScoreRecord
    {
        public int language;   // 1 = Java, 2 = Python
        public int level;      // เลเวลที่เล่น
        public int totalScore; // คะแนนรวมของด่านนั้น
    }

    private string apiUrl = "https://codingforlearning.onrender.com/gameplay/raw-scores";

    void OnEnable()
    {
        StartCoroutine(LoadScoreSummary());
    }

    private IEnumerator LoadScoreSummary()
    {
        LvlText.text = $"คุณผ่านระดับ {ChangeScenes.Language}{ChangeScenes.Level} แล้ว!";

        int uid = LoginForm.id;
        string url = $"{apiUrl}/{uid}";
        Debug.Log("Fetching scores from: " + url);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + request.error);
                scoreText.text = "Error loading score";
                yield break;
            }

            string json = request.downloadHandler.text;
            Debug.Log("Raw JSON: " + json);

            ScoreRecord[] records;
            try
            {
                records = JsonHelper.FromJson<ScoreRecord>(json);
            }
            catch
            {
                Debug.LogError("Failed to parse JSON");
                scoreText.text = "Error parsing data";
                yield break;
            }

            // เก็บคะแนนรวมของแต่ละภาษา + เลเวล
            Dictionary<string, int> totalScores = new Dictionary<string, int>();

            foreach (var record in records)
            {
                string langKey = record.language == 1 ? "Java" : "Python";
                string diff = GetDifficulty(record.level, record.language); // easy/normal/hard
                string key = langKey + diff;

                if (!totalScores.ContainsKey(key))
                    totalScores[key] = 0;

                totalScores[key] += record.totalScore;
            }

            // แสดงคะแนนสำหรับ Language + Level ปัจจุบัน
            string currentKey = ChangeScenes.Language + ChangeScenes.Level;
            Debug.Log("Looking for key: " + currentKey);

            if (totalScores.ContainsKey(currentKey))
            {
                scoreText.text = $"{totalScores[currentKey]}";
            }
            else
            {
                scoreText.text = "ยังไม่มีคะแนนสำหรับด่านนี้";
            }
        }
    }

    // ฟังก์ชันกำหนด Difficulty ตาม level
    string GetDifficulty(int level, int language)
    {
        if (language == 1) // Java
        {
            if (level <= 10) return "Easy";
            else if (level <= 21) return "Normal";
            else return "Hard";
        }
        else // Python
        {
            if (level <= 9) return "Easy";
            else if (level <= 20) return "Normal";
            else return "Hard";
        }
    }

    // JsonHelper สำหรับแปลง JSON array
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{\"array\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
}
