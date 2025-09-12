using UnityEngine;
using UnityEngine.UI;

public class OpenHintAndGoals : MonoBehaviour
{
    public GameObject ClickHintAndGoalsOpenTxt;
    public GameObject HitAndGoals;
    public bool isFisrtLevel = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isFisrtLevel == true)
        {
            ClickHintAndGoalsOpenTxt.gameObject.SetActive(true);
            HitAndGoals.gameObject.SetActive(false);
        }
        else{
            ClickHintAndGoalsOpenTxt.gameObject.SetActive(false);
            HitAndGoals.gameObject.SetActive(true);
        }
    }
    public void ClickOpenHintAndGoalsBtn()
    {
        if (ClickHintAndGoalsOpenTxt.activeSelf == false)
        {
            ClickHintAndGoalsOpenTxt.gameObject.SetActive(true);
            HitAndGoals.gameObject.SetActive(false);
        }
        else
        {
            ClickHintAndGoalsOpenTxt.gameObject.SetActive(false);
            HitAndGoals.gameObject.SetActive(true);
        }
    }
}
