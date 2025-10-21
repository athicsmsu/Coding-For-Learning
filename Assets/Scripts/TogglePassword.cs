using UnityEngine;
using UnityEngine.UI;

public class TogglePassword : MonoBehaviour
{
    public InputField passwordField;
    public GameObject showIcon;  // ไอคอน "โชว์"
    public GameObject hideIcon;  // ไอคอน "ซ่อน"

    private bool isHidden = true;

    void Start()
    {
        HidePassword(); // เริ่มต้นด้วยการซ่อนรหัส
    }

    public void ShowPassword()
    {
        isHidden = false;

        passwordField.contentType = InputField.ContentType.Standard;
        passwordField.ForceLabelUpdate();

        showIcon.SetActive(false);
        hideIcon.SetActive(true);
    }

    public void HidePassword()
    {
        isHidden = true;

        passwordField.contentType = InputField.ContentType.Password;
        passwordField.ForceLabelUpdate();

        showIcon.SetActive(true);
        hideIcon.SetActive(false);
    }
}
