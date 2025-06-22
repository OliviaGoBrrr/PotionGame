using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    public float Width;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tempSlider = temperatureSliderObject.GetComponent<Slider>();
        carbonSlider = carbonationSliderObject.GetComponent<Slider>();
        pazazSlider = pazazSliderObject.GetComponent<Slider>();

        tempGauge = temperatureGaugeObject.GetComponent<Slider>();
        carbonGauge = carbonationGaugeObject.GetComponent<Slider>();
        pazazGauge = pazazGaugeObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            UpdateGaugePositions(0, 100, 5, 100, 10, 100);
        }
    }

    public void UpdateSliderStats(float temperature, float carbonation, float pazaz, float potency, bool isHoney)
    {

        float tempTotal = tempSlider.value;
        float carbTotal = carbonSlider.value;
        float pazTotal = pazazSlider.value;

        if (isHoney == false)
        {
            tempTotal = tempSlider.value + temperature;
            carbTotal = carbonSlider.value + carbonation;
            pazTotal = pazazSlider.value + pazaz;
        }
        else
        {
            tempTotal = HoneyStats(tempSlider, tempTotal);
            carbTotal = HoneyStats(carbonSlider, carbTotal);
            pazTotal = HoneyStats(pazazSlider, pazTotal);
        }
        Tween tempTween = DOTween.To(() => tempSlider.value, x => tempSlider.value = x, tempTotal, 0.4f).SetEase(Ease.OutSine);
        Tween carbTween = DOTween.To(() => carbonSlider.value, x => carbonSlider.value = x, carbTotal, 0.4f).SetEase(Ease.OutSine);
        Tween pazTween = DOTween.To(() => pazazSlider.value, x => pazazSlider.value = x, pazTotal, 0.4f).SetEase(Ease.OutSine);
    }

    private float HoneyStats(Slider slider, float total)
    {
        if (slider.value > 5)
        {
            if (slider.value <= 7)
            {
                total = 5;
            }
            else
            {
                total = slider.value - 2;
            }
        }
        else if (tempSlider.value < 5)
        {
            if (slider.value >= 3)
            {
                total = 5;
            }
            else
            {
                total = slider.value + 2;
            }
        }
        else
        {
            total = slider.value;
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
