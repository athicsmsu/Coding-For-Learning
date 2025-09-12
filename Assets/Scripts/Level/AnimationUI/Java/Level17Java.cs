using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level17Java : MonoBehaviour
{
    public Transform tile1Point;     // พื้นหมายเลข 1
    public Transform tile3Point;     // พื้นหมายเลข 3
    public Transform fallOffPoint;   // ตำแหน่งตกน้ำ (ถ้ามี หรือจะสร้างเอง)
    public float runSpeed = 2f;

    public void Correct(string answer, Text askText, PlayerController player)
    {
        Debug.Log("✅ Correct Level17Java");

        if (askText != null)
            askText.text = "3";

        if (player == null || player.CurrentCharacter == null)
            return;

        StartCoroutine(CorrectSequence(player));
    }

    public void Wrong(Text askText, PlayerController player)
    {
        Debug.Log("❌ Wrong Level17Java");

        if (askText != null)
            askText.text = "Lose!";

        if (player == null || player.CurrentCharacter == null)
            return;

        StartCoroutine(WrongSequence(player));
    }

private IEnumerator CorrectSequence(PlayerController player)
{
    GameObject character = player.CurrentCharacter;
    Animator animator = character.GetComponent<Animator>();

    Vector3 startPos = character.transform.position;

    // หันขวา
    Vector3 scale = character.transform.localScale;
    scale.x = Mathf.Abs(scale.x);
    character.transform.localScale = scale;

    // เดินไป tile1 (ล็อก y เริ่มต้นให้เดินแนวนอน)
    Vector3 tile1Pos = tile1Point.position;
    tile1Pos.y = startPos.y;

    animator.SetTrigger("Run");
    yield return StartCoroutine(MoveToPosition(character, tile1Pos, runSpeed));

    // หยุดเดินก่อนกระโดด
    animator.ResetTrigger("Run");
    TriggerAnimation(animator, "Idle");

    // กระโดดแบบเคลื่อนที่โค้งไป tile3
    yield return StartCoroutine(JumpMovement(character, tile1Pos, tile3Point.position, 3f, 1.0f));

    // หลังกระโดดเสร็จ ให้เคลื่อนที่แกน Y กลับมา start.y
    yield return StartCoroutine(MoveToYPosition(character, startPos.y, 1.0f));

    // ถึงแล้วทำท่า Win
    TriggerAnimation(animator, "Win");
}

// เคลื่อนที่แกน Y จากตำแหน่งปัจจุบันไป yTarget ระยะเวลา duration วินาที
private IEnumerator MoveToYPosition(GameObject obj, float yTarget, float duration)
{
    float elapsed = 0f;
    Vector3 startPos = obj.transform.position;

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        float newY = Mathf.Lerp(startPos.y, yTarget, t);
        obj.transform.position = new Vector3(startPos.x, newY, startPos.z);

        yield return null;
    }

    // แน่ใจว่าจบที่ yTarget เป๊ะๆ
    Vector3 finalPos = obj.transform.position;
    finalPos.y = yTarget;
    obj.transform.position = finalPos;
}


private IEnumerator JumpMovement(GameObject obj, Vector3 start, Vector3 target, float jumpHeight, float duration)
{
    float elapsed = 0f;

    // ล็อก target.y ให้เท่ากับ start.y เพื่อไม่ให้เคลื่อนขึ้นหรือลงเลย
    target.y = start.y;

    // คำนวณระยะทางแนวนอน
    float distance = Vector3.Distance(new Vector3(start.x, 0, start.z), new Vector3(target.x, 0, target.z));
    float maxJumpHeight = jumpHeight * Mathf.Min(1f, 1f / distance);

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        Vector3 horizontalPos = Vector3.Lerp(start, target, t);

        float height = 4 * maxJumpHeight * t * (1 - t);

        obj.transform.position = new Vector3(horizontalPos.x, start.y + height, horizontalPos.z);

        yield return null;
    }

    obj.transform.position = target;
}

    private IEnumerator WrongSequence(PlayerController player)
{
    GameObject character = player.CurrentCharacter;
    Animator animator = character.GetComponent<Animator>();

    Vector3 startPos = character.transform.position;

    // หันขวา
    Vector3 scale = character.transform.localScale;
    scale.x = Mathf.Abs(scale.x);
    character.transform.localScale = scale;

    // เดินเลย tile1 ไปขวา (อาจใช้ fallOffPoint เป็นตำแหน่งเดินผ่าน)
    Vector3 fallPos = fallOffPoint.position;
    fallPos.y = character.transform.position.y;

    TriggerAnimation(animator, "Run");
    yield return StartCoroutine(MoveToPosition(character, fallPos, runSpeed));

    yield return new WaitForSeconds(0.1f);

    TriggerAnimation(animator, "Lose");

    // ตกน้ำ ลงต่ำ
    Vector3 fallTarget = character.transform.position + new Vector3(0f, -1.5f, 0f);
    float fallSpeed = 3f;
    while (Vector3.Distance(character.transform.position, fallTarget) > 0.05f)
    {
        character.transform.position = Vector3.MoveTowards(character.transform.position, fallTarget, fallSpeed * Time.deltaTime);
        yield return null;
    }

    Debug.Log("💦 ตกน้ำเรียบร้อย");

    yield return new WaitForSeconds(0.5f);

    yield return StartCoroutine(MoveToPosition(character, startPos, runSpeed));

    TriggerAnimation(animator, "Idle");
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
