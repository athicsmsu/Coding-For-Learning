using System.Collections;
using System.Diagnostics; // Required for Process
using System.IO; // Required for File and Path
using UnityEngine;
using TMPro; // Required for TextMeshPro
using UnityEngine.UI;
using UnityEngine.Networking;  // Required for UnityWebRequest
using System.Text.RegularExpressions; // Required for Regex
using System.Linq;

public class CodeCompileFirst : MonoBehaviour
{
    public Text[] codeInput;
    public TMP_Text codeShow;
    public Level1 levelGameplayManager; // Reference to Level1 script
   
    public Text resultText;

    private void Start()
    {

    }
    // แก้จาก void เป็น IEnumerator
    public IEnumerator OnCompileJavaAuto()
    {
        int option = SettingManager.GetCodeOptionCompileJava();
        UnityEngine.Debug.Log($"Auto Compile Java: Option = {option}");

        if (option == 1)
        {
            yield return StartCoroutine(OnCompileJavaServer());
        }
        else if (option == 2)
        {
            yield return StartCoroutine(OnCompileJavaLocal());
        }

        levelGameplayManager.CheckOutput(); // รอ compile เสร็จแล้วค่อยเช็ค
    }

    public IEnumerator OnCompilePythonAuto()
    {
        int option = SettingManager.GetCodeOptionCompilePython();
        UnityEngine.Debug.Log($"Auto Compile Python: Option = {option}");

        if (option == 1)
        {
            yield return StartCoroutine(OnCompilePythonServer());
        }
        else if (option == 2)
        {
            yield return StartCoroutine(OnCompilePythonLocal());
        }

        levelGameplayManager.CheckOutput(); // รอ compile เสร็จแล้วค่อยเช็ค
    }
    public void OnClickCompileJava()
    {
        StartCoroutine(OnCompileJavaAuto());
    }
    public void OnClickCompilePython()
    {
        StartCoroutine(OnCompilePythonAuto());
    }


    public void OnClickNewButton()
    {

    }

    public IEnumerator OnCompileJavaLocal()
    {
        string[] userInput = new string[codeInput.Length];
        // ดึงค่าจาก InputField
        for (int i = 0; i < codeInput.Length; i++)
        {
            userInput[i] = codeInput[i].text;
        }

        // ดึงโค้ดจาก TMP_Text ที่โชว์อยู่
        string rawCodeWithTags = codeShow.text;

        // ลบแท็ก <color=...> และ </color> ออกให้หมดก่อน
        string rawCode = Regex.Replace(rawCodeWithTags, @"<color=.*?>|</color>", "");

        // แทนที่คำว่า choice1 ด้วยค่าที่ผู้ใช้ป้อน
        for (int i = 0; i < userInput.Length; i++)
        {
            rawCode = rawCode.Replace($"choice{i + 1}", $"{userInput[i]}");
        }
        string formattedCode = rawCode;

        // ตรวจสอบว่าผู้ใช้ป้อนข้อความหรือไม่
        if (userInput.Any(string.IsNullOrEmpty))
        {
            resultText.text = "Please enter the code to compile.";
            UnityEngine.Debug.LogError("One or more input fields are empty");
            yield break;
        }

        // ส่งโค้ดที่แทนค่าแล้วไป compile
        yield return StartCoroutine(SendCodeJava(formattedCode));
    }
    
    public IEnumerator OnCompilePythonLocal()
    {
        string[] userInput = new string[codeInput.Length];

        // ดึงค่าจาก InputField
        for (int i = 0; i < codeInput.Length; i++)
        {
            userInput[i] = codeInput[i].text;
        }

        // ดึงโค้ดจาก TMP_Text ที่โชว์อยู่
        string rawCodeWithTags = codeShow.text;

        // ลบแท็ก <color=...> และ </color> ออกให้หมดก่อน
        string rawCode = Regex.Replace(rawCodeWithTags, @"<color=.*?>|</color>", "");

        // แทนที่คำว่า choice1 ด้วยค่าที่ผู้ใช้ป้อน
        for (int i = 0; i < userInput.Length; i++)
        {
            rawCode = rawCode.Replace($"choice{i + 1}", $"{userInput[i]}");
        }
        string formattedCode = rawCode;

        // ตรวจสอบว่าผู้ใช้ป้อนข้อความหรือไม่
        if (userInput.Any(string.IsNullOrEmpty))
        {
            resultText.text = "Please enter the code to compile.";
            UnityEngine.Debug.LogError("One or more input fields are empty");
            yield break;
        }

        yield return StartCoroutine(SendCodePython(formattedCode));
    }

