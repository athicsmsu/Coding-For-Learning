using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level14Python : MonoBehaviour
{
    public Transform tile2Point;      // พื้นหมายเลข 2
    public Transform fallOffPoint;    // ตำแหน่งตกน้ำ
    public float runSpeed = 2f;

    public void Correct(string answer, Text askText, PlayerController player)
    {
        Debug.Log("✅ Correct Level16Java");

        if (askText != null)
            askText.text = "2";

        if (player == null || player.CurrentCharacter == null)
            return;

        StartCoroutine(MoveToAndTrigger(player, tile2Point.position, "Win"));
    }

    public void Wrong(Text askText, PlayerController player)
    {
        Debug.Log("❌ Wrong Level16Java");

        if (askText != null)
            askText.text = "Lose!";

        if (player == null || player.CurrentCharacter == null)
            return;

        StartCoroutine(MoveToAndTrigger(player, fallOffPoint.position, "Lose", true));
    }

    private IEnumerator MoveToAndTrigger(PlayerController player, Vector3 target, string trigger, bool fall = false)
    {
        GameObject character = player.CurrentCharacter;
        Animator animator = character.GetComponent<Animator>();

        // บันทึกตำแหน่งเริ่มต้น
        Vector3 startPos = character.transform.position;

        // หันขวา
        Vector3 scale = character.transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        character.transform.localScale = scale;

        // 🔧 ล็อก Y ไม่ให้เดินเฉียง
        target.y = character.transform.position.y;

        // เดิน
        animator.SetTrigger("Run");
        yield return StartCoroutine(MoveToPosition(character, target, runSpeed));

        // รอหยุด
        yield return new WaitForSeconds(0.1f);

        // แสดงท่า
        TriggerAnimation(animator, trigger);

        if (fall)
        {
            yield return new WaitForSeconds(0.5f);

            // ตกลงด้านล่าง
            Vector3 fallTarget = character.transform.position + new Vector3(0f, -1.5f, 0f);
            float fallSpeed = 3f;
            while (Vector3.Distance(character.transform.position, fallTarget) > 0.05f)
            {
                character.transform.position = Vector3.MoveTowards(character.transform.position, fallTarget, fallSpeed * Time.deltaTime);
                yield return null;
            }

            Debug.Log("💦 ตกน้ำเรียบร้อย");

            // รออีกนิดแล้วเดินกลับจุดเดิม
            yield return new WaitForSeconds(0.5f);

            // กลับไปจุดเริ่มต้น
            yield return StartCoroutine(MoveToPosition(character, startPos, runSpeed));

            // หยุดเดิน
            TriggerAnimation(animator, "Idle");
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
            animator.ResetTrigger("Run");
            animator.ResetTrigger("Win");
            animator.ResetTrigger("Lose");
            animator.ResetTrigger("Idle");
            animator.SetTrigger(trigger);
            Debug.Log($"🎯 Triggered: {trigger}");
        }
    }
}
