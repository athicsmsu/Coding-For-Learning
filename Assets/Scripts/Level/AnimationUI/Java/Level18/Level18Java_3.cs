using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Level18Java_3 : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public string itemName;
        public GameObject prefab;
        [HideInInspector] public GameObject instance; // ตัวที่ถูกสร้างจริง
    }

    public List<ItemData> items;
    public Transform itemParent;
    public GameObject foodBagPrefab;
    public GameObject weaponBagPrefab;
    public Transform bagParent;
    public Text outputText;

    public float bagScaleMultiplier = 550f; // ขนาดกระเป๋าที่ปรับได้
    public float moveSpeed = 2f; // ความเร็วลอย

    private GameObject foodBag;
    private GameObject weaponBag;

    void Start()
    {
        SpawnAllItems();
        ShowBags();
    }

    public void Correct(string answer)
    {
        Debug.Log("Correct Answer");
        ShowOutput(answer);
        StartCoroutine(SendItemsToBag(true)); // ลบ item หลังเข้ากระเป๋า
    }

    public void Wrong(string answer)
    {
        Debug.Log("Wrong Answer");
        ShowOutput(answer);
        StartCoroutine(SendItemsToBag(false)); // กลับ item เดิม
    }

    private void ShowOutput(string answer)
    {
        if (outputText == null) return;

        // answer อาจเป็น string เช่น "Food : 1, Food : 2, Weapon : 6"
        // แปลงเป็นบรรทัดต่อบรรทัด
        string[] outputs = answer.Split(',');

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (var line in outputs)
        {
            sb.AppendLine(line.Trim());
        }

        outputText.text = sb.ToString();
    }


    private void SpawnAllItems()
    {
        foreach (var item in items)
        {
            if (item.prefab != null && item.instance == null)
            {
                item.instance = Instantiate(item.prefab, itemParent);
            }
        }
    }

    private void ShowBags()
    {
        if (foodBag != null) Destroy(foodBag);
        if (weaponBag != null) Destroy(weaponBag);

        foodBag = Instantiate(foodBagPrefab, bagParent);
        weaponBag = Instantiate(weaponBagPrefab, bagParent);

        foodBag.transform.localScale = Vector3.one * bagScaleMultiplier;
        weaponBag.transform.localScale = Vector3.one * bagScaleMultiplier;

        foodBag.transform.localPosition = new Vector3(-100f, 0f, 0f);
        weaponBag.transform.localPosition = new Vector3(100f, 0f, 0f);

        SpriteRenderer foodSR = foodBag.GetComponent<SpriteRenderer>();
        if (foodSR != null) foodSR.sortingOrder = 1;

        SpriteRenderer weaponSR = weaponBag.GetComponent<SpriteRenderer>();
        if (weaponSR != null) weaponSR.sortingOrder = 1;
    }

    // destroyAfter = true -> ตอบถูก, false -> ตอบผิด
    private IEnumerator SendItemsToBag(bool destroyAfter)
    {
        string[] outputs = outputText.text.Split('\n');

        foreach (string line in outputs)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(':');
            if (parts.Length != 2) continue;

            string bagType = parts[0].Trim();
            int itemIndex;
            if (!int.TryParse(parts[1].Trim(), out itemIndex)) continue;

            itemIndex -= 1; // Output เป็น 1-based

            if (itemIndex < 0 || itemIndex >= items.Count) continue;

            var item = items[itemIndex];
            if (item.instance == null) continue;

            Transform targetBag = bagType.Contains("Weapon") ? weaponBag.transform : foodBag.transform;

            Vector3 startPos = item.instance.transform.position  ; // เก็บตำแหน่งเดิม
            Vector3 startScale = item.instance.transform.localScale;

            yield return StartCoroutine(MoveToBag(item.instance, targetBag.position, destroyAfter));

            if (!destroyAfter)
            {
                // ตอบผิด → เอากลับตำแหน่งเดิม
                item.instance.transform.position = startPos;
                item.instance.transform.localScale = startScale;
            }
            else
            {
                item.instance = null;
            }
        }
    }

    private IEnumerator MoveToBag(GameObject item, Vector3 target, bool destroyAfter)
    {
        float time = 0f;
        Vector3 startPos = item.transform.position;
        Vector3 startScale = item.transform.localScale;
        Vector3 targetScale = destroyAfter ? Vector3.zero : startScale;

        while (time < 1f)
        {
            time += Time.deltaTime * moveSpeed;
            float t = Mathf.SmoothStep(0, 1, time);

            item.transform.position = Vector3.Lerp(startPos, target, t);
            item.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

        item.transform.position = target;
        item.transform.localScale = targetScale;

        if (destroyAfter)
            item.SetActive(false); // ปิดแทนลบ
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
}
