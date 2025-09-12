using UnityEngine;

public class WobbleTextX : MonoBehaviour
{
    public float amplitude = 5f; // ระยะที่ขยับ
    public float frequency = 5f; // ความเร็วในการขยับ
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPos + new Vector3(offset, 0f, 0f);
    }
}
