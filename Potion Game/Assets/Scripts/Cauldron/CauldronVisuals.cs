using UnityEngine;
using UnityEngine.Rendering;

public class CauldronVisuals : MonoBehaviour
{
    [SerializeField] GameObject cauldronOuter;
    [SerializeField] GameObject cauldronInner;
    [SerializeField] GameObject cauldronLiquid;

    public Color liquidColour;
    float liquidTimer = 0;

    [SerializeField] ParticleSystem hotParticle;
    [SerializeField] ParticleSystem coldParticle;
    [SerializeField] ParticleSystem bubblyParticle;
    [SerializeField] ParticleSystem flatParticle;
    [SerializeField] ParticleSystem glisteningParticle;
    [SerializeField] ParticleSystem lusterlessParticle;

    float shakeAmount = 0;
    bool cauldronShake = false;
    bool liquidShake = false;
    float cauldronTilt = 0;
    float liquidTilt = 0;

    bool shakeDirection; // true = positive shake, negative = negative shake

    // Update is called once per frame
    void Update()
    {
        if (shakeAmount >= 0)
        {
            ShakeShake();
        }
        ColourUpdate();
    }
    void ColourUpdate() // Updates the colour of the liquid
    {
        liquidTimer += Time.deltaTime;
        cauldronLiquid.GetComponent<SpriteRenderer>().color = Color.Lerp(cauldronLiquid.GetComponent<SpriteRenderer>().color, liquidColour, liquidTimer / 3);
    }
    void ShakeShake() // It's not done yettttt
    {
        shakeAmount -= Time.deltaTime;
        if (cauldronShake == false)
        {
            switch (shakeDirection)
            {
                case (true):
                    transform.LeanRotateZ(shakeAmount, 2f).setEase(LeanTweenType.easeOutQuart);
                break;
            }
        }
        
        cauldronOuter.transform.localRotation = Quaternion.Euler(0, 0, cauldronTilt);
        cauldronLiquid.transform.localRotation = Quaternion.Euler(0, 0, liquidTilt + cauldronTilt/10);
    }
    public void GetCauldronValues(float TempValue, float CarbValue, float PazazValue, float Alpha)
    {
        liquidColour = new Color(TempValue / 10, CarbValue / 10, PazazValue / 10, Alpha);
        liquidTimer = 0;
        var hotEmission = hotParticle.emission;
        hotEmission.rateOverTime = Mathf.Clamp(TempValue - 5, 0, 10);
        var coldEmission = coldParticle.emission;
        coldEmission.rateOverTime = Mathf.Clamp(5 - TempValue, 0, 10);
        var bubbleEmission = bubblyParticle.emission;
        bubbleEmission.rateOverTime = Mathf.Clamp(CarbValue - 5, 0, 10);
        var flatEmission = flatParticle.emission;
        flatEmission.rateOverTime = Mathf.Clamp(5 - CarbValue, 0, 10);
        var glisteningEmission = glisteningParticle.emission;
        glisteningEmission.rateOverTime = Mathf.Clamp(PazazValue - 5, 0, 10);
        var lusterlessEmission = lusterlessParticle.emission;
        lusterlessEmission.rateOverTime = Mathf.Clamp(5 - PazazValue, 0, 10);
    }
    public void FireBurst(int TempDiff, int CarbDiff, int PazazDiff)
    {
        if (TempDiff > 0)
        {
            hotParticle.Emit(Random.Range(4, 7) * TempDiff);
        }
        else if (TempDiff < 0)
        {
            coldParticle.Emit(Random.Range(-4, -7) * TempDiff);
        }
        if (CarbDiff > 0)
        {
            bubblyParticle.Emit(Random.Range(4, 7) * CarbDiff);
        }
        else if (CarbDiff < 0)
        { 
            flatParticle.Emit(Random.Range(-4, -7) * CarbDiff);
        }
        if (PazazDiff > 0)
        {
            glisteningParticle.Emit(Random.Range(4, 7) * PazazDiff);
        }
        else if (PazazDiff < 0)
        {
            lusterlessParticle.Emit(Random.Range(-4, -7) * PazazDiff);
        }
    }
    public void StartTheRock(float ShakeAmount, bool ShakeDirection)
    {
        if (shakeDirection == ShakeDirection)
        {
            shakeAmount += ShakeAmount;
        }
        else
        {
            if(shakeAmount >= ShakeAmount)
            {
                shakeAmount -= ShakeAmount;
            }
            else
            {
                shakeDirection = ShakeDirection;
                shakeAmount = ShakeAmount - shakeAmount;
            }
        }
    }
}
