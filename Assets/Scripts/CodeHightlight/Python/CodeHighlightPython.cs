using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class CodeHighlightPython : MonoBehaviour
{
    public TMP_Text codeText;

    [TextArea(5, 20)]
    private string rawInput = "";

    private bool isUpdating = false;

    void Start()
    {
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
    // 1. แยก string literals ออกก่อน
    var stringList = new List<string>();
    code = Regex.Replace(code, @"(\"".*?\""|'.*?')", match =>
    {
        stringList.Add(match.Value);
        return $"__STRING{stringList.Count - 1}__";
    });

    // ✅ 1.5 แทน <color=#xxxxxx> ด้วย placeholder ก่อน เพื่อป้องกันโดน regex comment จับผิด
    var colorTags = new Dictionary<string, string>();
    int colorIndex = 0;
    code = Regex.Replace(code, @"<color=#([0-9A-Fa-f]{6})>", match =>
    {
        string placeholder = $"__COLOR{colorIndex}__";
        colorTags[placeholder] = match.Value;
        colorIndex++;
        return placeholder;
    });

    // 2. แยก comment ที่ขึ้นต้นด้วย # ออก
    var commentList = new List<string>();
    code = Regex.Replace(code, @"(#.*?$)", match =>
    {
        commentList.Add(match.Value);
        return $"__COMMENT{commentList.Count - 1}__";
    }, RegexOptions.Multiline);

    // 3. highlight ตัวเลข
    code = Regex.Replace(code, @"\b\d+(\.\d+)?\b", @"<color=#9933CC>$&</color>");
    
    // 3.5 Highlight คำว่า "print" ให้เป็นสีแดง (ทุกกรณี)
        code = Regex.Replace(code, @"\bprint\b", @"<color=#FF3333>print</color>");

    // 4. highlight ฟังก์ชัน เช่น print()
        code = Regex.Replace(code, @"\b((?!print)[a-zA-Z_][a-zA-Z0-9_]*)\s*(?=\()", @"<color=#FF3333>$1</color>");

    // 5. highlight keyword
    string[] keywords = {
        "def", "return", "if", "elif", "else", "for", "while",
        "break", "continue", "class", "try", "except", "finally",
        "import", "from", "as", "pass", "raise", "with", "lambda",
        "in", "is", "and", "or", "not", "None", "True", "False"
    };
    string keywordPattern = $@"\b({string.Join("|", keywords)})\b";
    code = Regex.Replace(code, keywordPattern, @"<color=#007FFF>$1</color>");

    // 6. ใส่ string literals กลับเข้าไป
    for (int i = 0; i < stringList.Count; i++)
    {
        string val = $"<color=#009933>{stringList[i]}</color>";
        code = code.Replace($"__STRING{i}__", val);
    }

    // 7. ใส่ comment กลับเข้าไป
    for (int i = 0; i < commentList.Count; i++)
    {
        string val = $"<color=#888888>{commentList[i]}</color>";
        code = code.Replace($"__COMMENT{i}__", val);
    }

    // ✅ 8. คืน placeholder ของ <color=#xxxxxx> กลับ
    foreach (var kv in colorTags)
    {
        code = code.Replace(kv.Key, kv.Value);
    }

    return code;
}

}
