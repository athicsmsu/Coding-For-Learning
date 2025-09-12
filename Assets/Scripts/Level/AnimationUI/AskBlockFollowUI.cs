using UnityEngine;

public class AskBlockFollow : MonoBehaviour
{
    public PlayerController player; // PlayerController ที่มีตัวละครหลัก

    public Vector3 offset = new Vector3(0, 7f, 0); // ระยะห่างจากตัวละคร

    void LateUpdate()
    {
        if (player != null && player.CurrentCharacter != null)
        {
            // ตามตัวละครหลักพร้อม offset
            transform.position = player.CurrentCharacter.transform.position + offset;
        }
    }
}
