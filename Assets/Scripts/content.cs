using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class AutoRebuildOnTextChange : MonoBehaviour
{
    private RectTransform rectTransform;
    private Text[] textComponents;
    private TMP_Text[] tmpTextComponents;

    private string[] lastTexts;
    private string[] lastTMPTexts;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // หา Text ภายใน
        textComponents = GetComponentsInChildren<Text>(true);
        tmpTextComponents = GetComponentsInChildren<TMP_Text>(true);

        // เก็บค่าเดิม
        lastTexts = new string[textComponents.Length];
        lastTMPTexts = new string[tmpTextComponents.Length];

        for (int i = 0; i < textComponents.Length; i++)
        {
            lastTexts[i] = textComponents[i].text;
        }
        for (int i = 0; i < tmpTextComponents.Length; i++)
        {
            lastTMPTexts[i] = tmpTextComponents[i].text;
        }
    }

    void Update()
    {
        bool changed = false;

        // เช็ค Text ปกติ
        for (int i = 0; i < textComponents.Length; i++)
        {
            if (textComponents[i].text != lastTexts[i])
            {
                lastTexts[i] = textComponents[i].text;
                changed = true;
            }
        }

        // เช็ค TMP_Text
        for (int i = 0; i < tmpTextComponents.Length; i++)
        {
            if (tmpTextComponents[i].text != lastTMPTexts[i])
            {
                lastTMPTexts[i] = tmpTextComponents[i].text;
                changed = true;
            }
        }

        if (changed)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}
