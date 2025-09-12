using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{
    public Image backgroundImage; // ตัวแสดงรูป Background ใน UI (Image Component)
    public Sprite[] images; // รูปทั้งหมดในโฟลเดอร์
    public float interval = 3f; // เวลาเปลี่ยนรูป (วินาที)
    public float fadeDuration = 1f; // ระยะเวลาการ Fade (วินาที)

    private int currentImageIndex = 0;
    private float timer = 0f;
    private bool isFading = false;

    void Start()
    {
        if (images.Length == 0)
        {
            Debug.LogError("No images found in the folder! Please assign the images.");
        }
        else
        {
            backgroundImage.sprite = images[currentImageIndex]; // ตั้งค่าเริ่มต้น
        }
    }

    void Update()
    {
        if (!isFading) // ถ้ายังไม่ได้ Fade
        {
            timer += Time.deltaTime;

            if (timer >= interval && images.Length > 0)
            {
                StartCoroutine(FadeToNextImage());
                timer = 0f; // รีเซ็ตตัวจับเวลา
            }
        }
    }

    private System.Collections.IEnumerator FadeToNextImage()
    {
        isFading = true;

        // ค่อย ๆ ลด Alpha ของรูปปัจจุบันลง
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            Color color = backgroundImage.color;
            color.a = Mathf.Lerp(1f, 0f, normalizedTime);
            backgroundImage.color = color;
            yield return null;
        }

        // เปลี่ยนรูปภาพ
        currentImageIndex = (currentImageIndex + 1) % images.Length;
        backgroundImage.sprite = images[currentImageIndex];

        // ค่อย ๆ เพิ่ม Alpha ของรูปใหม่
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            Color color = backgroundImage.color;
            color.a = Mathf.Lerp(0f, 1f, normalizedTime);
            backgroundImage.color = color;
            yield return null;
        }

        isFading = false;
    }
}
