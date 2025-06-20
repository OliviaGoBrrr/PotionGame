using UnityEngine;
using UnityEngine.UI;

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

    }

    public void UpdateSliderStats(float temperature, float carbonation, float pazaz, float potency)
    {

        AddStats(tempSlider, temperature);

        AddStats(carbonSlider, carbonation);

        AddStats(pazazSlider, pazaz);
    }

    private void AddStats(Slider slider, float stat)
    {
        slider.value += stat;
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
