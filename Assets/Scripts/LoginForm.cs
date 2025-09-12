using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class LoginForm : MonoBehaviour
{
    public GameObject loginForm;
    public GameObject RegisterForm;
    public GameObject ForgotForm;
    public GameObject VerifyOTPForm;
    public GameObject ResetPasswordForm;
    public GameObject PressToLogintxt;
    public GameObject PressToLogouttxt;
    public GameObject Profiletxt;
    public GameObject LogoutBtn;
    public GameObject LogoutForm;


    public static bool LoginStatus = false;
    public static bool OnclickLoginBtn = false;
    public static int id;

    public Text Email;
    public Text Password;
    public Text PasswordRegister;
    public Text EmailRegister;
    public Text UserNameRegister;
    public Text EmailForOTP;
    public Text OTP;
    public Text NewPassword;
    public Text ConfirmPassword;

    public AudioSource wrongAudio;
    [Header("Notification UI")]
    public GameObject notificationPanel; // GameObject มี Text ด้านใน
    public Text notificationText;        // Text ด้านในของ Panel
    public float notificationDuration = 2f; // เวลาแสดงข้อความ
    [Header("App Version")]
    public Text VersionText; // ข้อความแสดงเวอร์ชัน
    [Header("Update Version Panel")]
    public GameObject UpdateVersionPanel; // Panel แจ้งให้อัปเดตเวอร์ชัน
    public Text VersionUpdatText; // ข้อความแสดงเวอร์ชันที่ควรอัปเดต

    [Header("Credits")]
    public GameObject CreditsPanel; // Panel แสดงเครดิต

    private void Start()
    {
        OnclickLoginBtn = false;

        loginForm.SetActive(false);
        RegisterForm.SetActive(false);
        ForgotForm.SetActive(false);
        VerifyOTPForm.SetActive(false);
        ResetPasswordForm.SetActive(false);
        CreditsPanel.SetActive(false);

        PressToLogintxt.SetActive(!LoginStatus);
        Profiletxt.SetActive(LoginStatus);
        LogoutBtn.SetActive(LoginStatus); // แสดงปุ่ม Logout เมื่อ login แล้ว
        PressToLogouttxt.SetActive(LoginStatus);
        LogoutForm.SetActive(false);      // ซ่อน popup Logout ตอนเริ่มต้น
        notificationPanel.SetActive(false);
        UpdateVersionPanel.SetActive(false);
        // ✅ เริ่มเช็คเวอร์ชันแอพ
        LoadingVersion.ShowLoadingScreen("กำลังติดต่อกับเซิร์ฟเวอร์ โปรดรอสักครู่...");
        StartCoroutine(CheckVersionCoroutine());
    }
    
    IEnumerator CheckVersionCoroutine()
    {
        string url = "https://codingforlearning.onrender.com/app/version";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Loading.HideLoadingScreen();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            VersionResponse response = JsonUtility.FromJson<VersionResponse>(json);

            string currentVersion = VersionText.text; // ← กำหนดเวอร์ชันของแอพตรงนี้

            if (response.version != currentVersion)
            {
                ShowNotification("⚠️ กรุณาอัปเดตแอพเป็นเวอร์ชันล่าสุด (" + response.version + ")");
                Debug.Log("App version mismatch: Server=" + response.version + " Local=" + currentVersion);
                // ถ้าต้องการบังคับให้อัปเดตก็สามารถปิด UI ได้เลย
                LoadingVersion.HideLoadingScreen();
                UpdateVersionPanel.SetActive(true);
                VersionUpdatText.text = response.version;
            }
            else
            {
                LoadingVersion.HideLoadingScreen();
                Debug.Log("✅ App version ตรงกัน");
            }
        }
        else
        {
            ShowNotification("❌ ไม่สามารถติดต่อกับเซิร์ฟเวอร์ได้");
            Debug.LogError("CheckVersion failed: " + request.error);
        }
    }

    [System.Serializable]
    public class VersionResponse
    {
        public string version;
        public string message;
    }

    public static LoginForm Instance;

    private void Awake()
    {
        Instance = this;
    }
    // ====== UPDATE VERSION ======
    public void OnUpdateYes()
    {
        string updateUrl = "https://drive.google.com/drive/u/0/folders/10VdT3HDxVNxkGLCA-maBoVRKnusV8J6L"; // 👉 ใส่ลิงก์ที่คุณต้องการ
        Application.OpenURL(updateUrl);
    }


    // ====== UI Control ======
    public static void ToggleLoginForm()
    {
        if (!LoginStatus && Instance != null)
        {
            OnclickLoginBtn = !OnclickLoginBtn;
            Instance.loginForm.SetActive(OnclickLoginBtn);
        }
    }

    public void SetRegisterForm(bool show)
    {
        RegisterForm.SetActive(show);
        loginForm.SetActive(!show);
    }

    public void SetForgotForm(bool show)
    {
        ForgotForm.SetActive(show);
        loginForm.SetActive(!show);
    }

    public void SetVerifyOTPForm(bool show)
    {
        VerifyOTPForm.SetActive(show);
        ForgotForm.SetActive(!show);
    }

    public void SetResetPasswordForm(bool show)
    {
        ResetPasswordForm.SetActive(show);
        VerifyOTPForm.SetActive(!show);
    }
    public void ShowLogoutForm()
    {
        LogoutForm.SetActive(true);
    }
    public void HideLogoutForm()
    {
        LogoutForm.SetActive(false);
    }
    public void ToggleCreditsPanel()
    {
        if (CreditsPanel != null)
        {
            bool isActive = CreditsPanel.activeSelf;
            CreditsPanel.SetActive(!isActive);
        }
    }

    // ====== NOTIFICATION ======
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

    // ====== LOGOUT ======
    public void Logout()
    {
        if (LoginStatus && id != -1)
        {
            ShowNotification("Logging out...");
            StartCoroutine(LogoutCoroutine(id));
        }

        // Reset UI และค่าต่าง ๆ
        LoginStatus = false;
        OnclickLoginBtn = false;
        id = -1;

        LogoutForm.SetActive(false);
        LogoutBtn.SetActive(false);
        PressToLogouttxt.SetActive(false);
        loginForm.SetActive(false);
        RegisterForm.SetActive(false);
        ForgotForm.SetActive(false);
        VerifyOTPForm.SetActive(false);
        ResetPasswordForm.SetActive(false);
        Profiletxt.SetActive(false);
        PressToLogintxt.SetActive(true);
    }

    IEnumerator LogoutCoroutine(int uid)
    {
        string url = "https://codingforlearning.onrender.com/user/logout";
        var jsonBody = JsonUtility.ToJson(new LogoutRequest(uid));

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Logout API success");
        }
        else
        {
            Debug.LogError("❌ Logout API failed: " + request.error);
        }
    }
    [System.Serializable]
    public class LogoutRequest
    {
        public int uid;
        public LogoutRequest(int uid)
        {
            this.uid = uid;
        }
    }


    // ====== LOGIN ======
    public void LoginBtnClick()
    {
        Debug.Log("➡️ กดปุ่ม Login แล้ว");
        if (!IsValidInput(Email.text.Trim(), 1, true) ||
            !IsValidInput(Password.text.Trim(), 1, false))
        {
            ShowNotification("⚠️ Email หรือ Password ไม่ถูกต้อง");
            wrongAudio.Play();
            return;
        }

        Loading.ShowLoadingScreen();
        StartCoroutine(LoginCoroutine(Email.text.Trim(), Password.text.Trim()));
    }


    IEnumerator LoginCoroutine(string email, string password)
    {

        string url = "https://codingforlearning.onrender.com/user/login";
        var jsonBody = JsonUtility.ToJson(new LoginRequest(email, password));

        var request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        Loading.HideLoadingScreen();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            // แยก status
            StatusResponse response = JsonUtility.FromJson<StatusResponse>(json);

            switch (response.status)
            {
                case "Success":
                    id = response.data.uid;
                    Debug.Log("✅ Login success, uid: " + id);
                    loginForm.SetActive(false);
                    OnclickLoginBtn = true;
                    LoginStatus = true;
                    PressToLogintxt.SetActive(false);
                    Profiletxt.SetActive(LoginStatus);
                    LogoutBtn.SetActive(LoginStatus);
                    PressToLogouttxt.SetActive(LoginStatus);
                    break;

                case "InvalidCredentials":
                    ShowNotification("⚠️ อีเมลหรือรหัสผ่านไม่ถูกต้อง");
                    wrongAudio.Play();
                    break;

                case "AlreadyLoggedIn":
                    ShowNotification("⚠️ ผู้ใช้นี้กำลังล็อกอินอยู่ที่อื่น");
                    wrongAudio.Play();
                    break;

                case "ServerError":
                default:
                    ShowNotification("❌ เกิดข้อผิดพลาดจากเซิร์ฟเวอร์");
                    wrongAudio.Play();
                    break;
            }
        }
        else
        {
            // ShowNotification("❌ ไม่สามารถเชื่อมต่อกับเซิร์ฟเวอร์ได้");
            string json = request.downloadHandler.text;
            // แยก status
            StatusResponse response = JsonUtility.FromJson<StatusResponse>(json);
            switch (response.status)
            {
                case "InvalidCredentials":
                    ShowNotification("⚠️ อีเมลหรือรหัสผ่านไม่ถูกต้อง");
                    wrongAudio.Play();
                    break;
                case "AlreadyLoggedIn":
                    ShowNotification("⚠️ ผู้ใช้นี้กำลังล็อกอินอยู่ที่อื่น");
                    wrongAudio.Play();
                    break;

                case "ServerError":
                default:
                    ShowNotification("❌ เกิดข้อผิดพลาดจากเซิร์ฟเวอร์");
                    wrongAudio.Play();
                    break;
            }
            wrongAudio.Play();
            Debug.LogError("Login request failed: " + request.error);
        }
    }

    // Model สำหรับอ่าน status
    [System.Serializable]
    public class StatusResponse
    {
        public string status;
        public string message;
        public User data;
    }

    [System.Serializable]
    public class LoginRequest
    {
        public string email;
        public string password;
        public LoginRequest(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }
    [System.Serializable]
    public class User
    {
        public int uid;
        public string name;
        public string email;
        public bool status;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string message;
        public User user;
    }

    // ====== VALIDATION HELPERS ======
    private bool IsValidInput(string value, int minLength = 1, bool requireAtSymbol = false)
    {
        if (string.IsNullOrEmpty(value)) return false;
        if (value.Length < minLength) return false;
        if (requireAtSymbol && !value.Contains("@")) return false;
        return true;
    }


    // ====== REGISTER ======

    public void RegisterBtnClick()
    {
        Debug.Log("➡️ กดปุ่ม Register แล้ว");
        if (string.IsNullOrEmpty(UserNameRegister.text.Trim()))
        {
            ShowNotification("⚠️ กรุณากรอกชื่อผู้ใช้");
            wrongAudio.Play();
            return;
        }else if (UserNameRegister.text.Contains(" "))
        {
            ShowNotification("⚠️ ชื่อผู้ใช้ห้ามมีช่องว่าง");
            wrongAudio.Play();
            return;
        }
        else if (!IsValidInput(EmailRegister.text.Trim(), 1, true) || !EmailRegister.text.Contains(".") || EmailRegister.text.StartsWith("@") || EmailRegister.text.EndsWith("@") || EmailRegister.text.EndsWith(".") || EmailRegister.text.Contains("..") || EmailRegister.text.Contains(" "))
        {
            ShowNotification("⚠️ รูปแบบอีเมลไม่ถูกต้อง");
            wrongAudio.Play();
            return;
        }
        else if (!IsValidInput(PasswordRegister.text.Trim(), 4, false))
        {
            ShowNotification("⚠️ รหัสผ่านต้องมีความยาวอย่างน้อย 4 ตัวอักษร");
            wrongAudio.Play();
            return;
        }else if (PasswordRegister.text.Contains(" "))
        {
            ShowNotification("⚠️ รหัสผ่านห้ามมีช่องว่าง");
            wrongAudio.Play();
            return;
        }

        Loading.ShowLoadingScreen();
        StartCoroutine(RegisterCoroutine(
            UserNameRegister.text.Trim(),
            EmailRegister.text.Trim(),
            PasswordRegister.text.Trim()
        ));
    }


    IEnumerator RegisterCoroutine(string name, string email, string password)
    {
        string url = "https://codingforlearning.onrender.com/user/register";
        var jsonBody = JsonUtility.ToJson(new RegisterRequest(name, email, password));

        var request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        Loading.HideLoadingScreen();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // สมมติ API ส่งกลับ status
            StatusResponse response = JsonUtility.FromJson<StatusResponse>(request.downloadHandler.text);
            switch (response.status)
            {
                case "Success":
                    SetRegisterForm(false);
                    ShowNotification("✅ สมัครสมาชิกสำเร็จ");
                    break;
                default:
                    ShowNotification("❌ เกิดข้อผิดพลาดจากเซิร์ฟเวอร์");
                    wrongAudio.Play();
                    break;
            }
        }
        else
        {
            // สมมติ API ส่งกลับ status
            StatusResponse response = JsonUtility.FromJson<StatusResponse>(request.downloadHandler.text);
            switch (response.status)
            {
                case "EmailExists":
                    ShowNotification("⚠️ อีเมลนี้มีผู้ใช้แล้ว");
                    wrongAudio.Play();
                    break;
                case "ServerError":
                default:
                    ShowNotification("❌ เกิดข้อผิดพลาดจากเซิร์ฟเวอร์");
                    wrongAudio.Play();
                    break;
            }
            // ShowNotification("❌ ไม่สามารถเชื่อมต่อกับเซิร์ฟเวอร์ได้");
            wrongAudio.Play();
            Debug.LogError("Register request failed: " + request.error);
        }
    }

    [System.Serializable]
    public class RegisterRequest
    {
        public string name;
        public string email;
        public string password;
        public RegisterRequest(string name, string email, string password)
        {
            this.name = name;
            this.email = email;
            this.password = password;
        }
    }

    // ====== SEND OTP ======
    public void SendOTP()
    {
        Loading.ShowLoadingScreen();
        StartCoroutine(SendOTPCoroutine(EmailForOTP.text));
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

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("📩 OTP ส่งสำเร็จ");
            SetVerifyOTPForm(true);
            Loading.HideLoadingScreen();
        }
        else
        {
            Loading.HideLoadingScreen();
            ShowNotification("❌ ส่ง OTP ไม่สำเร็จ (ตรวจสอบอีเมลอีกครั้ง)");
            Debug.LogError("❌ ส่ง OTP ไม่สำเร็จ: " + request.downloadHandler.text);
            wrongAudio.Play(); // เล่นเสียงผิดพลาด
        }
    }

    // ====== VERIFY OTP ======
    public void VerifyOTP()
    {
        Loading.ShowLoadingScreen();
        StartCoroutine(VerifyOTPCoroutine(EmailForOTP.text, OTP.text));
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
            Loading.HideLoadingScreen();
            SetResetPasswordForm(true);
        }
        else
        {
            Loading.HideLoadingScreen();
            ShowNotification("❌ OTP ไม่ถูกต้องหรือหมดอายุ ");
            Debug.LogError("❌ OTP ไม่ถูกต้องหรือหมดอายุ: " + request.downloadHandler.text);
            wrongAudio.Play(); // เล่นเสียงผิดพลาด
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

    // ====== RESET PASSWORD ======
    public void SubmitNewPassword()
    {
        if (!IsValidInput(NewPassword.text.Trim(), 4, false) ||
            !IsValidInput(ConfirmPassword.text.Trim(), 4, false))
        {
            ShowNotification("⚠️ รหัสผ่านต้องมีความยาวอย่างน้อย 4 ตัวอักษร");
            wrongAudio.Play(); // เล่นเสียงผิดพลาด
            return;
        }else if (NewPassword.text.Contains(" ") || ConfirmPassword.text.Contains(" "))
        {
            ShowNotification("⚠️ รหัสผ่านห้ามมีช่องว่าง");
            wrongAudio.Play(); // เล่นเสียงผิดพลาด
            return;
        }
        if (NewPassword.text == ConfirmPassword.text)
        {
            Loading.ShowLoadingScreen();
            StartCoroutine(ResetPasswordCoroutine(EmailForOTP.text, NewPassword.text));
        }
        else
        {
            ShowNotification("❌ รหัสผ่านไม่ตรงกัน");
            Debug.Log("❌ รหัสผ่านไม่ตรงกัน");
            wrongAudio.Play(); // เล่นเสียงผิดพลาด
        }
    }

    IEnumerator ResetPasswordCoroutine(string email, string newPassword)
    {
        string url = "https://codingforlearning.onrender.com/user/reset-password";
        var jsonBody = JsonUtility.ToJson(new ResetPassword(email, newPassword));

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        Loading.HideLoadingScreen();

        if (request.result == UnityWebRequest.Result.Success)
        {
            StatusResponse response = JsonUtility.FromJson<StatusResponse>(request.downloadHandler.text);

            switch (response.status)
            {
                case "Success":
                    SetResetPasswordForm(false);
                    SetVerifyOTPForm(false);
                    SetForgotForm(false);
                    loginForm.SetActive(true);
                    ShowNotification("🔐 เปลี่ยนรหัสผ่านสำเร็จ");
                    break;
                case "UserNotFound":
                    ShowNotification("⚠️ ไม่พบผู้ใช้ในระบบ");
                    wrongAudio.Play();
                    break;
                case "ServerError":
                default:
                    ShowNotification("❌ เกิดข้อผิดพลาดจากเซิร์ฟเวอร์");
                    wrongAudio.Play();
                    break;
            }
        }
        else
        {
            ShowNotification("❌ ไม่สามารถเชื่อมต่อกับเซิร์ฟเวอร์ได้");
            wrongAudio.Play();
            Debug.LogError("ResetPassword request failed: " + request.error);
        }
    }


    [System.Serializable]
    public class ResetPassword
    {
        public string email;
        public string newPassword;
        public ResetPassword(string email, string newPassword)
        {
            this.email = email;
            this.newPassword = newPassword;
        }
    }
}
