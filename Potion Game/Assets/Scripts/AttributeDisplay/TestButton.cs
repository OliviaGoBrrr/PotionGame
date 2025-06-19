using UnityEngine;
using UnityEngine.EventSystems;

public class TestButton : MonoBehaviour, IPointerClickHandler
{
    public SliderController sliders;
    public int TempLower;
    public int TempUpper;
    public int BubbleLower;
    public int BubbleUpper;
    public int SheenLower;
    public int SheenUpper;

    public int TempNotch;
    public int BubbleNotch;
    public int SheenNotch;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) // Left click to test guideline
        {
            sliders.SetNewPotionGuidelines(TempLower, TempUpper, BubbleLower, BubbleUpper, SheenLower, SheenUpper);
        }
        if (eventData.button == PointerEventData.InputButton.Right) // Right click to test notch
        {
            sliders.UpdateCauldronNotches(TempNotch, BubbleNotch, SheenNotch);
        }
        
    }
}
