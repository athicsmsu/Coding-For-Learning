using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level9Python : MonoBehaviour
{
    public GameObject hammerObject;
    public GameObject goodSword;
    public GameObject brokenSword;
    public Transform hammerTargetPosition;

    private bool isAnimating = false;

    void Start()
    {
        // เริ่มต้นซ่อนดาบและค้อน
        goodSword.SetActive(false);
        brokenSword.SetActive(false);
    }

    // ✅ รองรับการเรียกจาก AnswerUIManager
    public void Correct(string answer, Text askText, PlayerController player)
    {
        if (isAnimating) return;
        isAnimating = true;

        if (askText != null)
            askText.text = answer;

        goodSword.SetActive(false);
        brokenSword.SetActive(false);

        StartCoroutine(ForgeSequence(true, askText, player));
    }

    public void Wrong(Text askText, PlayerController player)
    {
        if (isAnimating) return;
        isAnimating = true;

        if (askText != null)
            askText.text = "Lose!";

        goodSword.SetActive(false);
        brokenSword.SetActive(false);

        StartCoroutine(ForgeSequence(false, askText, player));
    }

    private IEnumerator ForgeSequence(bool isCorrect, Text askText, PlayerController player)
{
    if (hammerObject != null)
    {
        hammerObject.SetActive(true);
        
        // 🔰 เริ่มจากตำแหน่งบนเล็กน้อย (เหนือโต๊ะ)
        Vector3 startPos = hammerTargetPosition.position + new Vector3(0f, 2f, 0f); // ยกขึ้นจากโต๊ะ
        Vector3 endPos = hammerTargetPosition.position;

        hammerObject.transform.position = startPos;
        hammerObject.transform.rotation = Quaternion.identity;

        // ✅ เคลื่อนที่ลงมาค่อยๆ
        yield return StartCoroutine(MoveHammerSmooth(hammerObject, startPos, endPos, 0.3f));

        // ✅ หลังจากลงมาถึง ค่อยทำ shake
        yield return StartCoroutine(ShakeHammer(hammerObject));
    }

    // ✅ แสดงดาบ
    GameObject swordToShow = isCorrect ? goodSword : brokenSword;
    swordToShow.SetActive(true);
    SetAlpha(swordToShow, 0f);
    yield return StartCoroutine(FadeObject(swordToShow, 0f, 1f, 0.5f));

    // ✅ ท่าทางตัวละคร
    TriggerAnimation(player, isCorrect ? "Win" : "Lose");
    yield return new WaitForSeconds(2f);
    TriggerAnimation(player, "Idle");

    // ✅ ซ่อนค้อน
        if (hammerObject != null)
            hammerObject.SetActive(false);

    isAnimating = false;
}

    private IEnumerator ShakeHammer(GameObject hammer)
    {
        float duration = 0.5f;
        float time = 0f;
        float angle = 20f;

        while (time < duration)
        {
            float rotationZ = Mathf.Sin(time * 30f) * angle;
            hammer.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            time += Time.deltaTime;
            yield return null;
        }

        hammer.transform.rotation = Quaternion.identity;
    }

    private void TriggerAnimation(PlayerController player, string trigger)
    {
        if (player != null && player.CurrentCharacter != null)
        {
            Animator animator = player.CurrentCharacter.GetComponent<Animator>();
            if (animator != null)
            {
                animator.ResetTrigger("Win");
                animator.ResetTrigger("Lose");
                animator.ResetTrigger("Idle");

                animator.SetTrigger(trigger);
            }
        }
    }

    private void SetAlpha(GameObject obj, float alpha)
    {
        SpriteRenderer[] renderers = obj.GetComponentsInChildren<SpriteRenderer>();
        foreach (var rend in renderers)
        {
            Color color = rend.color;
            color.a = alpha;
            rend.color = color;
        }
    }

    private IEnumerator FadeObject(GameObject obj, float startAlpha, float endAlpha, float duration)
    {
        SpriteRenderer[] renderers = obj.GetComponentsInChildren<SpriteRenderer>();
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            float a = Mathf.Lerp(startAlpha, endAlpha, t);
            foreach (var rend in renderers)
            {
                Color color = rend.color;
                color.a = a;
                rend.color = color;
            }

            time += Time.deltaTime;
            yield return null;
        }

        foreach (var rend in renderers)
        {
            Color color = rend.color;
            color.a = endAlpha;
            rend.color = color;
        }
    }
    private IEnumerator MoveHammerSmooth(GameObject hammer, Vector3 from, Vector3 to, float duration)
{
    float time = 0f;
    while (time < duration)
    {
        hammer.transform.position = Vector3.Lerp(from, to, time / duration);
        time += Time.deltaTime;
        yield return null;
    }
    hammer.transform.position = to; // ensure final position
}
}
