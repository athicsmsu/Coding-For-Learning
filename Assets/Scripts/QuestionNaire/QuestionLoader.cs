using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class QuestionLoader : MonoBehaviour
{
    public static QuestionLoader Instance;

    public GameObject loadingScreen;
    public bool isLoading = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (loadingScreen == null)
        {
            Debug.LogError("❌ Question loading screen GameObject is not assigned.");
            return;
        }
        HideLoadingScreen();
    }

    public static void ShowLoadingScreen()
    {
        if (Instance != null && Instance.loadingScreen != null)
        {
            Instance.loadingScreen.SetActive(true);
            Instance.isLoading = true;
            Debug.Log("📥 กำลังโหลดคำถาม...");
        }
    }

    public static void HideLoadingScreen()
    {
        if (Instance != null && Instance.loadingScreen != null)
        {
            Instance.loadingScreen.SetActive(false);
            Instance.isLoading = false;
            Debug.Log("✅ โหลดคำถามเสร็จ");
        }
    }

    public IEnumerator LoadQuestionsFromAPI()
    {
        ShowLoadingScreen();

        using (UnityWebRequest www = UnityWebRequest.Get("https://codingforlearning.onrender.com/question"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("❌ โหลดคำถามไม่สำเร็จ: " + www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                Debug.Log("📄 คำถามที่ได้: " + json);

                // TODO: parse JSON แล้วแสดงคำถามใน UI
            }
        }

        HideLoadingScreen();
    }
}
