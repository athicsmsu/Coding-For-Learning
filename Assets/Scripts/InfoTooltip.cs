using UnityEngine;
using UnityEngine.EventSystems;

public class InfoTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject infoPanel; // drag มาจาก Inspector

    // ตอน Hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (infoPanel != null)
            infoPanel.SetActive(true);
    }

    // ตอนเลิก Hover
    public void OnPointerExit(PointerEventData eventData)
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }
}
