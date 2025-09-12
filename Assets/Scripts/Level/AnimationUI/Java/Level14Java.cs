using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;

public class Level14Java : MonoBehaviour
{
    public float stepSpeed = 5f;
    public float horizontalSpacing = 3f;
    public float verticalSpacing = 2f;

    // ศัตรูที่อยู่ใน Scene จริง ให้ลากมาใน Inspector หรือเก็บไว้ตอน Instantiate
    public GameObject[] enemiesInScene;

    public float gridSpacingX = 3f;
    public float gridSpacingY = 2f;
    public Vector2 gridOrigin = new Vector2(-7.139126f, -3.996701f);
    public Vector2 startGridPosition = new Vector2(0f, -1f);
    private Vector2Int currentGridIndex = new Vector2Int(0, -1);

    public void Correct(string answer, PlayerController player)
    {
        Debug.Log("✅ Correct\n" + answer);
        if (player == null || player.CurrentCharacter == null) return;

        StartCoroutine(MoveInOrder(player.CurrentCharacter, answer, "Win"));
    }

    public void Wrong(string answer, PlayerController player)
    {
        Debug.Log("❌ Wrong\n" + answer);
        if (player == null || player.CurrentCharacter == null) return;

        StartCoroutine(MoveInOrder(player.CurrentCharacter, answer, "Lose"));
    }

    private IEnumerator MoveInOrder(GameObject character, string answer, string finalTrigger)
    {
        Animator animator = character.GetComponent<Animator>();
        if (animator == null) yield break;

        Vector3 originalPosition = character.transform.position;
        startGridPosition = new Vector2(originalPosition.x, originalPosition.y);
        currentGridIndex = Vector2Int.zero;

        TriggerAnimation(animator, "Run");

        string[] lines = answer.Split('\n');
        foreach (string line in lines)
        {
            Vector2Int dir = Vector2Int.zero;
            int steps = 0;

            if (line.Contains("Walk Up")) { dir = Vector2Int.up; steps = ExtractDirectionValue(line, "Walk Up"); }
            else if (line.Contains("Walk Down")) { dir = Vector2Int.down; steps = ExtractDirectionValue(line, "Walk Down"); }
            else if (line.Contains("Walk Right")) { dir = Vector2Int.right; steps = ExtractDirectionValue(line, "Walk Right"); }
            else if (line.Contains("Walk Left")) { dir = Vector2Int.left; steps = ExtractDirectionValue(line, "Walk Left"); }

            if (steps > 0)
            {
                yield return MoveSteps(character, dir, steps);
            }
        }

        TriggerAnimation(animator, finalTrigger);

        if (finalTrigger == "Win")
{
    yield return new WaitForSeconds(2f);

    foreach (var enemy in enemiesInScene)
    {
        if (enemy != null)
        {
            Debug.Log($"💥 {enemy.name} หายไปหลังชนะ");
            enemy.SetActive(false); // ทำให้หายไป
        }
    }
}
else if (finalTrigger == "Lose")
{
    yield return new WaitForSeconds(0.8f);

    Vector3 startPosition = new Vector3(startGridPosition.x, startGridPosition.y, character.transform.position.z);

    while (Vector3.Distance(character.transform.position, startPosition) > 0.01f)
    {
        character.transform.position = Vector3.MoveTowards(character.transform.position, startPosition, stepSpeed * Time.deltaTime);
        yield return null;
    }

    TriggerAnimation(animator, "Idle");
}
    }
    
    private int gridWidth = 3;
private int gridHeight = 3;

    private IEnumerator MoveSteps(GameObject character, Vector2Int direction, int steps)
    {
        if (steps <= 0) yield break;

        for (int i = 0; i < steps; i++)
        {
            Vector2Int nextGridIndex = currentGridIndex + direction;

            // ✅ ไม่ให้เดินออกนอกกริด
            if (nextGridIndex.x < 0 || nextGridIndex.x >= gridWidth ||
                nextGridIndex.y < 0 || nextGridIndex.y >= gridHeight)
            {
                Debug.Log("🚫 ขอบตาราง: ตัวละครจะเดินออกนอกช่อง");
                yield break;
            }

            currentGridIndex = nextGridIndex;

            float xStep = direction.x * horizontalSpacing;
            float yStep = direction.y * verticalSpacing;
            Vector3 stepOffset = new Vector3(xStep, yStep, 0f);
            Vector3 targetPos = character.transform.position + stepOffset;

            FaceDirection2D(character.transform, direction);

            while (Vector3.Distance(character.transform.position, targetPos) > 0.01f)
            {
                character.transform.position = Vector3.MoveTowards(character.transform.position, targetPos, stepSpeed * Time.deltaTime);
                yield return null;
            }

            CheckIfSteppedOnEnemy(character);
        }
    }


    private void FaceDirection2D(Transform transform, Vector2 dir)
    {
        if (dir.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = dir.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private void TriggerAnimation(Animator animator, string trigger)
    {
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Win");
        animator.ResetTrigger("Lose");
        animator.ResetTrigger("Idle");
        animator.SetTrigger(trigger);
    }

    private int ExtractDirectionValue(string input, string direction)
    {
        Match match = Regex.Match(input, @$"{direction}\s*:\s*(\d+)");
        return match.Success ? int.Parse(match.Groups[1].Value) : 0;
    }

    private void CheckIfSteppedOnEnemy(GameObject character)
    {
        if (enemiesInScene == null || enemiesInScene.Length == 0) return;

        Vector2 characterPos2D = new Vector2(character.transform.position.x, character.transform.position.y);

        foreach (var enemy in enemiesInScene)
        {
            if (enemy == null) continue;

            Vector2 enemyPos2D = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
            float distance = Vector2.Distance(characterPos2D, enemyPos2D);

            if (distance < 1.0f)
            {
                Animator enemyAnimator = enemy.GetComponent<Animator>();
                if (enemyAnimator != null)
                {
                    Debug.Log($"⚔️ {enemy.name} โจมตี");
                    StartCoroutine(PlayAttackThenIdle(enemyAnimator));
                }
            }
        }
    }
    private IEnumerator PlayAttackThenIdle(Animator animator)
    {
        animator.ResetTrigger("Idle");
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.8f); // เล่นท่าโจมตี

        animator.ResetTrigger("Attack");
        animator.SetTrigger("Idle");
    }


}
