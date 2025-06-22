using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;

public class SliderContainer : MonoBehaviour
{
    [SerializeField] private GameObject temperatureSliderObject;
    [SerializeField] private GameObject carbonationSliderObject;
    [SerializeField] private GameObject pazazSliderObject;

    [SerializeField] private GameObject temperatureGaugeObject;
    [SerializeField] private GameObject carbonationGaugeObject;
    [SerializeField] private GameObject pazazGaugeObject;

    [SerializeField] private RectTransform tempGaugeTransform;
    [SerializeField] private RectTransform carbGaugeTransform;
    [SerializeField] private RectTransform pazazGaugeTransform;

    private Slider tempSlider;
    private Slider carbonSlider;
    private Slider pazazSlider;

    private Slider tempGauge;
    private Slider carbonGauge;
    private Slider pazazGauge;

    private float actualTempValue;
    private float actualCarbonValue;
    private float actualPazazValue;

    public float Width;

    public CauldronStats cauldronScript;

    [HideInInspector] public bool isClicked;
    private float clickedTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tempSlider = temperatureSliderObject.GetComponent<Slider>();
        carbonSlider = carbonationSliderObject.GetComponent<Slider>();
        pazazSlider = pazazSliderObject.GetComponent<Slider>();

        tempGauge = temperatureGaugeObject.GetComponent<Slider>();
        carbonGauge = carbonationGaugeObject.GetComponent<Slider>();
        pazazGauge = pazazGaugeObject.GetComponent<Slider>();

        actualTempValue = tempSlider.value;
        actualCarbonValue = carbonSlider.value;
        actualPazazValue = pazazSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            UpdateGaugePositions(0, 100, 5, 100, 10, 100);
        }

        if (isClicked == true)
        {
            clickedTimer += Time.deltaTime;
            if (clickedTimer >= 0.1f)
            {
                isClicked = false;
                clickedTimer = 0f;
            }
        }
    }
    
    public void UpdateSliderStats(float temperature, float carbonation, float pazaz, float potency, bool isHoney)
    {
        if (isClicked == true) return; // so no spamming
        isClicked = true;
        // sets values to what they should be so spamming doesnt affect the tweens

        if (isHoney == true)
        {
            actualTempValue = HoneyStats(actualTempValue);
            actualCarbonValue = HoneyStats(actualCarbonValue);
            actualPazazValue = HoneyStats(actualPazazValue);
        }
        else
        {
            actualTempValue += temperature;
            actualCarbonValue += carbonation;
            actualPazazValue += pazaz;
        }

        actualTempValue = Mathf.Clamp(actualTempValue, 0, 10);
        actualCarbonValue = Mathf.Clamp(actualCarbonValue, 0, 10);
        actualPazazValue = Mathf.Clamp(actualPazazValue, 0, 10);

        cauldronScript.IngredientEnters((int)actualTempValue, (int)actualCarbonValue, (int)actualPazazValue, (int)potency);

        Sequence sliderSequence = DOTween.Sequence();
        float timeElapsed = 0f;

        if (actualTempValue != tempSlider.value)
        {
            sliderSequence.Append(DOTween.To(() => tempSlider.value, x => tempSlider.value = x, actualTempValue, 0.3f).SetEase(Ease.OutSine));
            timeElapsed += 0.2f;
        }
        if (actualCarbonValue != carbonSlider.value)
        {
            sliderSequence.Insert(timeElapsed, DOTween.To(() => carbonSlider.value, x => carbonSlider.value = x, actualCarbonValue, 0.3f).SetEase(Ease.OutSine));
            timeElapsed += 0.2f;
        }
        if (actualPazazValue != pazazSlider.value)
        {
            sliderSequence.Insert(timeElapsed, DOTween.To(() => pazazSlider.value, x => pazazSlider.value = x, actualPazazValue, 0.3f).SetEase(Ease.OutSine));
        }
    }
    
    private void CheckIfStatIsMinMax(Slider slider, float total)
    {
        if (slider.value == 1 || slider.value == 10)
        {
            total = slider.value;
        }
    }
    

    private float HoneyStats(float total)
    {
        if (total > 5)
        {
            if (total <= 7)
            {
                total = 5;
            }
            else
            {
                total += -2;
            }
        }
        else if (total < 5)
        {
            if (total >= 3)
            {
                total = 5;
            }
            else
            {
                total += 2;
            }
        }

        return total;
    }


    public void UpdateGaugePositions(float tempPos, float tempSize, float carbPos, float carbSize, float pazPos, float pazSize)
    {
        tempGauge.value = tempPos;
        tempGaugeTransform.sizeDelta = new Vector2(tempSize, 0);


        carbonGauge.value = carbPos;
        carbGaugeTransform.sizeDelta = new Vector2(carbSize, 0);

        pazazGauge.value = pazPos;
        pazazGaugeTransform.sizeDelta = new Vector2(pazSize, 0);


    }
}
