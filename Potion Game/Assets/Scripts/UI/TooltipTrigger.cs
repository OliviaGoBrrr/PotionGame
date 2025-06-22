using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string content;
    public string header;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystems.Show(header, content);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystems.Hide();
    }
}
