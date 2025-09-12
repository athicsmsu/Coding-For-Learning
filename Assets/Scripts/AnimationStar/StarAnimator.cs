using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StarAnimator : MonoBehaviour
{
    public GameObject starPrefab;   // ดาวแบบปกติ
    public GameObject starNon;      // ดาวแบบที่สอง
    public Transform[] starTargets; // จุดที่ดาวแต่ละดวงจะไปหยุด (3 ตำแหน่ง)
    public float delayBetweenStars = 0.5f;


    public AudioClip soundGood;  // เสียงสำหรับ starPrefab
    public AudioClip soundBad;   // เสียงสำหรับ starNon
    public AudioSource audioSource; // AudioSource สำหรับเล่นเสียง
    public AudioSource WinSound;

    public void ShowStars(int score)
    {
        WinSound.Play();
        StartCoroutine(AnimateStars(score));
    }

    public Transform[] spawnPoints; // 3 จุด spawn

    IEnumerator AnimateStars(int score)
    {
        int count = starTargets.Length;

        for (int i = 0; i < count; i++)
        {
            GameObject prefabToUse = starPrefab;

            if (score >= 100)
            {
                prefabToUse = starPrefab;
            }
            else if (score >= 70)
            {
                prefabToUse = (i == 2) ? starNon : starPrefab;
            }
            else if (score >= 10)
            {
                prefabToUse = (i == 0) ? starPrefab : starNon;
            }
            else
            {
                continue;
            }

            // สร้างดาวใหม่โดยใช้ parent เป็น container นี้เอง
            GameObject star = Instantiate(prefabToUse, transform);

            // กำหนดตำแหน่งเริ่มต้นจาก spawnPoints[i]
            if (spawnPoints != null && spawnPoints.Length > i && spawnPoints[i] != null)
            {
                star.transform.position = spawnPoints[i].position;
            }
            else
            {
                // ถ้า spawnPoints ไม่ถูกเซ็ต ให้ใช้ตำแหน่งเดิม (0,400) local space
                RectTransform rtFallback = star.GetComponent<RectTransform>();
                if (rtFallback != null)
                    rtFallback.anchoredPosition = new Vector2(0, 400);
            }

            // ตั้งขนาดเริ่มต้น
            RectTransform rt = star.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.sizeDelta = new Vector2(100, 100);
                rt.localScale = Vector3.one * 3f; // เริ่มต้นใหญ่
            }

            // เล่นเสียงดาว
            if (audioSource != null)
            {
                if (prefabToUse == starPrefab && soundGood != null)
                    audioSource.PlayOneShot(soundGood);
                else if (prefabToUse == starNon && soundBad != null)
                    audioSource.PlayOneShot(soundBad);
            }

            // เคลื่อนที่ไปยัง starTargets[i]
            StartCoroutine(MoveToTarget(star.transform, starTargets[i]));

            yield return new WaitForSeconds(delayBetweenStars);
        }
    }


    IEnumerator MoveToTarget(Transform star, Transform target)
    {
        float duration = 0.6f;
        float elapsed = 0f;
        Vector3 startPos = star.position;
        Vector3 endPos = target.position;

        Vector3 endScale = starPrefab.transform.localScale;
        Vector3 startScale = endScale * 3f;

        star.localScale = startScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            star.position = Vector3.Lerp(startPos, endPos, t);
            star.localScale = Vector3.Lerp(startScale, endScale, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        star.position = endPos;
        star.localScale = endScale;
    }
}
