using UnityEngine;

public class Loading : MonoBehaviour
{
    public static Loading Instance;

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
            Debug.LogError("Loading screen GameObject is not assigned.");
            return;
        }
        else
        {
            HideLoadingScreen();
        }
        Debug.Log("Loading...");
    }

    public static void ShowLoadingScreen()
    {
        if (Instance != null && Instance.loadingScreen != null)
        {
            Instance.loadingScreen.SetActive(true);
            Instance.isLoading = true;
        }
    }

    public static void HideLoadingScreen()
    {
        if (Instance != null && Instance.loadingScreen != null)
        {
            Instance.loadingScreen.SetActive(false);
            Instance.isLoading = false;
        }
    }
}
