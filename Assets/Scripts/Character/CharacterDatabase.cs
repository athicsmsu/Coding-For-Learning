using UnityEngine;

[CreateAssetMenu]
public class CharacterDatabase : ScriptableObject
{
    public Character[] characters;
    public int CharacterCount
    {
        get { return characters.Length; }
    }
    public Character GetCharacter(int index)
    {
        if (index < 0 || index >= characters.Length)
        {
            Debug.LogError("Index out of range");
            return null;
        }
        return characters[index];
    }
}

