using UnityEngine;

public class TooltipSystems : MonoBehaviour
{
    private static TooltipSystems current;
    public Tooltip tooltip;

    public void Awake()
    {
        current = this;
    }

    public static void Show(string header, string content)
    { 
        current.tooltip.SetText(header, content);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
