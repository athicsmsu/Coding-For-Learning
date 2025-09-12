using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HistoryManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject rowPrefab;      // Prefab UI row ที่เตรียมไว้
    public Transform contentParent;   // Parent สำหรับวาง row

    [Header("Sprites")]
    public Sprite pythonSprite;
    public Sprite javaSprite;
    public Sprite easySprite;
    public Sprite normalSprite;
    public Sprite hardSprite;
    int order = 1;

    public GameObject imgNoInformation;
    public GameObject LoadingPanel;

    private void Start()
    {
        LoadingPanel.gameObject.SetActive(true);
        StartCoroutine(GetHistoryData());
    }

    IEnumerator GetHistoryData()
    {
        string url = $"https://codingforlearning.onrender.com/history/list/{LoginForm.id}"; // URL จริง

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                LoadingPanel.gameObject.SetActive(false);
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                HistoryListData historyList = JsonUtility.FromJson<HistoryListData>("{\"histories\":" + json + "}");

                // ✅ ถ้าไม่มีข้อมูลเลย
                if (historyList.histories == null || historyList.histories.Count == 0)
                {
                    LoadingPanel.gameObject.SetActive(false);
                    imgNoInformation.SetActive(true);
                    Debug.Log("ไม่พบข้อมูลใน database");
                }
                else
                {
                    imgNoInformation.SetActive(false); // ซ่อนถ้ามีข้อมูล
                    Debug.Log("พบข้อมูลใน database");

                    foreach (HistoryData data in historyList.histories)
                    {
                        CreateRow(data);
                        order++;
                    }
                    LoadingPanel.gameObject.SetActive(false);
                }
            }
        }
    }


    void CreateRow(HistoryData data)
    {
        GameObject newRow = Instantiate(rowPrefab, contentParent);

        // ดึง component HistoryRow
        HistoryRow row = newRow.GetComponent<HistoryRow>();

        // Set ค่าต่าง ๆ
        row.orderText.text = order.ToString();

        if (data.language == 0)
        {
            row.languageImg.sprite = pythonSprite;
        }
        else if (data.language == 1)
        {
            row.languageImg.sprite = javaSprite;
        }

        if (data.level <= 11)
        {
            row.levelImg.sprite = easySprite;
        }
        else if (data.level > 11 && data.level <= 22)
        {
            row.levelImg.sprite = normalSprite;
        }
        else if (data.level > 22)
        {
            row.levelImg.sprite = hardSprite;
        }

        // ✅ ตั้งชื่อด่าน แยกตามภาษา
        string stageName = "";

        if (data.language == 0) // Python
        {
            switch (data.level)
            {
                case 1:
                    stageName = "GET START";
                    break;
                case 2:
                    stageName = "TEXT output";
                    break;
                case 3:
                    stageName = "Number output";
                    break;
                case 4:
                    stageName = "Comments";
                    break;
                case 5:
                    stageName = "Declare Variable";
                    break;
                case 6:
                    stageName = "Print Variables";
                    break;
                case 7:
                    stageName = "Print Variables";
                    break;
                case 8:
                    stageName = "Type Casting";
                    break;
                case 9:
                    stageName = "Operators";
                    break;
                case 10:
                    stageName = "IF AND ELSE";
                    break;
                case 11:
                    stageName = "IF ELSE AND ELSE IF";
                    break;
                case 12:
                    stageName = "While";
                    break;
                case 13:
                    stageName = "For";
                    break;
                case 14:
                    stageName = "Switch";
                    break;
                case 15:
                    stageName = "Break";
                    break;
                case 16:
                    stageName = "Continue";
                    break;
                case 17:
                    stageName = "Arrays";
                    break;
                case 18:
                    stageName = "Methods";
                    break;
                case 19:
                    stageName = "Method Parameters";
                    break;
                case 20:
                    stageName = "Return";
                    break;
                case 21:
                    stageName = "Classes/Objects";
                    break;
                case 22:
                    stageName = "Class Attributes";
                    break;
                case 23:
                    stageName = "Constructors";
                    break;
                case 24:
                    stageName = "Encapsulation";
                    break;
                case 25:
                    stageName = "Inheritance";
                    break;
                case 26:
                    stageName = "Polymorphism";
                    break;
                default:
                    stageName = "Python Level " + data.level;
                    break;
            }
        }
        else if (data.language == 1) // Java
        {
            switch (data.level)
            {
                case 1:
                    stageName = "GET START";
                    break;
                case 2:
                    stageName = "TEXT output";
                    break;
                case 3:
                    stageName = "Number output";
                    break;
                case 4:
                    stageName = "Comments";
                    break;
                case 5:
                    stageName = "Declare Variable";
                    break;
                case 6:
                    stageName = "Data Type Variables";
                    break;
                case 7:
                    stageName = "Print Variables";
                    break;
                case 8:
                    stageName = "Print Variables";
                    break;
                case 9:
                    stageName = "Type Casting";
                    break;
                case 10:
                    stageName = "Operators";
                    break;
                case 11:
                    stageName = "IF AND ELSE";
                    break;
                case 12:
                    stageName = "IF ELSE AND ELSE IF";
                    break;
                case 13:
                    stageName = "While";
                    break;
                case 14:
                    stageName = "For";
                    break;
                case 15:
                    stageName = "Switch";
                    break;
                case 16:
                    stageName = "Break";
                    break;
                case 17:
                    stageName = "Continue";
                    break;
                case 18:
                    stageName = "Arrays";
                    break;
                case 19:
                    stageName = "Methods";
                    break;
                case 20:
                    stageName = "Method Parameters";
                    break;
                case 21:
                    stageName = "Return";
                    break;
                case 22:
                    stageName = "Classes/Objects";
                    break;
                case 23:
                    stageName = "Class Attributes";
                    break;
                case 24:
                    stageName = "Constructors";
                    break;
                case 25:
                    stageName = "Encapsulation";
                    break;
                case 26:
                    stageName = "Inheritance";
                    break;
                case 27:
                    stageName = "Polymorphism";
                    break;
                default:
                    stageName = "Java Level " + data.level;
                    break;
            }
        }

        row.stageText.text = stageName;

        if (data.mission == 1)
        {
            row.missionText.text = "Select the answer";
        }
        else if (data.mission == 2)
        {
            row.missionText.text = "Fill in the answer";
        }
        else if (data.mission == 3)
        {
            row.missionText.text = "Write the code";
        }

        row.scoreText.text = data.score.ToString();
        row.dateText.text = data.date;
    }

}

[System.Serializable]
public class HistoryListData
{
    public List<HistoryData> histories;
}

[System.Serializable]
public class HistoryData
{
    public int hid;
    public int pid;
    public string date;
    public int score;
    public int language;
    public int level;
    public int mission;
}
