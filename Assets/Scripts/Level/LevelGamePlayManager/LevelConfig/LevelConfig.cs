using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Level/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int level;
    public int mission;
    public int startScore = 100;
    public int minScore = 10;
    public int scorePenalty = 15;
    public string correctAnswer = "Hi";

    [Header("โครงสร้างโค้ดที่ต้องตรวจ (ถ้าไม่ใช้ปล่อยว่าง)")]
    public CodeCheck[] codeChecks;  // 👈 เพิ่มมาที่นี่เลย
}

[System.Serializable]
public class CodeCheck
{
    public string checkName;
    public string pattern;
    public string failMessage;
}
