using UnityEngine;
using UnityEngine.UI;

public class LoadingVersion : MonoBehaviour
{
    public GameObject loadingPanel;
    public Text loadingText;

    public static LoadingVersion Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static void ShowLoadingScreen(string message = "กำลังโหลด...")
    {
        if (Instance == null) return;
        if (Instance.loadingPanel != null) Instance.loadingPanel.SetActive(true);
        if (Instance.loadingText != null) Instance.loadingText.text = message;
    }

    public static void HideLoadingScreen()
    {
        if (Instance == null) return;
        if (Instance.loadingPanel != null) Instance.loadingPanel.SetActive(false);
    }
}
