using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CodeHighlight2 : MonoBehaviour
{
    public InputField inputField;  // InputField แบบ Legacy
    public Text highlightedText;   // Text แบบ Legacy สำหรับแสดงผล

    private bool isUpdating = false;

    void Start()
    {
        inputField.onValueChanged.AddListener(OnCodeChanged);
        UpdateHighlightedText(inputField.text);
    }

    public void OnCodeChanged(string input)
    {
        if (isUpdating) return;
        isUpdating = true;

        UpdateHighlightedText(input);

        isUpdating = false;
    }

    void UpdateHighlightedText(string code)
    {
        string highlighted = HighlightCode(code);
        highlightedText.text = highlighted;
    }

    string HighlightCode(string code)
    {
        // สีฟ้าสำหรับ keyword
        code = Regex.Replace(code, @"\b(class|static|void|public|protected|private|final)\b", @"<color=#007FFF>$1</color>");

        // สีแดงสำหรับชื่อ method
        code = Regex.Replace(code, @"\b[A-Za-z_][A-Za-z0-9_]*\b(?=\s*\()", @"<color=#FF3333>$&</color>");

        // สีแดงสำหรับชนิดข้อมูล
        code = Regex.Replace(code, @"\b(String|int|float|double|char|boolean)\b", @"<color=#FF3333>$1</color>");

        // วงเล็บ
        code = Regex.Replace(code, @"(\{|\})", @"<color=#000000>$1</color>");

        // สีเขียวสำหรับ comment แบบ //...
        code = Regex.Replace(code, @"(//.*?$)", @"<color=#009933>$1</color>", RegexOptions.Multiline);

        // ✅ สีเขียวสำหรับ String literal เช่น "Hello" หรือ 'c'
        code = Regex.Replace(code, @"(['""])(.*?)(['""])", @"<color=#009933>$1$2$3</color>");

        return code;
    }
}
