using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CodeHighlightInfo : MonoBehaviour
{
    public TMP_Text codeText; // Text ที่ใช้แสดงผลไฮไลต์
    [TextArea(5, 20)]

    private string rawInput = "";
    private bool isUpdating = false;

    void Start()
    {
        // แสดงข้อความโค้ดตอนเริ่มต้นพร้อมไฮไลต์
        rawInput = codeText.text;
        UpdateHighlightedText(rawInput);
    }

    public void OnCodeChanged(string input)
    {
        if (isUpdating) return;
        isUpdating = true;

        rawInput = input;
        UpdateHighlightedText(rawInput);

        isUpdating = false;
    }

    void UpdateHighlightedText(string code)
    {
        string highlightedText = HighlightCode(code);
        codeText.text = highlightedText;
    }

    string HighlightCode(string code)
    {
        // สีฟ้าสำหรับ keyword class/static/void/public
        code = Regex.Replace(code, @"\b(class|static|void|public|protected|private|final)\b", @"<color=#007FFF>$1</color>");

        // สีแดงสำหรับชื่อ class, method, function (พวก Main, main, println)
        code = Regex.Replace(code, @"\b[A-Za-z_][A-Za-z0-9_]*\b(?=\s*\()", @"<color=#FF3333>$&</color>");

        // สีแดงสำหรับชนิดข้อมูล String
        code = Regex.Replace(code, @"\b(String|int|float|double|char|boolean|new)\b", @"<color=#FF3333>$1</color>");

        // วงเล็บ
        code = Regex.Replace(code, @"(\{|\})", @"<color=#000000>$1</color>");
        
        // สีเขียวสำหรับ comment แบบ //...
        code = Regex.Replace(code, @"(//.*?$)", @"<color=#009933>$1</color>", RegexOptions.Multiline);

        // สีเขียวเข้มสำหรับคำว่า "ตัวอย่าง" (rgb(20,130,0) => #148200)
    code = Regex.Replace(code, @"\b(ตัวอย่าง)\b", @"<color=#148200>$1</color>");

    // สีฟ้าอ่อนสำหรับคำว่า "ผลลัพธ์" (rgb(0,117,255) => #0075FF)
    code = Regex.Replace(code, @"\b(ผลลัพธ์)\b", @"<color=#0075FF>$1</color>");
    // สีส้มสำหรับคำว่า "ข้อควรระวัง" (rgb(255,35,0) => #FF2300)
    code = Regex.Replace(code, @"\b(ข้อควรระวัง)\b", @"<color=#FF6D00>$1</color>");

    code = Regex.Replace(code, @"\b(if|else)\b", @"<color=#FF3333>$1</color>");

        return code;
    }
}