    // Method สำหรับ Compile Java ผ่าน HTTP POST
    public IEnumerator OnCompileJavaServer()
    {
        string[] userInput = new string[codeInput.Length];

        // ดึงค่าจาก InputField
        for (int i = 0; i < codeInput.Length; i++)
        {
            userInput[i] = codeInput[i].text;
        }

        // ดึงโค้ดจาก TMP_Text ที่โชว์อยู่
        string rawCodeWithTags = codeShow.text;

        // ลบแท็ก <color=...> และ </color> ออกให้หมดก่อน
        string rawCode = Regex.Replace(rawCodeWithTags, @"<color=.*?>|</color>", "");

        // แทนที่คำว่า choice1 ด้วยค่าที่ผู้ใช้ป้อน
        for (int i = 0; i < userInput.Length; i++)
        {
            rawCode = rawCode.Replace($"choice{i + 1}", $"{userInput[i]}");
        }
        string formattedCode = rawCode;

        // ตรวจสอบว่าผู้ใช้ป้อนข้อความหรือไม่
        if (userInput.Any(string.IsNullOrEmpty))
        {
            resultText.text = "Please enter the code to compile.";
            UnityEngine.Debug.LogError("One or more input fields are empty");
            yield break;
        }

        yield return StartCoroutine(SendCodeToServer("compile-java", formattedCode));
    }

    // Method สำหรับ Compile Python ผ่าน HTTP POST
    public IEnumerator OnCompilePythonServer()
    {
        string[] userInput = new string[codeInput.Length];

        // ดึงค่าจาก InputField
        for (int i = 0; i < codeInput.Length; i++)
        {
            userInput[i] = codeInput[i].text;
        }

        // ดึงโค้ดจาก TMP_Text ที่โชว์อยู่
        string rawCodeWithTags = codeShow.text;

        // ลบแท็ก <color=...> และ </color> ออกให้หมดก่อน
        string rawCode = Regex.Replace(rawCodeWithTags, @"<color=.*?>|</color>", "");

        // แทนที่คำว่า choice1 ด้วยค่าที่ผู้ใช้ป้อน
        for (int i = 0; i < userInput.Length; i++)
        {
            rawCode = rawCode.Replace($"choice{i + 1}", $"{userInput[i]}");
        }
        string formattedCode = rawCode;

        // ตรวจสอบว่าผู้ใช้ป้อนข้อความหรือไม่
        if (userInput.Any(string.IsNullOrEmpty))
        {
            resultText.text = "Please enter the code to compile.";
            UnityEngine.Debug.LogError("One or more input fields are empty");
            yield break;
        }

        yield return StartCoroutine(SendCodeToServer("compile-python", formattedCode));  // ส่งโค้ดไปที่เซิร์ฟเวอร์
    }

    [System.Serializable]
    public class CodeData
    {
        public string code;
    }

