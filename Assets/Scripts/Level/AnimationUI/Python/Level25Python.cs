using System.Collections;
using UnityEngine;

public class Level25Python : MonoBehaviour
{
    [System.Serializable]
    public class Animal
    {
        public GameObject obj;
        public float leftBound;   // ขอบซ้าย
        public float rightBound;  // ขอบขวา
        public AudioSource voice;
    }

    public Animal Zebra;
    public Animal Tiger;
    public Animal Lion;
    public Animal Elephant;
    public Animal Giraffe;
    public Animal Rhino;

    public float walkDelay = 2f;
    public float walkSpeed = 1f;

    private bool stopElephantLion = false;

    void Start()
    {
        StartCoroutine(AnimalsWalkLoop());
    }

    private IEnumerator AnimalsWalkLoop()
    {
        while (true)
        {
            MoveRandom(Zebra);
            MoveRandom(Tiger);

            if (!stopElephantLion)
            {
                MoveRandom(Elephant);
                MoveRandom(Lion);
            }

            MoveRandom(Giraffe);
            MoveRandom(Rhino);

            yield return new WaitForSeconds(walkDelay);
        }
    }

    private void MoveRandom(Animal animal)
    {
        if (animal != null && animal.obj != null)
        {
            StartCoroutine(WalkPingPong(animal));
        }
    }

    private IEnumerator WalkPingPong(Animal animal)
    {
        Animator anim = animal.obj.GetComponent<Animator>();
        if (anim != null) anim.SetTrigger("Front");

        // กำหนดทิศทางเริ่มต้น
        float dir = Random.value < 0.5f ? -1f : 1f;

        float elapsed = 0f;
        while (elapsed < walkDelay)
        {
            Vector3 pos = animal.obj.transform.position;
            pos.x += dir * walkSpeed * Time.deltaTime;

            // ถ้าเกินขอบซ้าย-ขวา ให้เปลี่ยนทิศทาง
            if (pos.x < animal.leftBound)
            {
                pos.x = animal.leftBound;
                dir = 1f;
            }
            else if (pos.x > animal.rightBound)
            {
                pos.x = animal.rightBound;
                dir = -1f;
            }

            animal.obj.transform.position = pos;

            // Flip ตัวสัตว์ตามทิศทาง
            Vector3 scale = animal.obj.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (dir >= 0 ? 1 : -1);
            animal.obj.transform.localScale = scale;

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (anim != null) anim.SetTrigger("Idle");
    }

    public void Correct(string answer)
    {
        Debug.Log("✅ Correct Java C27");

        stopElephantLion = true;
        StopAnimal(Elephant.obj);
        StopAnimal(Lion.obj);

        if (Elephant.voice != null) Elephant.voice.Play();

        // เรียก Coroutine เพื่อรอ 5 วินาทีแล้วปิดสัตว์ทั้งหมด
        StartCoroutine(DisableAllAnimalsAfterDelay(5f));
    }

    private IEnumerator DisableAllAnimalsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Animal[] allAnimals = { Zebra, Tiger, Lion, Elephant, Giraffe, Rhino };
        foreach (Animal a in allAnimals)
        {
            if (a.obj != null)
                a.obj.SetActive(false);
        }
    }


    public void Wrong(string answer)
    {
        Debug.Log("❌ Wrong Java C27");

        stopElephantLion = true;
        StopAnimal(Elephant.obj);
        StopAnimal(Lion.obj);

        Animal[] others = { Tiger, Zebra, Giraffe, Rhino };
        int index = Random.Range(0, others.Length);
        if (others[index].voice != null) others[index].voice.Play();
    }

    private void StopAnimal(GameObject obj)
    {
        if (obj != null)
        {
            Animator anim = obj.GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("Idle");
        }
    }
}
