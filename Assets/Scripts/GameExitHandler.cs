using UnityEngine;
using UnityEngine.Networking;

public class GameExitHandler : MonoBehaviour
{
    private static GameExitHandler instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ทำให้ GameObject อยู่ข้าม Scene
        }
        else
        {
            Destroy(gameObject); // ถ้ามีตัวซ้ำ ให้ลบ
        }
    }

    void OnApplicationQuit()
    {
        if (LoginForm.LoginStatus)
        {
            Debug.Log("Logging out before exit...");
            ForceLogout(LoginForm.id);
        }
    }

    private void ForceLogout(int uid)
    {
        string url = "https://codingforlearning.onrender.com/user/logout";
        var jsonBody = JsonUtility.ToJson(new LoginForm.LogoutRequest(uid));

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // รอจนกว่าจะเสร็จ (blocking)
            var operation = request.SendWebRequest();
            while (!operation.isDone) { }

            if (request.result == UnityWebRequest.Result.Success)
                Debug.Log("✅ Logout API success");
            else
                Debug.LogError("❌ Logout API failed: " + request.error);
        }
    }

}