    // Coroutine สำหรับส่งโค้ดไปที่เซิร์ฟเวอร์
    private IEnumerator SendCodeToServer(string endpoint, string code)
    {
        string url = "https://codingforlearning.onrender.com/code/" + endpoint;  // URL ของเซิร์ฟเวอร์

        // สร้างข้อมูล JSON
        // object codeRequest = code;
        CodeData codeData = new CodeData { code = code };
        string jsonBody = JsonUtility.ToJson(codeData);

        UnityEngine.Debug.Log("Sending request with code: " + jsonBody);

        // สร้าง UnityWebRequest เพื่อทำ HTTP POST
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            // กำหนด Content-Type เป็น application/json
            request.SetRequestHeader("Content-Type", "application/json");

            // ใช้ UploadHandlerRaw เพื่อส่ง JSON เป็น raw body
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonBody));

            // ใช้ DownloadHandlerBuffer เพื่อรับข้อมูลจาก response
            request.downloadHandler = new DownloadHandlerBuffer();

            // ส่งข้อมูล
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // ถ้าการส่งข้อมูลสำเร็จ
                resultText.text = request.downloadHandler.text;  // แสดงผลลัพธ์จากเซิร์ฟเวอร์
                UnityEngine.Debug.Log($"Output: {request.downloadHandler.text}");
            }
            else
            {
                // ถ้ามีข้อผิดพลาด
                resultText.text = request.error;
                UnityEngine.Debug.LogError($"Request Error: {request.error}");
            }
        }
    }

    [System.Serializable]
    public class CodeRequest
    {
        public string code;
    }

    public IEnumerator SendCodeJava(string code)
    {
        string tempFileName = "Main.java";
        string className = Path.GetFileNameWithoutExtension(tempFileName);

        // Write the code to a temporary file
        File.WriteAllText(tempFileName, code);

        // Compile the code using javac
        Process compileProcess = new Process();
        compileProcess.StartInfo.FileName = "javac";
        compileProcess.StartInfo.Arguments = tempFileName;
        compileProcess.StartInfo.RedirectStandardError = true;
        compileProcess.StartInfo.RedirectStandardOutput = true;
        compileProcess.StartInfo.UseShellExecute = false;
        compileProcess.StartInfo.CreateNoWindow = true;

        compileProcess.Start();
        compileProcess.WaitForExit(); // Wait synchronously

        if (compileProcess.ExitCode != 0)
        {
            string error = compileProcess.StandardError.ReadToEnd();
            resultText.text = error;
            UnityEngine.Debug.LogError($"Compilation Error: {error}");
            File.Delete(tempFileName);
            yield break;
        }

        // Run the code using java
        Process runProcess = new Process();
        runProcess.StartInfo.FileName = "java";
        runProcess.StartInfo.Arguments = className;
        runProcess.StartInfo.RedirectStandardError = true;
        runProcess.StartInfo.RedirectStandardOutput = true;
        runProcess.StartInfo.UseShellExecute = false;
        runProcess.StartInfo.CreateNoWindow = true;

        runProcess.Start();
        runProcess.WaitForExit(); // Wait synchronously

        if (runProcess.ExitCode != 0)
        {
            //ถ้ารันโค้ดแล้ว Error
            string error = runProcess.StandardError.ReadToEnd();
            resultText.text = error;
            UnityEngine.Debug.LogError($"Runtime Error: {error}");
        }
        else
        {
            //ถ้ารันโค้ดแล้วรันได้ปกติไม่ Error
            string output = runProcess.StandardOutput.ReadToEnd();
            resultText.text = output;
            UnityEngine.Debug.Log($"{output}");
        }

        // Delete temporary files
        File.Delete(tempFileName);
        File.Delete($"{className}.class");
    }

    public IEnumerator SendCodePython(string code)
    {
        string tempFileName = "Main.py"; // ใช้ไฟล์ .py สำหรับ Python

        // เขียนโค้ดลงในไฟล์ชั่วคราว
        File.WriteAllText(tempFileName, code);

        // ตรวจสอบว่า python ติดตั้งอยู่หรือไม่
        if (!IsExecutableAvailable("python"))
        {
            resultText.text = "Python is not installed or not in PATH.";
            UnityEngine.Debug.LogError("Python is not available");
            File.Delete(tempFileName);
            yield break;
        }

        // รันโค้ดโดยใช้ Python

        Process runProcess = new Process();
        runProcess.StartInfo.FileName = "python"; // ใช้ python เป็นคำสั่ง
        runProcess.StartInfo.Arguments = tempFileName; // ส่งชื่อไฟล์ .py เป็น argument
        runProcess.StartInfo.RedirectStandardError = true;
        runProcess.StartInfo.RedirectStandardOutput = true;
        runProcess.StartInfo.UseShellExecute = false;
        runProcess.StartInfo.CreateNoWindow = true;

        runProcess.Start();
        runProcess.WaitForExit(); // Wait synchronously

        if (runProcess.ExitCode != 0)
        {
            // ถ้ารันโค้ดแล้ว Error
            string error = runProcess.StandardError.ReadToEnd();
            resultText.text = error;
            UnityEngine.Debug.LogError($"Runtime Error: {error}");

            // หยุดการทำงานของ IEnumerator
            yield break;
        }
        else
        {
            // ถ้ารันโค้ดแล้วรันได้ปกติไม่ Error
            string output = runProcess.StandardOutput.ReadToEnd();
            resultText.text = output;
            UnityEngine.Debug.Log($"Output: {output}");
        }

        // ลบไฟล์ชั่วคราว
        File.Delete(tempFileName);

        // สิ้นสุด IEnumerator อย่างสมบูรณ์
        yield return null;
    }

    private bool IsExecutableAvailable(string executable)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = executable,
                Arguments = " --version", // ใช้ --version เพื่อเช็คเวอร์ชันของ Python
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }).WaitForExit();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
