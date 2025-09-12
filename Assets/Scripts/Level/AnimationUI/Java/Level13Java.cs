using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level13Java : MonoBehaviour
{
    public Sprite[] shieldSprites;             // ✅ ใส่รูปโล่ใน Inspector
    public Transform shieldParent;          // ✅ วางไว้ใน Canvas
    public float spacing = 60f;             // ✅ ระยะห่างระหว่างโล่ (Pixel)

    private PlayerController player;

    public void Correct(string answer, Text askText, PlayerController player)
    {
        this.player = player;

        int shieldCount = CountShields(answer);

        if (askText != null)
            askText.text = shieldCount.ToString();

        ClearShields();
        StartCoroutine(SpawnShieldsAndTrigger(shieldCount, "Win"));
    }

    public void Wrong(string answer, Text askText, PlayerController player)
    {
        this.player = player;
       
        int shieldCount = CountShields(answer);

        if (askText != null)
            if (string.IsNullOrEmpty(answer) || shieldCount == 0)
            {
                askText.text = "Lose!";
            }
            else
            {
                askText.text = shieldCount.ToString();
            }

        ClearShields();
        StartCoroutine(SpawnShieldsAndTrigger(shieldCount, "Lose"));
    }

    private int CountShields(string answer)
    {
        if (string.IsNullOrEmpty(answer)) return 0;

        string[] lines = answer.Split('\n');
        int count = 0;

        foreach (string line in lines)
        {
            if (line.Trim().StartsWith("Shield"))
                count++;
        }

        return count;
    }

    private void ClearShields()
    {
        foreach (Transform child in shieldParent)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator SpawnShieldsAndTrigger(int count, string trigger)
{
    for (int i = 0; i < count; i++)
    {
        GameObject shieldGO = new GameObject("ShieldImage");
        shieldGO.transform.SetParent(shieldParent, false);

        RectTransform rt = shieldGO.AddComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(i * spacing, 0f);
        rt.sizeDelta = new Vector2(100f, 100f);

        Image image = shieldGO.AddComponent<Image>();

        if (shieldSprites != null && shieldSprites.Length > 0)
        {
            // เลือกแบบเรียงลำดับ (วนซ้ำถ้าโล่เยอะกว่า sprite มี)
            int spriteIndex = i % shieldSprites.Length;
            image.sprite = shieldSprites[spriteIndex];
        }

        image.preserveAspect = true;

        yield return new WaitForSeconds(0.05f);
    }

    yield return new WaitForSeconds(0.2f);
    TriggerPlayerAnimation(trigger);
}


    private void TriggerPlayerAnimation(string trigger)
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
                Debug.Log($"🎯 Trigger: {trigger}");
            }
        }
    }
}
