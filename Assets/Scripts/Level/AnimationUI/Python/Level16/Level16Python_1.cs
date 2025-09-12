using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Level16Python_1 : MonoBehaviour
{
    [Header("Bag Settings")]
    public GameObject bagPrefab;
    public Transform bagSpawnPoint;
    public float spawnInterval = 0.2f;
    public float spreadX = 2f;
    public float spreadY = 2f;
    public float moveSpeed = 2f;

    [Header("Scale Multipliers")]
    public float bagScaleMultiplier = 550f;
    public float itemScaleMultiplier = 70f;

    [Header("Available Item Prefabs")]
    public List<GameObject> availableItemPrefabs;

    [Header("Default Item (ถ้าไม่พบชื่อ)")]
    public GameObject defaultItemPrefab;

    private Dictionary<string, GameObject> itemMap = new Dictionary<string, GameObject>();
    private GameObject bagInstance;

    private void Awake()
    {
        foreach (var prefab in availableItemPrefabs)
        {
            if (prefab != null)
            {
                string key = prefab.name.Trim().ToLower();
                if (!itemMap.ContainsKey(key))
                    itemMap.Add(key, prefab);
            }
        }
    }

    public void Correct(string answer)
    {
        StartCoroutine(SpawnBagItems(answer, true));
    }

    public void Wrong(string answer)
    {
        StartCoroutine(SpawnBagItems(answer, false));
    }

    private IEnumerator SpawnBagItems(string output, bool isCorrect)
    {
        if (bagPrefab == null || bagSpawnPoint == null || string.IsNullOrEmpty(output)) yield break;

        // สร้างกระเป๋าเพียงครั้งเดียว
        if (bagInstance == null)
        {
            bagInstance = Instantiate(bagPrefab, bagSpawnPoint.position, Quaternion.identity);
            bagInstance.transform.SetParent(bagSpawnPoint, false);
            bagInstance.transform.localPosition = Vector3.zero;
            bagInstance.transform.localScale = Vector3.one * bagScaleMultiplier;
            bagInstance.transform.SetAsLastSibling();

            SpriteRenderer sr = bagInstance.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingLayerName = "UI";
                sr.sortingOrder = 999;
            }
        }

        // เรียกลบกระเป๋า ถ้าเป็น Correct() ไม่ว่าจะสร้างใหม่หรือไม่
        if (isCorrect && bagInstance != null)
        {
            StartCoroutine(DestroyBagAfterTime(4f));
        }

        string[] lines = output.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] parts = line.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            string itemName = parts[parts.Length - 1].Trim().ToLower();

            GameObject itemPrefab = null;
            if (itemMap.ContainsKey(itemName))
                itemPrefab = itemMap[itemName];
            else if (defaultItemPrefab != null)
            {
                Debug.LogWarning($"Item Prefab not found: {itemName}. Using default.");
                itemPrefab = defaultItemPrefab;
            }

            if (itemPrefab != null)
            {
                GameObject item = Instantiate(itemPrefab, bagInstance.transform.position, Quaternion.identity);
                item.transform.SetParent(bagSpawnPoint, true);
                item.transform.SetAsLastSibling();

                SpriteRenderer srItem = item.GetComponent<SpriteRenderer>();
                if (srItem != null)
                {
                    srItem.sortingLayerName = "UI";
                    srItem.sortingOrder = 1000;
                }

                Vector3 targetPos = bagInstance.transform.position + new Vector3(
                    Random.Range(-spreadX, spreadX),
                    Random.Range(1f, spreadY),
                    0
                );

                item.transform.localScale = Vector3.one * itemScaleMultiplier;
                item.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));

                StartCoroutine(MoveItem(item, targetPos));

                // ลบไอเท็มหลัง 4 วิ
                StartCoroutine(DestroyAfterTime(item, 4f));
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator MoveItem(GameObject item, Vector3 targetPos)
    {
        float t = 0;
        Vector3 startPos = item.transform.position;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            item.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
    }

    private IEnumerator DestroyAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        if (obj != null)
            Destroy(obj);
    }

    private IEnumerator DestroyBagAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (bagInstance != null)
        {
            Destroy(bagInstance);
            bagInstance = null; // ให้สามารถสร้างใหม่ได้
        }
    }
}
