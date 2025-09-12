using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System;

public class EditProfile : MonoBehaviour
{
    public GameObject btnEdit;
    public GameObject javaScore;
    public GameObject pythonScore;
    public GameObject btnCheck;
    public GameObject btnExit;
    public GameObject btnSave;
    public GameObject btnSent;
    public Button btnBack;
    public Button btnHistory;
    public GameObject textOtp;
    public GameObject textPassword;
    public GameObject textConfirmPassword;
    public GameObject EditPassword;
    public GameObject EditConfirmPassword;
    public GameObject correctUI;
    public GameObject wrongUI;
    public GameObject btnNextProfile;
    public GameObject btnPreviousProfile;
    public GameObject LoadingPanel;
    public Text Password;
    public Text ConfirmPassword;
    public InputField EmailInput;
    public InputField UserNameInput;
    public Text numberOTP;
    public static bool EditStatus = false;
    public static bool ExitStatus = false;
    public static bool OnclickEditBtn = true;
    public static bool OnclickExitBtn = false;
    private string originalEmail;
    private string oldUserName;
    private string oldEmail;
    private int saveStatus = 0; // 0 = ไม่ได้แก้ไข, 1 = otp ถูก, 2 = otp ผิด, 3 = กำลังแก้ไข
    int currentUserId = LoginForm.id;
    public AudioSource wrongAudio;

    public Image profileImage;           // รูป Profile ปัจจุบัน (pictureProfile)
    public Sprite[] profileSprites;      // เก็บ Sprite ทั้งหมด
    private int currentIndex = 0;        // เก็บ index ของรูปปัจจุบัน
    private const string ProfileKey = "SelectedProfileIndex";

    [Header("Notification UI")]
    public GameObject notificationPanel; // GameObject มี Text ด้านใน
    public Text notificationText;        // Text ด้านในของ Panel
    public float notificationDuration = 2f; // เวลาแสดงข้อความ

    private void Start()
    {
        saveStatus = 0; // รีเซ็ตสถานะการบันทึก
        LoadingPanel.gameObject.SetActive(true);
        // โหลด index จาก PlayerPrefs ถ้ามี
        if (PlayerPrefs.HasKey(ProfileKey))
        {
            currentIndex = PlayerPrefs.GetInt(ProfileKey, 0);
            if (currentIndex < profileSprites.Length)
                profileImage.sprite = profileSprites[currentIndex];
        }
        else if (profileSprites.Length > 0)
        {
            profileImage.sprite = profileSprites[0];
        }

        Debug.Log("✅ Login success, uid: " + currentUserId);
        StartCoroutine(LoadUserData());

        btnNextProfile.gameObject.SetActive(false);
        btnPreviousProfile.gameObject.SetActive(false);
        btnExit.gameObject.SetActive(false);
        btnCheck.gameObject.SetActive(false);
        btnSave.gameObject.SetActive(false);
        EditPassword.gameObject.SetActive(false);
        EditConfirmPassword.gameObject.SetActive(false);
        textPassword.gameObject.SetActive(false);
        textConfirmPassword.gameObject.SetActive(false);
        correctUI.gameObject.SetActive(false);
        wrongUI.gameObject.SetActive(false);
        textOtp.gameObject.SetActive(false);

        UserNameInput.readOnly = true;
        EmailInput.readOnly = true;

        originalEmail = EmailInput.text;
        EmailInput.onValueChanged.AddListener(OnEmailChanged);
        btnSent.gameObject.SetActive(false);
        LoadingPanel.gameObject.SetActive(false);
        notificationPanel.SetActive(false); // ซ่อน Panel ตอนเริ่ม
    }

    private void ShowNotification(string message)
    {
        if (notificationPanel == null || notificationText == null) return;

        notificationText.text = message;
        notificationPanel.SetActive(true);

        StopCoroutine("HideNotificationCoroutine");
        StartCoroutine(HideNotificationCoroutine());
    }

