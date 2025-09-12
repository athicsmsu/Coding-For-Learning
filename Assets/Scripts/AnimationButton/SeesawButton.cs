using UnityEngine;

public class SeesawButton : MonoBehaviour
{
    public float angleAmplitude = 10f;  // มุมสูงสุดในการเอียง (องศา)
    public float frequency = 2f;        // ความเร็วในการโยกเยก
    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * frequency) * angleAmplitude;
        transform.localRotation = startRotation * Quaternion.Euler(0f, 0f, angle);
    }
}
