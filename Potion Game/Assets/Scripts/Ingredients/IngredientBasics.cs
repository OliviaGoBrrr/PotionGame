using UnityEngine;

public class IngredientBasics : MonoBehaviour
{
    public int temperature;
    public int carbonation;
    public int pazaz;
    public int potency;

    private SliderContainer sliders;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sliders = GameObject.FindWithTag("SliderContainer").GetComponent<SliderContainer>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckBounds();
 
    }

    private void CheckBounds()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.x > 1.1 || pos.x < -0.1)
        {
            Destroy(this.gameObject);
        }

        if (pos.y > 3 || pos.y < -0.1)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Cauldron")
        {
            var cauldronScript = collider.gameObject.GetComponent<CauldronStats>();
            cauldronScript.currentTemperature += temperature;
            cauldronScript.currentCarbonation += carbonation;
            cauldronScript.currentPazaz += pazaz;
            cauldronScript.currentPotency += potency;

            sliders.UpdateSliderStats(temperature, carbonation, pazaz, potency);
        }
    }
}