    private IEnumerator HideNotificationCoroutine()
    {
        yield return new WaitForSeconds(notificationDuration);
        notificationPanel.SetActive(false);
    }


    // เรียกเมื่อต้องการเปลี่ยนไปภาพถัดไป
    public void NextProfile()
    {
        if (profileSprites.Length == 0) return;

        currentIndex++;
        if (currentIndex >= profileSprites.Length)
            currentIndex = 0;

        profileImage.sprite = profileSprites[currentIndex];
    }

    // เรียกเมื่อต้องการเปลี่ยนกลับภาพก่อนหน้า
    public void PreviousProfile()
    {
        if (profileSprites.Length == 0) return;

        currentIndex--;
        if (currentIndex < 0)
            currentIndex = profileSprites.Length - 1;

        profileImage.sprite = profileSprites[currentIndex];
    }

    private void SaveProfile()
    {
        PlayerPrefs.SetInt(ProfileKey, currentIndex);
        PlayerPrefs.Save();
    }

    public void OnEmailChanged(string newValue)
    {
        // ต้องอยู่ในโหมด Edit (หลังจากกด btnEdit เท่านั้น)
        if (!OnclickExitBtn) return;

        // ✅ ตัดช่องว่างออกก่อน
        string trimmedEmail = newValue.Trim();

        // ✅ ถ้าว่าง -> ไม่แสดงปุ่ม
        if (string.IsNullOrEmpty(trimmedEmail))
        {
            btnSent.SetActive(false);
            saveStatus = 0;
            Debug.Log("Email is empty, btnSent disabled.");
            return;
        }

        // ✅ แสดงปุ่มเมื่ออีเมลเปลี่ยนจากค่าเดิม
        btnSent.SetActive(trimmedEmail != originalEmail);

        if (trimmedEmail != originalEmail)
        {
            saveStatus = 3; // รีเซ็ตสถานะการบันทึกเมื่ออีเมลเปลี่ยน
            Debug.Log("Email changed, saveStatus set to 3.");
        }
        else
        {
            saveStatus = 0; // รีเซ็ตสถานะการบันทึกเมื่ออีเมลกลับเป็นค่าเดิม
            Debug.Log("Email reverted to original, saveStatus set to 0.");
        }
    }


    public void OnClickBtnSent()
    {
        string emailToCheck = EmailInput.text.Trim();

        if (string.IsNullOrEmpty(emailToCheck))
        {
            ShowNotification("กรุณากรอกอีเมลก่อนส่ง OTP");
            return;
        }

        LoadingPanel.SetActive(true);
        StartCoroutine(CheckEmailBeforeOTP(emailToCheck));
    }

    IEnumerator CheckEmailBeforeOTP(string email)
    {
        string url = "https://codingforlearning.onrender.com/user/check-email";
        var jsonBody = JsonUtility.ToJson(new EmailCheckRequest(currentUserId, email));

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        LoadingPanel.SetActive(false);

        if (request.result == UnityWebRequest.Result.Success)
        {
            // ถ้า Success → email ยังไม่ซ้ำ → ส่ง OTP ต่อ
            SendOTP();
            btnSent.gameObject.SetActive(false);
        }
        else
        {
            // ถ้า error → เช็ค status จาก API
            string responseText = request.downloadHandler.text;
            try
            {
                var resp = JsonUtility.FromJson<StatusResponse>(responseText);

                if (resp.status == "EmailExists")
                {
                    ShowNotification("❌ อีเมลนี้ถูกใช้งานแล้ว");
                    wrongAudio.Play();
                }
                else
                {
                    ShowNotification("❌ เกิดข้อผิดพลาด: " + resp.message);
                }
            }
            catch
            {
                ShowNotification("❌ เกิดข้อผิดพลาดไม่ทราบสาเหตุ");
            }
        }
    }
    [System.Serializable]
    public class EmailCheckRequest
    {
        public int uid;
        public string email;
        public EmailCheckRequest(int uid, string email)
        {
            this.uid = uid;
            this.email = email;
        }
    }


