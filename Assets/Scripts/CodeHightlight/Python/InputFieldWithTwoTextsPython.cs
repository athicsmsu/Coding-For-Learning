using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class TMPInputFieldTwoTextsPython : MonoBehaviour
{
    public TMP_InputField tmpInputField;
    public TMP_Text extraText;

    private RectTransform mainTextRect;
    private RectTransform extraTextRect;

    private bool isUpdating = false;

    void Start()
    {
        mainTextRect = tmpInputField.textComponent.rectTransform;
        extraTextRect = extraText.rectTransform;

        tmpInputField.onValueChanged.AddListener(OnTextChanged);
        UpdateExtraText(tmpInputField.text);
    }

    void Update()
    {
        // Sync RectTransform ขนาดและตำแหน่ง
        extraTextRect.sizeDelta = mainTextRect.sizeDelta;
        extraTextRect.anchoredPosition = mainTextRect.anchoredPosition;
    }
    public void OnClickNewButton()
    {
        tmpInputField.text = "# พิมพ์โค้ดของคุณตรงนี้\n";
                    
        UpdateExtraText(tmpInputField.text);
        tmpInputField.ForceLabelUpdate(); // อัพเดตตำแหน่ง cursor
    }

    void OnTextChanged(string value)
    {
        if (isUpdating) return;
        isUpdating = true;

        UpdateExtraText(value);

        isUpdating = false;
    }

    void UpdateExtraText(string text)
    {
        string highlighted = HighlightCode(text);
        extraText.text = highlighted;
    }

     string HighlightCode(string code)
{
    // 🔒 Escape < > เพื่อไม่ให้ชนกับแท็ก <color> ของ TMP
    //code = code.Replace("<", "&lt;").Replace(">", "&gt;");

    // ✅ 1. ไฮไลต์ string literals เช่น "ข้อความ" หรือ 'ข้อความ'
    // 💚 สีเขียว: #009933
    var stringList = new List<string>();
    code = Regex.Replace(code, @"(\"".*?\""|'.*?')", match =>
    {
        stringList.Add(match.Value);
        return $"__STRING{stringList.Count - 1}__";
    });

    // ✅ 2. ไฮไลต์ comment ที่ขึ้นต้นด้วย #
    // ⚪ สีเทา: #888888
    var commentList = new List<string>();
    code = Regex.Replace(code, @"(#.*?$)", match =>
    {
        commentList.Add(match.Value);
        return $"__COMMENT{commentList.Count - 1}__";
    }, RegexOptions.Multiline);

    // ✅ 3. ไฮไลต์ตัวเลข เช่น 123, 3.14
    // 🟣 สีม่วง: #9933CC
    code = Regex.Replace(code, @"\b\d+(\.\d+)?\b", @"<color=#9933CC>$&</color>");

    // ✅ 4. ไฮไลต์ชื่อฟังก์ชัน เช่น print(), input()
    // 🔴 สีแดง: #FF3333
    code = Regex.Replace(code, @"\b([a-zA-Z_][a-zA-Z0-9_]*)\s*(?=\()", @"<color=#FF3333>$1</color>");

    // ✅ 5. ไฮไลต์คำสงวน (keywords) ของ Python
    // 🔵 สีฟ้า: #007FFF
    string[] keywords = {
        "def", "return", "if", "elif", "else", "for", "while",
        "break", "continue", "class", "try", "except", "finally",
        "import", "from", "as", "pass", "raise", "with", "lambda",
        "in", "is", "and", "or", "not", "None", "True", "False"
    };
    string keywordPattern = $@"\b({string.Join("|", keywords)})\b";
    code = Regex.Replace(code, keywordPattern, @"<color=#007FFF>$1</color>");

    // ✅ 6. ใส่ string กลับเข้าที่ พร้อมสีเขียว
    for (int i = 0; i < stringList.Count; i++)
    {
        string val = $"<color=#009933>{stringList[i]}</color>"; // 💚
        code = code.Replace($"__STRING{i}__", val);
    }

    // ✅ 7. ใส่ comment กลับเข้าที่ พร้อมสีเทา
    for (int i = 0; i < commentList.Count; i++)
    {
        string val = $"<color=#888888>{commentList[i]}</color>"; // ⚪
        code = code.Replace($"__COMMENT{i}__", val);
    }

    return code;
}

}
