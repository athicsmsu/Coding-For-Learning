using UnityEngine;

[System.Serializable]
public class Character 
{
    public GameObject characterPrefab;            // ตัวละครจริงที่มี Animator
    public RuntimeAnimatorController animatorController;   // Animator Controller เฉพาะของตัวละครนี้
}
