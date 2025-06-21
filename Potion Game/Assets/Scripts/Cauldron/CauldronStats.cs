using UnityEngine;
using UnityEngine.UIElements;

public class CauldronStats : MonoBehaviour
{
    [Tooltip("Current temperature score from 0 to 10. 0 is cold and 10 is hot.")]
    public float currentTemperature;
    [Tooltip("Current carbination score from 0 to 10. 0 is flat and 10 is bubbly.")]
    public float currentCarbonation;
    [Tooltip("Current pazaz score from 0 to 10. 0 is lusterless and 10 is glistening.")]
    public float currentPazaz;
    [Tooltip("Current potency score from 0 to 1. 0 impotent and 1 is the starting value.")]
    public float currentPotency;
    [SerializeField] CauldronVisuals cauldronVisuals;

    private void Awake() // Sets initial cauldron stats
    {
        currentTemperature = 5;
        currentCarbonation = 5;
        currentPazaz = 5;
        currentPotency = 1;
        cauldronVisuals.GetCauldronValues(currentTemperature, currentCarbonation, currentPazaz, currentPotency);
    }
    public void IngredientEnters(int TempChange, int CarbChange, int PazazChange) // Call when ingredient enters the cauldron
    {
        cauldronVisuals.StartTheRock(10, true);
        cauldronVisuals.FireBurst(TempChange, CarbChange, PazazChange);
        currentPotency -= 0.01f;
        currentTemperature = Mathf.Clamp(currentTemperature + TempChange, 0, 10);
        currentCarbonation = Mathf.Clamp(currentCarbonation + CarbChange, 0, 10);
        currentPazaz = Mathf.Clamp(currentPazaz + PazazChange, 0, 10);
        cauldronVisuals.GetCauldronValues(currentTemperature, currentCarbonation, currentPazaz, currentPotency);
        cauldronVisuals.StartTheRock(20, true);
    }
    public void PullStats(out float Temp, out float Carb, out float Pazaz, out float Potency)
    {
        Temp = currentTemperature;
        Carb = currentCarbonation;
        Pazaz = currentPazaz;
        Potency = currentPotency;
    }
}
