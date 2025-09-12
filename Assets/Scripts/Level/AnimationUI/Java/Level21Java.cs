using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level21Java : MonoBehaviour
{
    public GameObject smokePrefab;
    public GameObject lancerPrefab;
    public GameObject skeletonPrefab;

    private bool isBusy = false;

    public void Correct(string answer, Text askText, PlayerController player)
    {
        if (isBusy || player == null || player.CurrentCharacter == null) return;
        isBusy = true;

        if (askText != null) askText.text = answer;

        if (answer.Trim() == "50")
        {
            StartCoroutine(TransformToCharacter(player, lancerPrefab, "Win", false));
        }
        else
        {
            isBusy = false; // fallback
            Debug.LogWarning("⚠️ Invalid correct answer: " + answer);
        }
    }

    public void Wrong(string answer, Text askText, PlayerController player)
    {
        if (isBusy || player == null || player.CurrentCharacter == null) return;
        isBusy = true;

        if (askText != null) askText.text = "Lose!";

        StartCoroutine(TransformToCharacter(player, skeletonPrefab, "Lose", true));
    }

    private IEnumerator TransformToCharacter(PlayerController player, GameObject prefab, string trigger, bool returnToOriginal)
    {
        GameObject mainChar = player.CurrentCharacter;
        if (mainChar == null) yield break;

        // 1. จางหายตัวหลัก
        yield return StartCoroutine(FadeObject(mainChar, 1f, 0f, 0.5f));

        // 2. ควัน
        GameObject smoke = Instantiate(smokePrefab);
        smoke.transform.position = new Vector3(-4.5f, -0.47f, 0);
        smoke.transform.localScale = new Vector3(4.3f, 4.3f, 4.3f);
        SetAlpha(smoke, 0f);



        yield return StartCoroutine(FadeObject(smoke, 0f, 1f, 0.5f));
        yield return StartCoroutine(FadeObject(smoke, 1f, 0f, 0.5f));
        Destroy(smoke);

        // 3. ตัวละครใหม่
        GameObject newChar = Instantiate(prefab);
        newChar.transform.position = new Vector3(-4.5f, -4.5f, 0);
        newChar.transform.localScale = new Vector3(10, 10, 10);
        SetAlpha(newChar, 0f);

        yield return StartCoroutine(FadeObject(newChar, 0f, 1f, 0.5f));

        // 4. เล่นท่า Win หรือ Lose
        Animator animator = newChar.GetComponent<Animator>();
        if (animator != null) animator.SetTrigger(trigger);

        yield return new WaitForSeconds(trigger == "Win" ? 3f : 2f);

        if (returnToOriginal)
        {
            yield return StartCoroutine(FadeObject(newChar, 1f, 0f, 0.5f));
            Destroy(newChar);
            yield return StartCoroutine(FadeObject(mainChar, 0f, 1f, 0.5f));
        }
        else
        {
            // ✅ ปิดทุกตัวทันที
            newChar.SetActive(false);
            mainChar.SetActive(false);
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
