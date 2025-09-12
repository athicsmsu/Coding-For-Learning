using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Level18Java_2 : MonoBehaviour
{
    [Header("Bag Prefabs")]
    public GameObject minBagPrefab;
    public GameObject maxBagPrefab;
    public Transform bagParent;

    [Header("Coin Prefabs (ต้องมี SpriteRenderer)")]
    public GameObject goldCoinPrefab;
    public GameObject bronzeCoinPrefab;
    public GameObject silverCoinPrefab;

    [Header("Coin Sizes")]
    public float goldCoinScale = 1f;
    public float bronzeCoinScale = 1f;
    public float silverCoinScale = 1f;

    [Header("UI Output")]
    public Text outputText;

    [Header("Settings")]
    public float bagScaleMultiplier = 1f;
    public float spreadX = 2f;
    public float spreadY = 2f;
    public float moveSpeed = 2f;

    private GameObject minBag;
    private GameObject maxBag;

    void Start()
    {
        ShowBags();
    }

    private void ShowBags()
    {
        if (minBag != null) Destroy(minBag);
        if (maxBag != null) Destroy(maxBag);

        minBag = Instantiate(minBagPrefab, bagParent);
        maxBag = Instantiate(maxBagPrefab, bagParent);

        minBag.transform.localScale = Vector3.one * bagScaleMultiplier;
        maxBag.transform.localScale = Vector3.one * bagScaleMultiplier;

        minBag.transform.localPosition = new Vector3(-200f, 0f, 0f);
        maxBag.transform.localPosition = new Vector3(200f, 0f, 0f);

        SetSorting(minBag, 1);
        SetSorting(maxBag, 1);
    }

    private void SetSorting(GameObject obj, int order)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = order;
    }

    public void Correct(string answer)
    {
        ShowOutput(answer);
        SpawnCoinsFromAnswer(answer);
        StartCoroutine(HideBagsAfterDelay(5f)); // เริ่มนับถอยหลัง 5 วิ
    }

    public void Wrong(string answer)
    {
        ShowOutput(answer);
        SpawnCoinsFromAnswer(answer);
    }

    private void ShowOutput(string answer)
    {
        if (outputText == null) return;
        string[] lines = answer.Split(new char[] { '\n', ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (var line in lines)
        {
            sb.AppendLine(line.Trim());
        }
        outputText.text = sb.ToString();
    }
    private IEnumerator HideBagsAfterDelay(float delay)
    {
        float t = delay;
        while (t > 0f)
        {
            // สามารถอัปเดต UI นับถอยหลังตรงนี้ถ้าต้องการ
            t -= Time.deltaTime;
            yield return null;
        }

        // ซ่อนหรือทำลายกระเป๋า
        if (minBag != null) minBag.SetActive(false);
        if (maxBag != null) maxBag.SetActive(false);
    }


    private void SpawnCoinsFromAnswer(string answer)
    {
        int[] wallet;
        int choice1;
        ParseAnswerFlexible(answer, out wallet, out choice1);
        EvaluateResult(wallet, choice1);
    }

    private void ParseAnswerFlexible(string answer, out int[] wallet, out int choice1)
    {
        wallet = new int[0];
        choice1 = 0;
        if (string.IsNullOrEmpty(answer)) return;

        if (answer.Contains("|"))
        {
            string[] parts = answer.Split('|');
            if (parts.Length != 2) return;

            string[] nums = parts[0].Split(',');
            wallet = new int[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                int.TryParse(nums[i].Trim(), out wallet[i]);
            }
            int.TryParse(parts[1], out choice1);
        }
        else
        {
            List<int> nums = new List<int>();
            string[] lines = answer.Split('\n');
            foreach (var line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out int n))
                {
                    nums.Add(n);
                }
            }
            wallet = nums.ToArray();
            choice1 = wallet.Length;
        }
    }

    private void EvaluateResult(int[] wallet, int choice1)
    {
        if (wallet == null || wallet.Length == 0 || choice1 <= 0) return;

        int coinMax = wallet[0];
        int coinMin = wallet[0];

        for (int i = 0; i < choice1 && i < wallet.Length; i++)
        {
            if (wallet[i] > coinMax) coinMax = wallet[i];
            if (wallet[i] < coinMin) coinMin = wallet[i];
        }

        // กำหนดเหรียญสำหรับ Min
        GameObject minCoinPrefab;
        float minCoinScale;
        if (coinMin == 5)
        {
            minCoinPrefab = bronzeCoinPrefab;
            minCoinScale = bronzeCoinScale;
        }
        else
        {
            minCoinPrefab = silverCoinPrefab;
            minCoinScale = silverCoinScale;
        }

        // กำหนดเหรียญสำหรับ Max
        GameObject maxCoinPrefab;
        float maxCoinScale;
        if (coinMax == 20)
        {
            maxCoinPrefab = goldCoinPrefab;
            maxCoinScale = goldCoinScale;
        }
        else
        {
            maxCoinPrefab = silverCoinPrefab;
            maxCoinScale = silverCoinScale;
        }

        SpawnCoin(minBag.transform, minCoinPrefab, minCoinScale);
        SpawnCoin(maxBag.transform, maxCoinPrefab, maxCoinScale);
    }


    private void SpawnCoin(Transform bag, GameObject coinPrefab, float scale)
    {
        if (bag == null || coinPrefab == null) return;

        GameObject coin = Instantiate(coinPrefab, bag.position, Quaternion.identity, bagParent);
        coin.transform.localScale = Vector3.one * scale;

        SpriteRenderer sr = coin.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingLayerName = "UI";
            sr.sortingOrder = 10000; // ด้านหน้าสุด
        }

        Vector3 targetPos = bag.position + new Vector3(0f, spreadY, 0f); // เหรียญพุ่งตรงขึ้นไป spreadY หน่วย
        StartCoroutine(MoveItem(coin, targetPos));
        StartCoroutine(DestroyAfterTime(coin, 3f));
    }

    private IEnumerator MoveItem(GameObject item, Vector3 targetPos)
    {
        Vector3 startPos = item.transform.position;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            item.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        item.transform.position = targetPos;
    }

    private IEnumerator DestroyAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        if (obj != null) Destroy(obj);
    }
}
