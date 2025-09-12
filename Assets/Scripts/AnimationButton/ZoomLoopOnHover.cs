using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomLoopOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float zoomAmount = 0.1f;      // ขยาย-หด เท่าไร (เช่น 0.1 = 10%)
    public float frequency = 3f;         // ความเร็วการ zoom loop
    private Vector3 originalScale;
    private bool isHover = false;
    private float timeCounter = 0f;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isHover)
        {
            timeCounter += Time.deltaTime * frequency;
            float scaleOffset = Mathf.Sin(timeCounter) * zoomAmount;
            transform.localScale = originalScale + Vector3.one * scaleOffset;
        }
        else
        {
            // ถ้าไม่ได้ Hover → กลับมา original scale อย่างนุ่มนวล
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * 5f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;
        timeCounter = 0f;  // reset loop ให้ซิงค์
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
    }
}
