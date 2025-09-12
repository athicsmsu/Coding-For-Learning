using UnityEngine;

public class ZoomButton : MonoBehaviour
{
    public float zoomAmount = 0.1f;     // ขนาดที่ขยาย (เช่น 0.1 = 10%)
    public float frequency = 2f;        // ความเร็วในการซูม
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scaleOffset = Mathf.Sin(Time.time * frequency) * zoomAmount;
        transform.localScale = originalScale + Vector3.one * scaleOffset;
    }
}
