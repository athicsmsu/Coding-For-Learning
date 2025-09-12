using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class TotalPoints
{
    public int java_easy;
    public int java_normal;
    public int java_hard;
    public int python_easy;
    public int python_normal;
    public int python_hard;
}

[System.Serializable]
public class TotalPointsResponse
{
    public string message;
    public TotalPoints totals;
}
public class ScoreManager : MonoBehaviour
{
    private int uid; // กำหนดจาก LoginForm.id
    public Text javaEasyText;
    public Text javaNormalText;
    public Text javaHardText;
    public Text pythonEasyText;
    public Text pythonNormalText;
    public Text pythonHardText;

    void Start()
    {
        uid = LoginForm.id;
        StartCoroutine(GetTotalPoints());
    }

    private IEnumerator GetTotalPoints()
    {
        string url = $"https://codingforlearning.onrender.com/question/questionnaire/total/{uid}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching total points: " + request.error);
            }
            else
            {
                TotalPointsResponse response = JsonUtility.FromJson<TotalPointsResponse>(request.downloadHandler.text);
                UpdateUI(response.totals);
            }
        }
    }

    private void UpdateUI(TotalPoints totals)
    {
        javaEasyText.text = totals.java_easy.ToString()+"/100";
        javaNormalText.text = totals.java_normal.ToString()+"/100";
        javaHardText.text = totals.java_hard.ToString()+"/100";
        pythonEasyText.text = totals.python_easy.ToString()+"/100";
        pythonNormalText.text = totals.python_normal.ToString()+"/100";
        pythonHardText.text = totals.python_hard.ToString()+"/100";
    }
}
