using UnityEngine;

public class CauldronStats : MonoBehaviour
{

    public float currentTemperature;
    public float currentCarbonation;
    public float currentPazaz;
    public float currentPotency;
    [SerializeField] CauldronVisuals cauldronVisuals;


    private void Awake()
    {
        currentTemperature = 5;
        currentCarbonation = 5;
        currentPazaz = 5;
        currentPotency = 1;
        cauldronVisuals.GetCauldronValues(currentTemperature, currentCarbonation, currentPazaz, currentPotency);
    }
    public void PotencyReduction()
    {
        currentPotency -= 0.01f;
        cauldronVisuals.StartTheRock();
    }
    public void NewCauldronValues(int TempValue, int CarbValue, int PazazValue)
    {
        currentTemperature = TempValue;
        currentCarbonation = CarbValue;
        currentPazaz = PazazValue;
        
        cauldronVisuals.GetCauldronValues(currentTemperature, currentCarbonation, currentPazaz, currentPotency);
    }

}
