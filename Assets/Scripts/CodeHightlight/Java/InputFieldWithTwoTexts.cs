using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class TMPInputFieldTwoTexts : MonoBehaviour
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
        tmpInputField.text = "public class Main {\n" +
                          "    public static void main(String[] args) {\n" +
                          "\t// พิมพ์โค้ดของคุณตรงนี้\n" +
                          "\t\n"+
                          "    }\n" +
                          "}";
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
