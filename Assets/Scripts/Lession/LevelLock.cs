using UnityEngine;

public class LevelLock : MonoBehaviour
{
    public GameObject NormalLock;
    public GameObject HardLock;
    public GameObject infoEasy;
    public GameObject infoNormal;
    public GameObject infoHard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        infoEasy.SetActive(false);
        infoNormal.SetActive(false);
        infoHard.SetActive(false);
        
        //แบ่งภาษา
        if (ChangeScenes.Language == "Java")
        {
            if (ChangeScenes.checkpoint <= 10)
            {
                NormalLock.gameObject.SetActive(true);
                HardLock.gameObject.SetActive(true);
            }
            else if (ChangeScenes.checkpoint <= 21)
            {
                NormalLock.gameObject.SetActive(false);
                HardLock.gameObject.SetActive(true);
            }
            else
            {
                NormalLock.gameObject.SetActive(false);
                HardLock.gameObject.SetActive(false);
            }
        }
        else if (ChangeScenes.Language == "Python")
        {
            if (ChangeScenes.checkpoint <= 9)
            {
                NormalLock.gameObject.SetActive(true);
                HardLock.gameObject.SetActive(true);
            }
            else if (ChangeScenes.checkpoint <= 20)
            {
                NormalLock.gameObject.SetActive(false);
                HardLock.gameObject.SetActive(true);
            }
            else
            {
                NormalLock.gameObject.SetActive(false);
                HardLock.gameObject.SetActive(false);
            }
        }

    }
}
