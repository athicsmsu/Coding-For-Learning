using UnityEngine;
using UnityEngine.EventSystems;

public class LessonButton : MonoBehaviour
{
    public GameObject CanPlay;
    public GameObject CanPlayPoint;
    public GameObject CantPlay;

    private bool isPlayable = false;

    void Start()
    {
        AddHoverEffect();
    }

    public void SetStatus(bool canPlay)
    {
        isPlayable = canPlay;
        CanPlay.SetActive(canPlay);
        CanPlayPoint.SetActive(false);
        CantPlay.SetActive(!canPlay);
    }

    private void AddHoverEffect()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = gameObject.AddComponent<EventTrigger>();

        trigger.triggers.Clear();

        // PointerEnter
        EventTrigger.Entry entryEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entryEnter.callback.AddListener((eventData) =>
        {
            if (isPlayable && CanPlay.activeSelf)
            {
                Debug.Log($"[Hover] Enter: {gameObject.name}");
                CanPlay.SetActive(false);
                CanPlayPoint.SetActive(true);
            }
        });
        trigger.triggers.Add(entryEnter);

        // PointerExit
        EventTrigger.Entry entryExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        entryExit.callback.AddListener((eventData) =>
        {
            if (isPlayable && CanPlayPoint.activeSelf)
            {
                Debug.Log($"[Hover] Exit: {gameObject.name}");
                CanPlayPoint.SetActive(false);
                CanPlay.SetActive(true);
            }
        });
        trigger.triggers.Add(entryExit);
    }
}
