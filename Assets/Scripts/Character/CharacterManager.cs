using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDatabase;
    public GameObject btnSelect; // ตัวละครที่มี Animator
    private GameObject currentCharacterInstance;
    private int selectedOption = 0;
    public Text unlockMessageText; 

    public int latestLevel = 0; // ปลดล็อคตัวละครตามเลเวล

    void Start()
    {
        int uid = LoginForm.id;
        int language = 0;    // 0 = Python // 1 = Java
        if (ChangeScenes.Language == "Python")
        {
            language = 0;
        }
        else if (ChangeScenes.Language == "Java")
        {
            language = 1;
        }
        Debug.Log("UID: " + uid);
        Debug.Log("Language: " + language);
        Debug.Log("Latest Level: " + latestLevel);

        if (!PlayerPrefs.HasKey("selectOption"))
        {
            selectedOption = 0; // ถ้าไม่มีการบันทึก ให้เริ่มที่ตัวเลือกแรก
        }
        else
        {
            Load(); // โหลดตัวเลือกที่บันทึกไว้
        }

        StartCoroutine(LoadLatestLevel(uid, language));
    }

    public void btnRight()
    {
        selectedOption++;
        if (selectedOption >= characterDatabase.CharacterCount)
            selectedOption = 0;

        UpdateCharacter(selectedOption);
    }


    public void btnLeft()
    {
        selectedOption--;
        if (selectedOption < 0)
            selectedOption = characterDatabase.CharacterCount - 1;

        UpdateCharacter(selectedOption);
    }
    public void btnSelectCharacter()
    {
        Debug.Log("Selected character index: " + selectedOption);
        Save(); // บันทึกตัวเลือกปัจจุบัน
        if (ChangeScenes.sceneStack.Count > 1) // เช็คว่ามีซีนก่อนหน้าใน Stack
        {
            // นำซีนปัจจุบันออกจาก Stack
            ChangeScenes.sceneStack.Pop();

            // โหลดซีนก่อนหน้า
            string previousScene = ChangeScenes.sceneStack.Peek();
            Debug.Log("Back to: " + previousScene);
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.LogWarning("No more scenes to go back to.");
        }
    }


    private void UpdateCharacter(int selectedOption)
    {
        Debug.Log("Updating character index: " + selectedOption);

        if (currentCharacterInstance != null)
        {
            Destroy(currentCharacterInstance);
            Debug.Log("Destroyed old character");
        }

        Character character = characterDatabase.GetCharacter(selectedOption);
        if (character != null && character.characterPrefab != null)
        {
            currentCharacterInstance = Instantiate(
                character.characterPrefab,
                Vector3.zero,
                Quaternion.identity,
                transform
            );

            currentCharacterInstance.transform.localPosition = Vector3.zero;

            Debug.Log("Instantiated new character: " + character.characterPrefab.name);

            // คำนวณตัวละครที่ปลดล็อคได้ (ทุก 3 level จะปลด 1 ตัวใหม่)
            int unlockedCharacterCount = Mathf.Clamp(1 + (latestLevel - 1) / 3, 1, characterDatabase.CharacterCount);

            bool isUnlocked = selectedOption < unlockedCharacterCount;

            if (currentCharacterInstance.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                renderer.color = isUnlocked ? Color.white : new Color(1f, 1f, 1f, 0.8f);
                btnSelect.GetComponent<Button>().interactable = isUnlocked;
            }

            // ✅ แสดงข้อความปลดล็อค
            if (!isUnlocked)
            {
                int requiredLevel = (selectedOption * 3) + 1;

                unlockMessageText.gameObject.SetActive(true);
                unlockMessageText.text = $"Unlock after Stage {requiredLevel - 1}";
            }
            else
            {
                unlockMessageText.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("Character prefab is null or character not found");
        }
    }


    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectOption");
    }

    private void Save()
    {
        PlayerPrefs.SetInt("selectOption", selectedOption);
    }

    IEnumerator LoadLatestLevel(int uid, int language)
    {
        string url = $"https://codingforlearning.onrender.com/gameplay/latest-level/{uid}/{language}";
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("API Error: " + www.error);
            UpdateCharacter(selectedOption); // fallback ถ้าโหลดไม่ได้
        }
        else
        {
            string json = www.downloadHandler.text;
            Debug.Log("JSON Response: " + json);
            LatestLevelResponse data = JsonUtility.FromJson<LatestLevelResponse>(json);
            latestLevel = data.latestLevel;
            UpdateCharacter(selectedOption); // ✅ โหลดตัวใหม่หลังได้ level
        }
    }

    [System.Serializable]
    public class LatestLevelResponse
    {
        public int latestLevel;
    }

}
