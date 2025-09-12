using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviour
{
    public CharacterDatabase characterDatabase;
    private GameObject currentCharacterInstance;
    public GameObject CurrentCharacter => currentCharacterInstance;
    private int selectedOption = 0;

    public int latestLevel = 0; // ปลดล็อคตัวละครตามเลเวล

    void Start()
    {
        int uid = LoginForm.id;
        int language = 0; // 0 = Python, 1 = Java
        if (ChangeScenes.Language == "Python")
            language = 0;
        else if (ChangeScenes.Language == "Java")
            language = 1;

        Debug.Log("UID: " + uid);
        Debug.Log("Language: " + language);
        Debug.Log("Latest Level: " + latestLevel);

        if (!PlayerPrefs.HasKey("selectOption"))
            selectedOption = 0; // เริ่มตัวเลือกแรกถ้ายังไม่มีบันทึก
        else
            Load(); // โหลดตัวเลือกที่บันทึกไว้

        StartCoroutine(LoadLatestLevel(uid, language));
    }

    private void UpdateCharacter(int selectedOption)
    {
        Debug.Log("Updating character index: " + selectedOption);

        // ลบตัวละครเก่าออกถ้ามี
        if (currentCharacterInstance != null)
        {
            Destroy(currentCharacterInstance);
            Debug.Log("Destroyed old character");
        }

        Character character = characterDatabase.GetCharacter(selectedOption);
        if (character != null && character.characterPrefab != null)
        {
            // สร้างตัวละครใหม่
            currentCharacterInstance = Instantiate(
                character.characterPrefab,
                Vector3.zero,
                Quaternion.identity,
                transform
            );

            // ตั้งตำแหน่งตัวละครใหม่ให้อยู่ตรงกลาง parent
            currentCharacterInstance.transform.localPosition = Vector3.zero;

            Debug.Log("Instantiated new character: " + character.characterPrefab.name);

            //ตั้ง Animator Controller ให้ตรงกับตัวละคร
            Animator animator = currentCharacterInstance.GetComponent<Animator>();
            if (animator != null && character.animatorController != null)
            {
                animator.runtimeAnimatorController = character.animatorController;
                Debug.Log("Animator Controller ถูกตั้งเรียบร้อย");
            }
            else
            {
                Debug.LogWarning("Animator หรือ Animator Controller ไม่พบในตัวละคร");
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
            UpdateCharacter(selectedOption); // โหลดตัวละครใหม่หลังได้ level
        }
    }

    [System.Serializable]
    public class LatestLevelResponse
    {
        public int latestLevel;
    }
}
