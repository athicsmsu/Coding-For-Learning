using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level9Java : MonoBehaviour
{
    private bool isShowingWrongEffect = false;
    public GameObject smokePrefab;     // ควันแบบ sprite หรือ .aseprite ที่ตั้งค่าไว้แล้ว
    public GameObject knightPrefab;    // ตัวละครอัศวิน (Prefab)
    public GameObject skeletonPrefab; // ตัวละครโครงกระดูก (Prefab)

    public void Correct(string answer, Text askText, PlayerController player)
    {
        Debug.Log("Correct Java C9");

        if (askText != null)
            askText.text = answer;

        StartCoroutine(TransformToKnight(player));
    }

    private IEnumerator TransformToKnight(PlayerController player)
    {
        if (player != null && player.CurrentCharacter != null)
        {
            // 1. ค่อยๆ จางหายไป


            // 2. สร้างควัน + fade in
            GameObject smoke = Instantiate(smokePrefab);
            smoke.transform.position = new Vector3(-4.5f, -0.47f, 0);
            smoke.transform.localScale = new Vector3(4.3f, 4.3f, 4.3f);
            SetAlpha(smoke, 0f);


            // 3. ค่อยๆ จางหายไป


            // 4. สร้างอัศวิน + fade in
            GameObject knight = Instantiate(knightPrefab);
            knight.transform.position = new Vector3(-4.5f, -4.5f, 0);
            knight.transform.localScale = new Vector3(10, 10, 10);
            SetAlpha(knight, 0f);

            yield return StartCoroutine(FadeObject(player.CurrentCharacter, 1f, 0f, 0.5f));

            yield return StartCoroutine(FadeObject(smoke, 0f, 1f, 0.5f));

            yield return StartCoroutine(FadeObject(smoke, 1f, 0f, 0.5f));
            Destroy(smoke);

            yield return StartCoroutine(FadeObject(knight, 0f, 1f, 0.5f));

            // 5. เล่นท่า Win
            Animator animator = knight.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Win");
            }

            yield return new WaitForSeconds(3f);
            Destroy(knight);
        }
    }

    public void Wrong(Text askText, PlayerController player)
{
    if (isShowingWrongEffect) return; // ป้องกันไม่ให้ซ้ำ
    isShowingWrongEffect = true;

    Debug.Log("Wrong Java W9");

    if (askText != null)
        askText.text = "Lose!";

    StartCoroutine(TransformToSkeleton(player));
}

    private IEnumerator TransformToSkeleton(PlayerController player)
    {
        if (player != null && player.CurrentCharacter != null)
        {
            GameObject smoke = Instantiate(smokePrefab);
            smoke.transform.position = new Vector3(-4.5f, -0.47f, 0);
            smoke.transform.localScale = new Vector3(4.3f, 4.3f, 4.3f);
            SetAlpha(smoke, 0f);

            // สร้าง skeleton และเซ็ตตำแหน่ง+ขนาดตามที่ต้องการ
            GameObject skeleton = Instantiate(skeletonPrefab);
            skeleton.transform.position = new Vector3(-4.5f, -4.5f, 0);
            skeleton.transform.localScale = new Vector3(10, 10, 10);
            SetAlpha(skeleton, 0f);

            yield return StartCoroutine(FadeObject(player.CurrentCharacter, 1f, 0f, 0.5f));

            yield return StartCoroutine(FadeObject(smoke, 0f, 1f, 0.5f));

            yield return StartCoroutine(FadeObject(smoke, 1f, 0f, 0.5f));
            Destroy(smoke);

            yield return StartCoroutine(FadeObject(skeleton, 0f, 1f, 0.5f));

            // เล่นท่า Lose
            Animator animator = skeleton.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Lose");
            }
            yield return new WaitForSeconds(2f);

            yield return StartCoroutine(FadeObject(skeleton, 1f, 0f, 0.5f));
            Destroy(skeleton);
            isShowingWrongEffect = false;
            yield return StartCoroutine(FadeObject(player.CurrentCharacter, 0f, 1f, 0.5f));
        }
    }


    private void TriggerAnimation(PlayerController player, string trigger)
    {
        if (player != null && player.CurrentCharacter != null)
        {
            Animator animator = player.CurrentCharacter.GetComponent<Animator>();
            if (animator != null && animator.runtimeAnimatorController != null)
            {
                animator.ResetTrigger("Win");
                animator.ResetTrigger("Lose");
                animator.ResetTrigger("Idle");

                animator.SetTrigger(trigger);
            }
        }
    }
    // ตั้งค่า alpha ให้กับ GameObject (รวม SpriteRenderer ทั้งหมดในลูก)
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

    // ค่อยๆ เปลี่ยน alpha จาก start → end
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

        // Ensure final value
        foreach (var rend in renderers)
        {
            Color color = rend.color;
            color.a = endAlpha;
            rend.color = color;
        }
    }
}
