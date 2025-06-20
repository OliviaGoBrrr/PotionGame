using UnityEngine;
using UnityEngine.UI;

public class SliderContainer : MonoBehaviour
{
    [SerializeField] private GameObject temperatureObject;
    [SerializeField] private GameObject carbonationObject;
    [SerializeField] private GameObject pazazObject;

    private Slider tempSlider;
    private Slider carbonSlider;
    private Slider pazazSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tempSlider = temperatureObject.GetComponent<Slider>();
        carbonSlider = carbonationObject.GetComponent<Slider>();
        pazazSlider = pazazObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSliderStats(int temperature, int carbonation, int pazaz, int potency)
    {
        AddStats(tempSlider, temperature);

        AddStats(carbonSlider, carbonation);

        AddStats(pazazSlider, pazaz);
    }

    private void AddStats(Slider slider, int stat)
    {
        slider.value += stat;
    }
}
