using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class GradeFetcher : MonoBehaviour
{
    private string apiUrl = "https://codingforlearning.onrender.com/gameplay/raw-scores";

    [System.Serializable]
    public class GradeObjects
    {
        public GameObject gradeA;
        public GameObject gradeB;
        public GameObject gradeC;
        public GameObject gradeD;
    }

    public GradeObjects javaEasy;
    public GradeObjects javaNormal;
    public GradeObjects javaHard;
    public GradeObjects pythonEasy;
    public GradeObjects pythonNormal;
    public GradeObjects pythonHard;

    private Dictionary<string, int> standardStages = new Dictionary<string, int>()
    {
        { "java_easy", 10 },
        { "java_normal", 11 },
        { "java_hard", 6 },
        { "python_easy", 9 },
        { "python_normal", 10 },
        { "python_hard", 6 }
    };

    void Start()
    {
        StartCoroutine(GetRawScores());
    }

    IEnumerator GetRawScores()
    {
        string url = $"{apiUrl}/{LoginForm.id}";
        Debug.Log($"กำลังดึงเกรดสำหรับ UID: {LoginForm.id} จาก URL: {url}");

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("เกิดข้อผิดพลาดจาก API: " + www.error);
                yield break;
            }

            string json = www.downloadHandler.text;
            Debug.Log("ได้รับข้อมูล JSON: " + json);

            ScoreRecord[] records = JsonHelper.FromJson<ScoreRecord>(json);
            Debug.Log($"แปลงข้อมูลได้ {records.Length} เรคคอร์ดคะแนน");

            // เก็บคะแนนรวมของแต่ละ lang+diff
            Dictionary<string, int> totalScoresByCategory = new Dictionary<string, int>();
            // เก็บจำนวนเลเวลที่เล่นจริงในแต่ละ lang+diff
            Dictionary<string, int> stageCountByCategory = new Dictionary<string, int>();

            foreach (var record in records)
            {
                string langKey = record.language == 1 ? "java" : "python";
                string difficulty = GetDifficulty(record.level, record.language);
                string dictKey = $"{langKey}_{difficulty}";

                if (!totalScoresByCategory.ContainsKey(dictKey))
                    totalScoresByCategory[dictKey] = 0;
                if (!stageCountByCategory.ContainsKey(dictKey))
                    stageCountByCategory[dictKey] = 0;

                totalScoresByCategory[dictKey] += record.totalScore;
                stageCountByCategory[dictKey] += 1;  // นับจำนวนเลเวลที่มีข้อมูล
            }

            foreach (var kvp in totalScoresByCategory)
            {
                string dictKey = kvp.Key;
                int totalScore = kvp.Value;
                int stagesPlayed = stageCountByCategory.ContainsKey(dictKey) ? stageCountByCategory[dictKey] : 0;

                if (!standardStages.ContainsKey(dictKey))
                {
                    Debug.LogWarning($"ไม่พบข้อมูลจำนวนด่านมาตรฐานสำหรับ '{dictKey}'");
                    continue;
                }

                int stageMax = standardStages[dictKey];

                GradeObjects objSet = GetGradeObjects(dictKey.Split('_')[0], dictKey.Split('_')[1]);
                if (objSet == null)
                {
                    Debug.LogWarning($"GradeObjects สำหรับ '{dictKey}' ยังไม่ได้กำหนด");
                    continue;
                }

                if (stagesPlayed < stageMax)
                {
                    Debug.Log($"จำนวนด่านยังไม่ครบ ({stagesPlayed} < {stageMax}) - แสดงเกรดทั้งหมด");
                    SetAllGradesActive(objSet, true);
                }
                else
                {
                    float avgScore = totalScore / (float)stageMax;
                    string grade = CalculateGrade(avgScore);
                    Debug.Log($"เล่นครบแล้ว คะแนนรวม: {totalScore}, คะแนนเฉลี่ย: {avgScore:F2}, เกรดที่ได้: {grade}");
                    ShowGrade(objSet, grade);
                }
            }
        }
    }

    string GetDifficulty(int level, int language)
{
    if (language == 1) // Java
    {
        if (level <= 10) return "easy";
        else if (level <= 21) return "normal";
        else return "hard";
    }
    else // Python (ปรับตามจริง)
    {
        if (level <= 9) return "easy";
        else if (level <= 20) return "normal";
        else return "hard";
    }
}

    string CalculateGrade(float score)
    {
        if (score >= 90) return "A";
        else if (score >= 75) return "B";
        else if (score >= 60) return "C";
        else return "D";
    }

    GradeObjects GetGradeObjects(string lang, string diff)
    {
        if (lang == "java" && diff == "easy") return javaEasy;
        if (lang == "java" && diff == "normal") return javaNormal;
        if (lang == "java" && diff == "hard") return javaHard;
        if (lang == "python" && diff == "easy") return pythonEasy;
        if (lang == "python" && diff == "normal") return pythonNormal;
        if (lang == "python" && diff == "hard") return pythonHard;
        return null;
    }

    void SetAllGradesActive(GradeObjects objs, bool state)
    {
        objs.gradeA.SetActive(state);
        objs.gradeB.SetActive(state);
        objs.gradeC.SetActive(state);
        objs.gradeD.SetActive(state);
    }

    void ShowGrade(GradeObjects objs, string grade)
    {
        Debug.Log($"แสดงเกรด: {grade}");
        SetAllGradesActive(objs, false);

        switch (grade)
        {
            case "A": objs.gradeA.SetActive(true); break;
            case "B": objs.gradeB.SetActive(true); break;
            case "C": objs.gradeC.SetActive(true); break;
            case "D": objs.gradeD.SetActive(true); break;
            default:
                Debug.LogWarning($"เกรด '{grade}' ไม่ถูกต้องหรือไม่รู้จัก");
                break;
        }
    }

    [System.Serializable]
    public class ScoreRecord
    {
        public int language;
        public int level;
        public int totalScore;
    }

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
