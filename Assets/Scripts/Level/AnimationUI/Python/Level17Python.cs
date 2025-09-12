using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level17Python : MonoBehaviour
{
    public GameObject rabbit; // ตั้งค่า Rabbit prefab ใน Inspector
    public float runSpeed = 2f;

    private Animator rabbitAnimator;

    private void Awake()
    {
        if (rabbit != null)
            rabbitAnimator = rabbit.GetComponent<Animator>();
    }

    public void Correct(string answer, Text askText, PlayerController player)
    {
        if (askText != null)
            askText.text = answer;

        StartCoroutine(CorrectSequence(player));
    }

    public void Wrong(string answer, Text askText, PlayerController player)
    {
        Debug.Log("❌ Wrong called with answer: " + answer);

        if (askText != null)
            askText.text = "Lose!";

        string trimmedAnswer = answer.Trim().ToLower();

        if (trimmedAnswer == "attack rabbit")
        {
            StartCoroutine(AttackRabbitSequence(player));
        }
        else if (trimmedAnswer == "run")
        {
            StartCoroutine(RunOnlySequence(player));
        }
        else
        {
            StartCoroutine(PlayLoseThenWait(player));
        }
    }
    private IEnumerator PlayLoseThenWait(PlayerController player)
    {
        TriggerPlayerAnimation(player, "Lose");
        yield return new WaitForSeconds(3f);
        TriggerPlayerAnimation(player, "Idle");
    }


    private IEnumerator CorrectSequence(PlayerController player)
    {
        GameObject mainChar = player.CurrentCharacter;
        if (mainChar == null) yield break;

        Vector3 startPos = mainChar.transform.position;
        Vector3 rabbitStartPos = rabbit.transform.position;

        // ตัวละครทำท่า Win
        TriggerPlayerAnimation(player, "Win");

        // กระต่ายทำท่า Idle
        TriggerRabbitAnimation("Idle");

        yield return new WaitForSeconds(4.5f);

        Destroy(rabbit);
    }

    private IEnumerator AttackRabbitSequence(PlayerController player)
    {
        GameObject mainChar = player.CurrentCharacter;
        if (mainChar == null || rabbit == null) yield break;

        Vector3 startPos = mainChar.transform.position;
        Vector3 rabbitStartPos = rabbit.transform.position;

        // 🧍‍♂️ ตัวละครหันขวาและทำท่าโจมตี
        mainChar.transform.localScale = new Vector3(Mathf.Abs(mainChar.transform.localScale.x), mainChar.transform.localScale.y, mainChar.transform.localScale.z);
        TriggerPlayerAnimation(player, "Attack");

        yield return new WaitForSeconds(0.3f); // รอจังหวะก่อนกระต่ายหนี

        // 🐰 กระต่ายทำท่า Run และวิ่งไปขวา
        TriggerRabbitAnimation("Run");
        rabbit.transform.localScale = new Vector3(Mathf.Abs(rabbit.transform.localScale.x), rabbit.transform.localScale.y, rabbit.transform.localScale.z); // หันขวา

        Vector3 rabbitEscapeTarget = rabbit.transform.position + new Vector3(2f, 0f, 0f); // ขยับไปทางขวา
        yield return StartCoroutine(MoveToPosition(rabbit, rabbitEscapeTarget, runSpeed));

        yield return new WaitForSeconds(0.5f); // หยุดพัก

        // 🐰 กลับมาที่เดิม และหันหน้าซ้าย
        TriggerRabbitAnimation("Run");
        rabbit.transform.localScale = new Vector3(-Mathf.Abs(rabbit.transform.localScale.x), rabbit.transform.localScale.y, rabbit.transform.localScale.z); // หันซ้าย
        yield return StartCoroutine(MoveToPosition(rabbit, rabbitStartPos, runSpeed));

        TriggerRabbitAnimation("Idle");

        // 🧍‍♂️ ตัวละครกลับตำแหน่งเดิม
        TriggerPlayerAnimation(player, "Run");
        yield return StartCoroutine(MoveToPosition(mainChar, startPos, runSpeed));
        TriggerPlayerAnimation(player, "Idle");
    }


    private IEnumerator RunOnlySequence(PlayerController player)
    {
        GameObject mainChar = player.CurrentCharacter;
        if (mainChar == null) yield break;

        Vector3 startPos = mainChar.transform.position;
        Vector3 rabbitStartPos = rabbit.transform.position;

        // 🧍‍♂️ ตัวละครหันซ้าย
        mainChar.transform.localScale = new Vector3(-Mathf.Abs(mainChar.transform.localScale.x), mainChar.transform.localScale.y, mainChar.transform.localScale.z);
        TriggerPlayerAnimation(player, "Run");

        // 🎯 วิ่งไปทางซ้าย
        Vector3 runTarget = startPos + new Vector3(-2f, 0f, 0f);
        yield return StartCoroutine(MoveToPosition(mainChar, runTarget, runSpeed));

        // 🐰 กระต่ายอยู่นิ่ง
        TriggerRabbitAnimation("Idle");

        yield return new WaitForSeconds(0.5f);

        // ⏪ กลับตำแหน่งเดิม (หันขวาก่อน)
        mainChar.transform.localScale = new Vector3(Mathf.Abs(mainChar.transform.localScale.x), mainChar.transform.localScale.y, mainChar.transform.localScale.z);
        TriggerPlayerAnimation(player, "Run");
        yield return StartCoroutine(MoveToPosition(mainChar, startPos, runSpeed));

        TriggerPlayerAnimation(player, "Idle");
    }


    private IEnumerator MoveToPosition(GameObject obj, Vector3 target, float speed)
    {
        while (Vector3.Distance(obj.transform.position, target) > 0.05f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void TriggerPlayerAnimation(PlayerController player, string trigger)
    {
        if (player != null && player.CurrentCharacter != null)
        {
            Animator animator = player.CurrentCharacter.GetComponent<Animator>();
            if (animator != null && animator.runtimeAnimatorController != null)
            {
                string[] validTriggers = { "Run", "Attack", "Win", "Lose", "Idle" };
                if (System.Array.Exists(validTriggers, t => t == trigger))
                {
                    ResetAllTriggers(animator);
                    animator.SetTrigger(trigger);
                    Debug.Log($"🎯 Trigger Player '{trigger}'");
                }
            }
        }
    }

    private void TriggerRabbitAnimation(string trigger)
    {
        if (rabbit != null && rabbitAnimator != null)
        {
            string[] validTriggers = { "Run", "Attack", "Hurt", "Idle" };
            if (System.Array.Exists(validTriggers, t => t == trigger))
            {
                ResetAllTriggers(rabbitAnimator);
                rabbitAnimator.SetTrigger(trigger);
                Debug.Log($"🎯 Trigger Rabbit '{trigger}'");
            }
        }
    }

    private void ResetAllTriggers(Animator animator)
    {
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Win");
        animator.ResetTrigger("Lose");
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Hurt");
    }
}