    // ====== SEND OTP ======
    public void SendOTP()
    {
        LoadingPanel.gameObject.SetActive(true);
        StartCoroutine(SendOTPCoroutine(EmailInput.text));
    }

    IEnumerator SendOTPCoroutine(string email)
    {
        string url = "https://codingforlearning.onrender.com/otp/send-otp";
        var jsonBody = JsonUtility.ToJson(new EmailOnly(email));

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        LoadingPanel.gameObject.SetActive(false);

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("📩 OTP ส่งสำเร็จ");
            ShowNotification("📩 OTP ส่งสำเร็จ");

            // ✅ แสดงปุ่มตรวจสอบ OTP และช่อง OTP **เฉพาะเมื่อส่งสำเร็จ**
            btnCheck.SetActive(true);
            textOtp.SetActive(true);

            // รีเซ็ตเลขของ OTP
            numberOTP.text = string.Empty;
        }
        else
        {
            Debug.Log("❌ ส่ง OTP ไม่สำเร็จ: " + request.downloadHandler.text);
            ShowNotification("❌ OTP ส่งไม่สำเร็จ");

            // ❌ ไม่แสดง btnCheck และ textOtp
            btnCheck.SetActive(false);
            textOtp.SetActive(false);
        }
    }

    // ====== VERIFY OTP ======
    public void VerifyOTP()
    {
        LoadingPanel.gameObject.SetActive(true);
        StartCoroutine(VerifyOTPCoroutine(EmailInput.text, numberOTP.text));
    }

    IEnumerator VerifyOTPCoroutine(string email, string otp)
    {
        string url = "https://codingforlearning.onrender.com/otp/verify-otp";
        var jsonBody = JsonUtility.ToJson(new OTPVerify(email, otp));

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            ShowNotification("✅ ยืนยัน OTP สำเร็จ");
            Debug.Log("✅ ยืนยัน OTP สำเร็จ");
            correctUI.SetActive(true);
            wrongUI.SetActive(false);
            saveStatus = 1; // OTP ถูกต้อง
            btnCheck.SetActive(false); // ซ่อนปุ่มตรวจสอบ OTP
            LoadingPanel.gameObject.SetActive(false);
            Debug.Log("OTP correct : saveStatus set to 1.");
        }
        else
        {
            ShowNotification("❌ OTP ไม่ถูกต้องหรือหมดอายุ: ");
            Debug.Log("❌ OTP ไม่ถูกต้องหรือหมดอายุ: " + request.downloadHandler.text);
            correctUI.SetActive(false);
            wrongUI.SetActive(true);
            wrongAudio.Play(); // เล่นเสียงผิดพลาด
            saveStatus = 2; // OTP ผิด
            LoadingPanel.gameObject.SetActive(false);
            Debug.Log("OTP incorrect : saveStatus set to 2.");
        }
    }

    [System.Serializable]
    public class EmailOnly
    {
        public string email;
        public EmailOnly(string email) { this.email = email; }
    }

    [System.Serializable]
    public class OTPVerify
    {
        public string email;
        public string otp;
        public OTPVerify(string email, string otp)
        {
            this.email = email;
            this.otp = otp;
        }
    }

    public void OpenEditForm()
    {
        if (!EditStatus)
        {
            if (OnclickEditBtn && !OnclickExitBtn)
            {
                // เปิดให้แก้ไขได้
                UserNameInput.readOnly = false;
                EmailInput.readOnly = false;
                // เก็บข้อมูลเดิมก่อนที่จะแก้ไข
                oldUserName = UserNameInput.text;
                oldEmail = EmailInput.text;
                // --- เปิดฟอร์ม Edit ---

                btnEdit.SetActive(false);
                javaScore.SetActive(false);
                pythonScore.SetActive(false);
                btnCheck.SetActive(false);

                correctUI.gameObject.SetActive(false);
                wrongUI.gameObject.SetActive(false);

                btnNextProfile.SetActive(true);
                btnPreviousProfile.SetActive(true);
                btnExit.SetActive(true);
                btnSave.SetActive(true);
                EditPassword.SetActive(true);
                EditConfirmPassword.SetActive(true);
                textPassword.SetActive(true);
                textConfirmPassword.SetActive(true);

                originalEmail = EmailInput.text; // ✅ บันทึกค่าเดิมตอนเข้า Edit

                // 🔒 ปิดการกดปุ่ม back และ history
                btnBack.GetComponent<Button>().interactable = false;
                btnHistory.GetComponent<Button>().interactable = false;

                OnclickEditBtn = false;
                OnclickExitBtn = true;
                saveStatus = 0;
                Debug.Log("Email reverted to original, saveStatus set to 0.");
            }
            else
            {
                // ปิดไม่ให้แก้ไข
                UserNameInput.readOnly = true;
                EmailInput.readOnly = true;
                // รีเซ็ตค่าข้อมูลกลับไปเป็นข้อมูลเดิม
                UserNameInput.text = oldUserName;
                EmailInput.text = oldEmail;
                // --- ปิดฟอร์ม Edit ---
                btnEdit.SetActive(true);
                javaScore.SetActive(true);
                pythonScore.SetActive(true);

                correctUI.gameObject.SetActive(false);
                wrongUI.gameObject.SetActive(false);

                btnNextProfile.SetActive(false);
                btnPreviousProfile.SetActive(false);
                btnExit.SetActive(false);
                btnCheck.SetActive(false);
                btnSave.SetActive(false);
                EditPassword.SetActive(false);
                EditConfirmPassword.SetActive(false);
                textPassword.SetActive(false);
                textConfirmPassword.SetActive(false);
                btnSent.SetActive(false);
                textOtp.SetActive(false);

                // ✅ เปิดปุ่มกลับมาใช้งานได้อีกครั้ง
                btnBack.GetComponent<Button>().interactable = true;
                btnHistory.GetComponent<Button>().interactable = true;

                OnclickEditBtn = true;
                OnclickExitBtn = false;
            }
        }
    }

    public void SaveBtnClick()
    {
        if (saveStatus < 2)
        {
            string newName = UserNameInput.text.Trim();
            string newEmail = EmailInput.text.Trim();
            string newPassword = EditPassword.activeSelf ? Password.text : "";
            string confirmPassword = EditConfirmPassword.activeSelf ? ConfirmPassword.text : "";

            // ✅ ตรวจสอบค่าว่าง
            if (string.IsNullOrEmpty(newName))
            {
                ShowNotification("กรุณากรอกชื่อ");
                return;
            }
            if (newName.Contains(" "))
            {
                ShowNotification("ชื่อผู้ใช้ห้ามมีช่องว่าง");
                return;
            }

            if (string.IsNullOrEmpty(newEmail))
            {
                ShowNotification("กรุณากรอกอีเมล");
                return;
            }

            // ✅ ตรวจสอบรูปแบบอีเมล
            if (!IsValidEmail(newEmail) || newEmail.Contains(" ") || newEmail.Contains("..") || newEmail.StartsWith("@") || newEmail.EndsWith("@") || newEmail.EndsWith(".") || !newEmail.Contains("."))
            {
                ShowNotification("รูปแบบอีเมลไม่ถูกต้อง");
                return;
            }

            // ✅ ตรวจสอบรหัสผ่าน
            if (!string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(confirmPassword))
            {
                if (newPassword.Length < 4)
                {
                    ShowNotification("รหัสผ่านต้องมีความยาวอย่างน้อย 4 ตัวอักษร");
                    return;
                }
                if (newPassword.Contains(" "))
                {
                    ShowNotification("รหัสผ่านห้ามมีช่องว่าง");
                    return;
                }
                if (newPassword != confirmPassword)
                {
                    ShowNotification("รหัสผ่านไม่ตรงกัน");
                    return;
                }
            }

            SaveProfile();
            LoadingPanel.gameObject.SetActive(true);
            StartCoroutine(EditCoroutine(currentUserId, newName, newEmail, newPassword, confirmPassword));

            btnBack.GetComponent<Button>().interactable = true;
            btnHistory.GetComponent<Button>().interactable = true;
        }
        else
        {
            ShowNotification("ไม่สามารถบันทึกได้");
        }
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    IEnumerator EditCoroutine(int uid, string name, string newEmail, string password, string confirmPassword)
    {
        string url = "https://codingforlearning.onrender.com/user/edit";
        var jsonBody = JsonUtility.ToJson(new EditRequest(uid, name, newEmail, password, confirmPassword));

        var request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        LoadingPanel.SetActive(false);

        string responseText = request.downloadHandler.text;
        StatusResponse response = null;

        try
        {
            response = JsonUtility.FromJson<StatusResponse>(responseText);
        }
        catch
        {
            response = new StatusResponse { status = "Unknown", message = responseText };
        }

        if (request.result == UnityWebRequest.Result.Success && response.status == "Success")
        {
            ShowNotification("✅ แก้ไขข้อมูลสำเร็จ");
            // ปิด UI edit และ reset state เหมือนเดิม
            UserNameInput.readOnly = true;
            EmailInput.readOnly = true;
            btnEdit.SetActive(true);
            javaScore.SetActive(true);
            pythonScore.SetActive(true);
            btnNextProfile.SetActive(false);
            btnPreviousProfile.SetActive(false);
            btnExit.SetActive(false);
            btnCheck.SetActive(false);
            btnSave.SetActive(false);
            EditPassword.SetActive(false);
            EditConfirmPassword.SetActive(false);
            textPassword.SetActive(false);
            textConfirmPassword.SetActive(false);
            textOtp.SetActive(false);
            correctUI.SetActive(false);
            wrongUI.SetActive(false);
            OnclickEditBtn = true;
            OnclickExitBtn = false;
        }
        else
        {
            switch (response.status)
            {
                case "EmailExists":
                    ShowNotification("❌ อีเมลนี้ถูกใช้งานแล้ว");
                    break;
                case "PasswordMismatch":
                    ShowNotification("❌ รหัสผ่านไม่ตรงกัน");
                    break;
                case "UserNotFound":
                    ShowNotification("❌ ไม่พบผู้ใช้");
                    break;
                case "ServerError":
                    ShowNotification("❌ เกิดข้อผิดพลาดจากเซิร์ฟเวอร์");
                    break;
                default:
                    ShowNotification("❌ แก้ไขข้อมูลไม่สำเร็จ: " + response.message);
                    break;
            }
            wrongAudio.Play();
        }
    }

    [System.Serializable]
    public class StatusResponse
    {
        public string status;  // เช่น "Success", "EmailExists", "PasswordMismatch"
        public string message; // ข้อความจาก API
    }



    [System.Serializable]
    public class EditRequest
    {
        public int uid;
        public string name;
        public string newEmail;
        public string password;
        public string confirmPassword;

        public EditRequest(int uid, string name, string newEmail, string password, string confirmPassword)
        {
            this.uid = uid;
            this.name = name;
            this.newEmail = newEmail;
            this.password = password;
            this.confirmPassword = confirmPassword;
        }
    }

    IEnumerator LoadUserData()
    {
        string url = $"https://codingforlearning.onrender.com/user/{currentUserId}";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                UserData user = JsonUtility.FromJson<UserData>(req.downloadHandler.text);
                UserNameInput.text = user.name;
                EmailInput.text = user.email;
            }
            else
            {
                Debug.LogError("❌ Load failed: " + req.error);
            }
        }
    }

    [System.Serializable]
    public class UserData
    {
        public string name;
        public string email;
    }

}
