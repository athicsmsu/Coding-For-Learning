using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level18Python : MonoBehaviour
{
    public GameObject smokePrefab;
    public GameObject swordmanPrefab;
    public GameObject magePrefab;
    public GameObject archerPrefab;
    public GameObject priestPrefab;

    private bool isBusy = false;

    public void Correct(string answer, Text askText, PlayerController player)
    {
        if (isBusy || player == null || player.CurrentCharacter == null) return;
        isBusy = true;

        if (askText != null) askText.text = answer;

        StartCoroutine(TransformToCharacter(player, swordmanPrefab, "Win", false));
    }

    public void Wrong(string answer, Text askText, PlayerController player)
    {
        if (isBusy || player == null || player.CurrentCharacter == null) return;
        isBusy = true;

        string lower = answer.Trim().ToLower();
        GameObject targetPrefab = null;

        if (lower == "mage")
        {
            if (askText != null) askText.text = "mage!";
            targetPrefab = magePrefab;
        }
        else if (lower == "archer")
        {
            if (askText != null) askText.text = "archer!";
            targetPrefab = archerPrefab;
        }
        else if (lower == "priest")
        {
            if (askText != null) askText.text = "priest!";
            targetPrefab = priestPrefab;
        }

        if (targetPrefab != null)
        {
            StartCoroutine(TransformToCharacter(player, targetPrefab, "Lose", true));
        }
        else
        {
            if (askText != null) askText.text = "Lose!";
            StartCoroutine(PlayMainCharacterLose(player));
        }
    }

    private IEnumerator PlayMainCharacterLose(PlayerController player)
    {
        GameObject mainChar = player.CurrentCharacter;
        if (mainChar == null)
        {
            isBusy = false;
            yield break;
        }

        Animator animator = mainChar.GetComponent<Animator>();
        if (animator != null)
        {
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Win");
            animator.ResetTrigger("Lose");
            animator.SetTrigger("Lose");
        }

        yield return new WaitForSeconds(3f);

        if (animator != null)
        {
            animator.ResetTrigger("Lose");
            animator.SetTrigger("Idle");
        }

        isBusy = false;
    }


    private IEnumerator TransformToCharacter(PlayerController player, GameObject prefab, string trigger, bool returnToOriginal)
    {
        GameObject mainChar = player.CurrentCharacter;
        if (mainChar == null) yield break;

        // 1. จางหายตัวหลัก
        yield return StartCoroutine(FadeObject(mainChar, 1f, 0f, 0.5f));

        // 2. ควัน
        GameObject smoke = Instantiate(smokePrefab);
        smoke.transform.position = new Vector3(-4.2f, 1.8f, 0);
        smoke.transform.localScale = new Vector3(2f, 2f, 2f);
        SetAlpha(smoke, 0f);

        yield return StartCoroutine(FadeObject(smoke, 0f, 1f, 0.5f));
        yield return StartCoroutine(FadeObject(smoke, 1f, 0f, 0.5f));
        Destroy(smoke);

        // 3. สร้างตัวใหม่
        GameObject newChar = Instantiate(prefab);
        newChar.transform.position = new Vector3(-4.2f, -0.5f, 0);
        newChar.transform.localScale = new Vector3(5f, 5f, 5f);
        SetAlpha(newChar, 0f);

        yield return StartCoroutine(FadeObject(newChar, 0f, 1f, 0.5f));

        // 4. เล่นท่า Win หรือ Lose
        Animator animator = newChar.GetComponent<Animator>();
        if (animator != null) animator.SetTrigger(trigger);

        yield return new WaitForSeconds(trigger == "Win" ? 3f : 2f);

        if (trigger == "Win")
        {
            // ✅ ปิดทุกตัวทันที
            newChar.SetActive(false);
            mainChar.SetActive(false);
            // ✅ ป้องกัน prefab ถูกแสดงจาก scene โดยตรง
            swordmanPrefab.SetActive(false);
            magePrefab.SetActive(false);
            archerPrefab.SetActive(false);
            priestPrefab.SetActive(false);
        }
        else if (returnToOriginal)
        {
            yield return StartCoroutine(FadeObject(newChar, 1f, 0f, 0.5f));
            Destroy(newChar);

            yield return StartCoroutine(FadeObject(mainChar, 0f, 1f, 0.5f));
        }

        isBusy = false;
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
        float t = 0f;

        while (t < duration)
        {
            float a = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            foreach (var rend in renderers)
            {
                Color c = rend.color;
                c.a = a;
                rend.color = c;
            }
            t += Time.deltaTime;
            yield return null;
        }

        foreach (var rend in renderers)
        {
            Color c = rend.color;
            c.a = endAlpha;
            rend.color = c;
        }
    }
}
