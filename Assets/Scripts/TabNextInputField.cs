using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabNextInputField : MonoBehaviour
{
    [Header("InputField ถัดไปเมื่อกด Tab")]
    public InputField nextInputField;

    private InputField currentInputField;

    void Awake()
    {
        currentInputField = GetComponent<InputField>();
    }

    void Update()
    {
        if (currentInputField.isFocused && Input.GetKeyDown(KeyCode.Tab))
        {
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            if (shift && nextInputField != null)
            {
                // ถ้าอยากย้อนกลับ สามารถเพิ่ม previousInputField ได้ แต่ตอนนี้ขอ Tab เดินหน้า
                // สามารถขยายได้ในอนาคต
            }
            else if (!shift && nextInputField != null)
            {
                nextInputField.Select();
                nextInputField.ActivateInputField();
            }

            // ป้องกัน Tab กดแล้วออกไปจาก UI
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
