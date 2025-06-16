using UnityEngine;
using UnityEngine.UI;

public class AttributeSlider : MonoBehaviour
{
    // Components
    [SerializeField] Image Border;
    [SerializeField] Image Guideline;
    [SerializeField] Image Notch;

    // Modifiers for minimum guideline size and lerp speeds
    float minGuidelineSize = 20f; // Minimum size for a guideline (for when the lower and upper bounds are the same number)
    float posSpeedMod = 2f; // Speed modifier for position lerp
    float sizeSpeedMod = 2f; // Speed modifier for size lerp

    // Saved values for things such as calcs and lerping
    float Width;
    Vector3 GuidelinePos;
    Vector2 GuidelineSize;
    Vector3 NotchPos;

    // Saved value for checking for improvement
    int prevValue = 5;

    // Pulls the current position and size values for later use
    void Start()
    {
        Width = Border.GetComponent<RectTransform>().rect.width;
        GuidelinePos = Guideline.GetComponent<RectTransform>().localPosition;
        GuidelineSize = Guideline.GetComponent<RectTransform>().sizeDelta;
        NotchPos = Notch.GetComponent<RectTransform>().localPosition;
    }

    // Starts the process of moving the guidelines to their correct position
    public void UpdateGuideline(int lowerBound, int upperBound) // Lowest number should be 0, highest should be 10
    {
        float boundAvr = (float)(lowerBound + upperBound) / 2;
        float boundGap = upperBound - lowerBound;
        GuidelinePos = new Vector3(Width / 10 * boundAvr - Width / 2, 0, 0);
        GuidelineSize = new Vector2(boundGap * Width / 10 + 20, Guideline.GetComponent<RectTransform>().sizeDelta.y);
    }

    // Starts the process of moving the notches to their correct position. Additionally, returns whether or not this is considered an improvement.
    public bool UpdateNotch(int value, float goal)
    {
        bool improvement;
        if (Mathf.Abs(goal - value) <= Mathf.Abs(goal - prevValue)) // If the difference between the goal and the value is smaller or the same
        {
            improvement = true;
        }
        else
        {
            improvement = false;
        }
        NotchPos = new Vector3(Width / 10 * value - Width / 2, 0, 0);
        prevValue = value;
        return improvement;
    }

    // Moves guidelines and notches to their correct position using lerps
    void Update()
    {
        if (Guideline.GetComponent<RectTransform>().localPosition != GuidelinePos)
        {
            Guideline.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(Guideline.GetComponent<RectTransform>().localPosition.x, GuidelinePos.x, Time.deltaTime * posSpeedMod), 0, 0);
        }
        if (Guideline.GetComponent<RectTransform>().sizeDelta != GuidelineSize)
        {
            Guideline.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Lerp(Guideline.GetComponent<RectTransform>().sizeDelta.x, GuidelineSize.x, Time.deltaTime * sizeSpeedMod), Guideline.GetComponent<RectTransform>().sizeDelta.y);
        }
        if (Notch.GetComponent<RectTransform>().localPosition != NotchPos)
        {
            Notch.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(Notch.GetComponent<RectTransform>().localPosition.x, NotchPos.x, Time.deltaTime * posSpeedMod), 0, 0);
        }
    }
}
