using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level10Python : MonoBehaviour
{
    public GameObject orc; // ตั้งค่า Orc prefab ใน Inspector
    public float runSpeed = 2f;

    private Animator orcAnimator;

    private void Awake()
    {
        if (orc != null)
            orcAnimator = orc.GetComponent<Animator>();
    }

    public void Correct(string answer, Text askText, PlayerController player)
    {
        if (askText != null)
            askText.text = answer;

        StartCoroutine(AttackSequence(player));
    }

    public void Wrong(Text askText, PlayerController player)
    {
        if (askText != null)
            askText.text = "Lose!";

        StartCoroutine(OrcAttackSequence(player));
    }

    private IEnumerator AttackSequence(PlayerController player)
    {
        GameObject mainChar = player.CurrentCharacter;
        if (mainChar == null) yield break;

        // หันขวา (แน่ใจว่าไม่ย่อขนาด)
        Vector3 originalScale = mainChar.transform.localScale;
        mainChar.transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        // ตัวหลักวิ่ง
        TriggerPlayerAnimation(player, "Run");
        Vector3 target = mainChar.transform.position + new Vector3(3f, 0f, 0f);
        yield return StartCoroutine(MoveToPosition(mainChar, target, runSpeed));

        // โจมตี
        TriggerPlayerAnimation(player, "Attack");
        Debug.Log("✅ Player Attack Triggered");
        yield return new WaitForSeconds(0.5f);

        // Orc ถูกตี
        TriggerOrcAnimation("Hurt");
        Debug.Log("✅ Orc Hurt Triggered");
        yield return new WaitForSeconds(0.5f);

        // ชนะ
        TriggerPlayerAnimation(player, "Win");
        Debug.Log("✅ Player Win Triggered");
        yield return new WaitForSeconds(0.5f);
        
        Destroy(orc);
    }

    private IEnumerator OrcAttackSequence(PlayerController player)
    {
        GameObject mainChar = player.CurrentCharacter;
        if (mainChar == null) yield break;

        Vector3 playerStartPos = mainChar.transform.position;
        Vector3 orcStartPos = orc.transform.position;

        // หันซ้าย
        Vector3 originalScale = mainChar.transform.localScale;
        mainChar.transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        // ตัวหลักวิ่งกลับ
        TriggerPlayerAnimation(player, "Run");
        Vector3 target = mainChar.transform.position + new Vector3(-1f, 0f, 0f);
        yield return StartCoroutine(MoveToPosition(mainChar, target, runSpeed));

        // Orc วิ่งตาม + หันซ้าย
        TriggerOrcAnimation("Run");
        orc.transform.localScale = new Vector3(-Mathf.Abs(orc.transform.localScale.x), orc.transform.localScale.y, orc.transform.localScale.z);
        Vector3 orcTarget = orc.transform.position + new Vector3(-3f, 0f, 0f);
        yield return StartCoroutine(MoveToPosition(orc, orcTarget, runSpeed));

        // Orc โจมตี
        TriggerOrcAnimation("Attack");
        Debug.Log("✅ Orc Attack Triggered");
        yield return new WaitForSeconds(0.5f);

        // ตัวหลักแพ้
        TriggerPlayerAnimation(player, "Lose");
        Debug.Log("✅ Player Lose Triggered");

        // รอให้ท่า lose เล่นสักพัก
        yield return new WaitForSeconds(1f);

        // ตัวหลักหันขวาก่อนวิ่งกลับ
        Vector3 playerScale = mainChar.transform.localScale;
        mainChar.transform.localScale = new Vector3(Mathf.Abs(playerScale.x), playerScale.y, playerScale.z);

        // ให้ตัวละครทั้งสองวิ่งกลับตำแหน่งเริ่มต้น
        TriggerPlayerAnimation(player, "Run");
        yield return StartCoroutine(MoveToPosition(mainChar, playerStartPos, runSpeed));

        TriggerOrcAnimation("Run");
        yield return StartCoroutine(MoveToPosition(orc, orcStartPos, runSpeed));

        // กลับมา Idle
        TriggerPlayerAnimation(player, "Idle");
        TriggerOrcAnimation("Idle");
    }


    private IEnumerator MoveToPosition(GameObject obj, Vector3 target, float speed)
    {
        while (Vector3.Distance(obj.transform.position, target) > 0.05f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }

    // ฟังก์ชัน Trigger Animation ของ PlayerController (ตัวละครหลัก)
    private void TriggerPlayerAnimation(PlayerController player, string trigger)
    {
        if (player != null && player.CurrentCharacter != null)
        {
            Animator animator = player.CurrentCharacter.GetComponent<Animator>();
            if (animator != null && animator.runtimeAnimatorController != null)
            {
                // ตัวละครหลักมี Trigger ที่ใช้บ่อยในนี้ (ปรับได้ตาม Animator ของตัวละคร)
                string[] validTriggers = { "Run", "Attack", "Win", "Lose", "Idle" };
                if (System.Array.Exists(validTriggers, t => t == trigger))
                {
                    ResetAllTriggers(animator);
                    animator.SetTrigger(trigger);
                    Debug.Log($"🎯 Trigger Player '{trigger}'");
                }
                else
                {
                    //Debug.LogWarning($"⚠️ Trigger '{trigger}' ไม่อยู่ในรายการของ Player");
                }
            }
            else
            {
                //Debug.LogWarning("⚠️ Animator ของ PlayerCharacter ไม่มีหรือไม่ถูกต้อง");
            }
        }
        else
        {
            //Debug.LogWarning("⚠️ Player หรือ CurrentCharacter เป็น null");
        }
    }

    // ฟังก์ชัน Trigger Animation ของ Orc
    private void TriggerOrcAnimation(string trigger)
    {
        if (orc != null && orcAnimator != null)
        {
            // Orc มี Trigger ที่ใช้บ่อยในนี้ (ปรับได้ตาม Animator ของ Orc)
            string[] validTriggers = { "Run", "Attack", "Hurt", "Idle" };
            if (System.Array.Exists(validTriggers, t => t == trigger))
            {
                ResetAllTriggers(orcAnimator);
                orcAnimator.SetTrigger(trigger);
                Debug.Log($"🎯 Trigger Orc '{trigger}'");
            }
            else
            {
                //Debug.LogWarning($"⚠️ Trigger '{trigger}' ไม่อยู่ในรายการของ Orc");
            }
        }
        else
        {
            //Debug.LogWarning("⚠️ Orc หรือ Animator เป็น null");
        }
    }

    // ฟังก์ชัน Reset Trigger ทั้งหมดที่ใช้บ่อย (เรียกใช้ก่อนตั้ง Trigger ใหม่)
    private void ResetAllTriggers(Animator animator)
    {
        animator.ResetTrigger("Win");
        animator.ResetTrigger("Lose");
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Hurt");
    }
}
