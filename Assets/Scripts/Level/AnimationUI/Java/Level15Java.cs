using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level15Java : MonoBehaviour
{
    public Transform friedEggPoint;
    public Transform boiledEggPoint;
    public Transform panFriedEggPoint;
    public float runSpeed = 2f; // ความเร็วในการวิ่ง

    public void Correct(string answer, Text askText, PlayerController player)
    {
        Debug.Log("✅ Correct Level15Java");

        if (askText != null)
            askText.text = answer;

        if (player == null || player.CurrentCharacter == null)
            return;

        // เดินลง แล้วไปตรงกลาง → Win
        StartCoroutine(MoveToFixedXAndTrigger(player, boiledEggPoint.position.x, "Win"));
    }

    public void Wrong(string answer, Text askText, PlayerController player)
    {
        Debug.Log("❌ Wrong Level15Java");

        if (askText != null)
            askText.text = "Lose!";

        if (player == null || player.CurrentCharacter == null)
            return;

        float targetX = player.CurrentCharacter.transform.position.x;

        if (answer.Contains("Fried egg") && !answer.Contains("Pan"))
        {
            targetX = friedEggPoint.position.x;
        }
        else if (answer.Contains("Pan-fried"))
        {
            targetX = panFriedEggPoint.position.x;
        }

        // เดินลง แล้วไปซ้ายหรือขวา → Lose
        StartCoroutine(MoveToFixedXAndTrigger(player, targetX, "Lose"));
    }

    private IEnumerator MoveToFixedXAndTrigger(PlayerController player, float targetX, string trigger)
{
    GameObject character = player.CurrentCharacter;
    Animator animator = character.GetComponent<Animator>();

    // ✅ บันทึกตำแหน่งเริ่มต้น
    Vector3 originalPosition = character.transform.position;

    // เดินลง
    animator.SetTrigger("Run");
    Vector3 downTarget = originalPosition + new Vector3(0f, -0.7f, 0f);
    yield return StartCoroutine(MoveToPosition(character, downTarget, runSpeed));

    // หันซ้าย/ขวา
    float dirX = targetX - character.transform.position.x;
    if (dirX != 0)
    {
        Vector3 scale = character.transform.localScale;
        scale.x = dirX > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        character.transform.localScale = scale;
    }

    // เดินแนวนอน
    Vector3 horizontalTarget = new Vector3(targetX, downTarget.y, downTarget.z);
    animator.SetTrigger("Run");
    yield return StartCoroutine(MoveToPosition(character, horizontalTarget, runSpeed));

    // แสดงท่าทาง Win / Lose
    TriggerAnimation(animator, trigger);

    // ✅ ถ้าแพ้ เดินกลับจุดเดิม
if (trigger == "Lose")
{
    yield return new WaitForSeconds(0.8f); // รอท่า Lose

    animator.SetTrigger("Run");
    yield return StartCoroutine(MoveToPosition(character, originalPosition, runSpeed));

    // หันกลับขวา
    Vector3 scale = character.transform.localScale;
    scale.x = Mathf.Abs(scale.x);
    character.transform.localScale = scale;

    yield return new WaitForSeconds(0.2f); // รอเดินกลับหยุด

    // ✅ Reset ก่อน Trigger Idle เพื่อความชัวร์
    animator.ResetTrigger("Run");
    animator.ResetTrigger("Win");
    animator.ResetTrigger("Lose");

    // ✅ Trigger Idle
    animator.SetTrigger("Idle");
    Debug.Log("✅ Triggered: Idle");
}
}


    private IEnumerator MoveToPosition(GameObject obj, Vector3 target, float speed)
    {
        while (Vector3.Distance(obj.transform.position, target) > 0.05f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void TriggerAnimation(Animator animator, string trigger)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.ResetTrigger("Win");
            animator.ResetTrigger("Lose");
            animator.ResetTrigger("Idle");
            animator.SetTrigger(trigger);
            Debug.Log($"🎯 Triggered: {trigger}");
        }
    }
}
