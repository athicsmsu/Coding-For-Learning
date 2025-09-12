using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class Level1 : MonoBehaviour, IPointerClickHandler
{
    public GameObject backButton;
    public GameObject newButton;
    public GameObject hintsButton;
    public GameObject bgAnimation;
    public GameObject homeButton;
    public GameObject runButton;
    public GameObject bgInput;
    public GameObject output;
    public GameObject NextLevelBG;

    public GameObject backBtnHint;
    public GameObject newBtnHint;
    public GameObject hintsBtnHint;
    public GameObject bgAnimationHint;
    public GameObject homeBtnHint;
    public GameObject runBtnHint;
    public GameObject bgInputHint;
    public GameObject outputHint;


    public Transform panelTransform;

    private List<GameObject> buttons;
    private int currentButtonIndex = 0;

    private Transform originalBackBtnParent;
    private Transform originalNewBtnParent;
    private Transform originalHintsBtnParent;
    private Transform originalBgAnimationParent;
    private Transform originalHomeBtnParent;
    private Transform originalRunBtnParent;
    private Transform originalBgInputParent;
    private Transform originalOutputParent;

    private Vector3 originalBackBtnPos;
    private Vector3 originalNewBtnPos;
    private Vector3 originalHintsBtnPos;
    private Vector3 originalBgAnimationPos;
    private Vector3 originalHomeBtnPos;
    private Vector3 originalRunBtnPos;
    private Vector3 originalBgInputPos;
    private Vector3 originalOutputPos;

    private Vector3 panelBackBtnPos;
    private Vector3 panelNewBtnPos;
    private Vector3 panelHintsBtnPos;
    private Vector3 panelBgAnimationPos;
    private Vector3 panelHomeBtnPos;
    private Vector3 panelRunBtnPos;
    private Vector3 panelBgInputPos;
    private Vector3 panelOutputPos;

    public Text outputText;
    private GameObject previousButton = null;
    public static int ScoreLevel1 = 100;
    public Text scoreText; // 👉 ลากจาก Inspector
    public TextMeshProUGUI TimeTxt;


    public Text AskTxt;
    public Animator animator;
    public Transform characterToMove;
    public PlayerController player;
    public AudioSource VoiceGood; // AudioSource สำหรับเล่นเสียงพูด
    public AudioSource VoiceBad; // AudioSource สำหรับเล่นเสียงพูด
    public AudioSource LoseSound;
    public AudioSource TimeCountdown;
    public AudioSource ClickSound; // เสียงคลิกปุ่ม
    public CountdownTimer countdownTimer;

    public GameObject animationCharacter; // ตัวละครที่จะแสดงแอนิเมชัน
    void Start()
    {
        ScoreLevel1 = 100; // เริ่มต้นคะแนนที่ 100
                           // Initialize the list of buttons
        buttons = new List<GameObject> { backButton, bgAnimation, output, homeButton, runButton, newButton, bgInput, hintsButton };

        // ปิด Hint ปุ่มทั้งหมดตอนเริ่มต้น
        backBtnHint?.SetActive(false);
        newBtnHint?.SetActive(false);
        bgAnimationHint?.SetActive(false);
        homeBtnHint?.SetActive(false);
        runBtnHint?.SetActive(false);
        bgInputHint?.SetActive(false);
        outputHint?.SetActive(false);
        hintsBtnHint?.SetActive(false);

        // Hide the NextLevelBG at the start
        NextLevelBG?.SetActive(false);

        // Store the original positions and parents for all buttons
        if (backButton != null)
        {
            originalBackBtnParent = backButton.transform.parent;
            originalBackBtnPos = backButton.transform.localPosition;
        }

        if (newButton != null)
        {
            originalNewBtnParent = newButton.transform.parent;
            originalNewBtnPos = newButton.transform.localPosition;
        }

        if (bgAnimation != null)
        {
            originalBgAnimationParent = bgAnimation.transform.parent;
            originalBgAnimationPos = bgAnimation.transform.localPosition;
        }

        if (homeButton != null)
        {
            originalHomeBtnParent = homeButton.transform.parent;
            originalHomeBtnPos = homeButton.transform.localPosition;
        }

        if (runButton != null)
        {
            originalRunBtnParent = runButton.transform.parent;
            originalRunBtnPos = runButton.transform.localPosition;
        }

        if (bgInput != null)
        {
            originalBgInputParent = bgInput.transform.parent;
            originalBgInputPos = bgInput.transform.localPosition;
        }

        if (output != null)
        {
            originalOutputParent = output.transform.parent;
            originalOutputPos = output.transform.localPosition;
        }

        if (hintsButton != null)
        {
            originalHintsBtnParent = hintsButton.transform.parent;
            originalHintsBtnPos = hintsButton.transform.localPosition;
        }

        // Set panel positions for all buttons
        panelBackBtnPos = backButton.transform.localPosition;
        panelNewBtnPos = newButton.transform.localPosition;
        panelBgAnimationPos = bgAnimation.transform.localPosition;
        panelHomeBtnPos = homeButton.transform.localPosition;
        panelRunBtnPos = runButton.transform.localPosition;
        panelBgInputPos = bgInput.transform.localPosition;
        panelOutputPos = output.transform.localPosition;
        panelHintsBtnPos = hintsButton.transform.localPosition;
        UpdateScoreUI();

        countdownTimer.onCountdownEnd.AddListener(OnTimeEnd);
        countdownTimer.onScorePenalty.AddListener(HandleScorePenalty);
        countdownTimer.StartCountdown(180); // เริ่มนับถอยหลัง
    }

    void OnTimeEnd()
    {
        ScoreLevel1 = 50;
        UpdateScoreUI();
    }

    void HandleScorePenalty(int elapsedTime)
    {
        if (ScoreLevel1 > 50)
        {
            ScoreLevel1 -= 0;
            if (ScoreLevel1 < 50) ScoreLevel1 = 50;
            UpdateScoreUI();
        }
    }

    public StarAnimator starAnimator;
    public void CheckOutput()
    {
        if(TimeCountdown.isPlaying) // ตรวจสอบว่าเสียงนับเวลายังเล่นอยู่หรือไม่
        {
            TimeCountdown.Stop(); // หยุดเสียงนับเวลาเมื่อผู้เล่นกดปุ่ม Check Output
        }
        if (outputText.text.Trim() == "Hi")
        {
            Debug.Log("Correct answer!");

            countdownTimer.StopCountdown();
            
            StartCoroutine(SendGameplayData());
            AskTxt.text = outputText.text;
            TimeTxt.color = Color.green; // เปลี่ยนสีแสดงเวลาค้างไว้
            VoiceGood.Play(); // เล่นเสียงพูด

            //ดีเลย์ 2 วินาทีก่อนเริ่มแอนิเมชัน
            StartCoroutine(ShowStarsWithDelay());
        }
        else
        {
            // Incorrect answer logic here
            Debug.Log($"Output text is: '{outputText.text}'");
            Debug.Log("Incorrect answer. Try again.");
            AskTxt.text = "Lose!";
            LoseSound.Play(); // เล่นเสียงพูด
            VoiceBad.Play(); // เล่นเสียงพูด
            UpdateScoreUI();
        }
    }
    private IEnumerator ShowStarsWithDelay()
    {
        yield return new WaitForSeconds(2f); // ดีเลย์ 2 วินาที
        animationCharacter?.SetActive(false); // ปิดตัวละครแอนิเมชัน
        NextLevelBG?.SetActive(true);
        if (!starAnimator.gameObject.activeInHierarchy)
        {
            starAnimator.gameObject.SetActive(true);
        }

        starAnimator.ShowStars(ScoreLevel1);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = ScoreLevel1.ToString();
        }
    }

    private IEnumerator RunAndMove()
    {
        Debug.Log("▶ เริ่มฟังก์ชัน RunAndMove");

        // if (characterToMove == null)
        // {
        //     Debug.LogError("❌ characterToMove ยังไม่ได้ตั้งค่า");
        //     yield break;
        // }
        // else
        // {
        //     Debug.Log("✅ characterToMove: " + characterToMove.name);
        // }

        animator = characterToMove.GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("❌ ไม่พบ Animator บน: " + characterToMove.name);
            yield break;
        }

        animator.SetBool("isRun", true);
        Debug.Log("🏃 เริ่มแสดงท่าวิ่ง");

        // Vector3 startPos = characterToMove.transform.position;
        // Vector3 targetPos = startPos + Vector3.right * moveDistance;
        // float elapsed = 0f;

        // while (elapsed < moveDuration)
        // {
        //     characterToMove.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
        //     elapsed += Time.deltaTime;
        //     yield return null;
        // }

        // characterToMove.transform.position = targetPos;

        // animator.SetBool("isRun", false);
        Debug.Log("🛑 หยุดแสดงท่าวิ่ง");
    }
    private IEnumerator SendGameplayData()
    {
        string url = "https://codingforlearning.onrender.com/gameplay/add-gameplay";
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
        int level = 1;
        int mission = 1;
        int score = ScoreLevel1;

        // 👇 สร้าง JSON payload
        string json = JsonUtility.ToJson(new GameplayData
        {
            uid = uid,
            language = language,
            level = level,
            mission = mission,
            score = score
        });

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Gameplay & History added successfully!");
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("uid: " + uid);
            Debug.Log("language: " + language);
            Debug.Log("level: " + level);
            Debug.Log("mission: " + mission);
            Debug.Log("score: " + score);
            Debug.LogError("Failed to send gameplay data: " + request.error);
        }
    }

    // 👇 คลาสสำหรับแปลงเป็น JSON
    [System.Serializable]
    public class GameplayData
    {
        public int uid;
        public int language;
        public int level;
        public int mission;
        public int score;
    }

    private void SetHintActive(GameObject button, bool active)
    {
        if (button == backButton) backBtnHint?.SetActive(active);
        else if (button == newButton) newBtnHint?.SetActive(active);
        else if (button == bgAnimation) bgAnimationHint?.SetActive(active);
        else if (button == homeButton) homeBtnHint?.SetActive(active);
        else if (button == runButton) runBtnHint?.SetActive(active);
        else if (button == bgInput) bgInputHint?.SetActive(active);
        else if (button == output) outputHint?.SetActive(active);
        else if (button == hintsButton) hintsBtnHint?.SetActive(active);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        // If all buttons are already in the panel, reset and start again
        if (currentButtonIndex >= buttons.Count)
        {
            // Move hint out of panel before destroying it
            if (hintsButton != null)
            {
                hintsButton.transform.SetParent(originalHintsBtnParent, true);
                hintsButton.transform.localPosition = originalHintsBtnPos;

                int panelIndexForHints = panelTransform.GetSiblingIndex();
                hintsButton.transform.SetSiblingIndex(panelIndexForHints - 1);

                hintsBtnHint?.SetActive(false);

            }
            if (panelTransform != null)
            {
                Destroy(panelTransform.gameObject);
            }

            // Reset button index
            currentButtonIndex = 0;

            // Remove all buttons from panel
            foreach (var button in buttons)
            {
                button.transform.SetParent(originalParent(button), true);
                button.transform.localPosition = GetOriginalPosition(button);

                // Move the button to the back of the panel (in the hierarchy)
                int panelIndexForButton = panelTransform.GetSiblingIndex();
                button.transform.SetSiblingIndex(panelIndexForButton - 1);
            }

            return;
        }
        else
        {
            ClickSound.Play(); // เล่นเสียงคลิกปุ่ม
        }

        GameObject currentButton = buttons[currentButtonIndex];

        // Move the previous button out of the panel if it exists
        if (previousButton != null)
        {
            // Move the previous button back to its original parent
            previousButton.transform.SetParent(originalParent(previousButton), true);
            previousButton.transform.localPosition = GetOriginalPosition(previousButton);

            // Move the previous button to the back of the panel (in the hierarchy)
            int panelIndex = panelTransform.GetSiblingIndex();
            previousButton.transform.SetSiblingIndex(panelIndex - 1); // Move to the back of the panel

            // ⬅️ ปิด Hint ของปุ่มก่อนหน้า
            SetHintActive(previousButton, false);
        }

        // Move the current button to the panel
        if (currentButton != null)
        {
            currentButton.transform.SetParent(panelTransform, true);

            // Determine the panel position based on the button
            if (currentButton == backButton)
                currentButton.transform.localPosition = panelBackBtnPos;
            else if (currentButton == newButton)
                currentButton.transform.localPosition = panelNewBtnPos;
            else if (currentButton == bgAnimation)
                currentButton.transform.localPosition = panelBgAnimationPos;
            else if (currentButton == homeButton)
                currentButton.transform.localPosition = panelHomeBtnPos;
            else if (currentButton == runButton)
                currentButton.transform.localPosition = panelRunBtnPos;
            else if (currentButton == bgInput)
                currentButton.transform.localPosition = panelBgInputPos;
            else if (currentButton == output)
                currentButton.transform.localPosition = panelOutputPos;
            else if (currentButton == hintsButton)
                currentButton.transform.localPosition = panelHintsBtnPos;
            // ⬅️ เปิด Hint ของปุ่มปัจจุบัน
            SetHintActive(currentButton, true);

            // Update the previousButton
            previousButton = currentButton;
        }

        // Move to the next button
        currentButtonIndex++;
    }

    // Helper function to get the original parent of a button
    private Transform originalParent(GameObject button)
    {
        if (button == backButton) return originalBackBtnParent;
        if (button == newButton) return originalNewBtnParent;
        if (button == bgAnimation) return originalBgAnimationParent;
        if (button == homeButton) return originalHomeBtnParent;
        if (button == runButton) return originalRunBtnParent;
        if (button == bgInput) return originalBgInputParent;
        if (button == output) return originalOutputParent;
        if (button == hintsButton) return originalHintsBtnParent;
        return null;
    }

    // Helper function to get the original position of a button
    private Vector3 GetOriginalPosition(GameObject button)
    {
        if (button == backButton) return originalBackBtnPos;
        if (button == newButton) return originalNewBtnPos;
        if (button == bgAnimation) return originalBgAnimationPos;
        if (button == homeButton) return originalHomeBtnPos;
        if (button == runButton) return originalRunBtnPos;
        if (button == bgInput) return originalBgInputPos;
        if (button == output) return originalOutputPos;
        if (button == hintsButton) return originalHintsBtnPos;
        return Vector3.zero;
    }
}
