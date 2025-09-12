using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level23Python : MonoBehaviour
{
    public GameObject goodSwordPrefab;
    public GameObject brokenSwordPrefab;
    public GameObject bagPrefab; // เพิ่ม Bag
    public Transform swordTargetPoint;
    private Vector3 brokenSwordStartPos;
    private Animator bagAnimator;

    private void Start()
    {
        if (goodSwordPrefab != null) goodSwordPrefab.SetActive(false);

        if (brokenSwordPrefab != null)
        {
            brokenSwordStartPos = brokenSwordPrefab.transform.position;
            brokenSwordPrefab.SetActive(false);
        }

        if (bagPrefab != null)
        {
            bagPrefab.SetActive(false);
            bagAnimator = bagPrefab.GetComponent<Animator>();
            if (bagAnimator != null)
            {
                bagAnimator.updateMode = AnimatorUpdateMode.Normal; // ให้ animation ทำงานปกติ
                bagAnimator.Play("Bag2", 0, 0f); // ตั้งชื่อ animation ของ Bag ว่า "OpenBag"
                bagAnimator.speed = 0f; // เริ่มหยุด animation
            }
        }
    }

    public void Correct(string answer, Text askText, PlayerController player)
    {
        Debug.Log("✅ Correct Level25Java");

        if (askText != null)
            askText.text = answer;

        if (player != null)
            TriggerAnimation(player, "Win");

        StartCoroutine(ShowBagThenSword(goodSwordPrefab, false, player));
    }

    public void Wrong(Text askText, PlayerController player)
    {
        Debug.Log("❌ Wrong Level25Java");

        if (askText != null)
            askText.text = "Lose!";

        if (player != null)
            TriggerAnimation(player, "Lose");

        StartCoroutine(ShowBagThenSword(brokenSwordPrefab, true, player));
    }

    private IEnumerator ShowBagThenSword(GameObject sword, bool isWrong, PlayerController player)
{
    // แสดง Bag และเล่น animation 1 รอบ
    if (bagPrefab != null && bagAnimator != null)
    {
        bagPrefab.SetActive(true);
        bagAnimator.speed = 1f; // เล่น animation
        yield return new WaitForSeconds(bagAnimator.GetCurrentAnimatorStateInfo(0).length); // รอ animation จบ
        bagAnimator.speed = 0f; // หยุดที่ frame สุดท้าย
    }

    // แสดงดาบต่อ
    yield return StartCoroutine(MoveSwordToCharacter(sword, isWrong, player));

    // จัดการการซ่อน Bag
    if (bagPrefab != null)
    {
        if (isWrong)
        {
            // ตอบผิด หายไปทันที
            bagPrefab.SetActive(false);
        }
        else
        {
            // ตอบถูก รอ 2 วิแล้วหายไป
            yield return new WaitForSeconds(2.5f);
            bagPrefab.SetActive(false);
        }
    }
}


    private IEnumerator MoveSwordToCharacter(GameObject sword, bool isWrong, PlayerController player)
    {
        sword.SetActive(true);
        Vector3 start = sword.transform.position;
        Vector3 end = swordTargetPoint != null ? swordTargetPoint.position : new Vector3(-4f, -3.5f, 0);

        float duration = 1f;
        float time = 0f;

        while (time < duration)
        {
            sword.transform.position = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        sword.transform.position = end;

        if (isWrong)
        {
            yield return new WaitForSeconds(1f);

            // ดาบหายไปแล้วกลับตำแหน่งเดิม
            sword.SetActive(false);
            sword.transform.position = brokenSwordStartPos;

            // ตัวละครทำท่า idle
            if (player != null)
                TriggerAnimation(player, "Idle");
        }
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
}
