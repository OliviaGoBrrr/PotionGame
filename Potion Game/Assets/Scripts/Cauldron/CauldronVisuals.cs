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

    float shakeAmount = 5;
    float shakeAmount2 = -5;
    int shakeState1 = 0;
    int shakeState2 = 0;
    float cauldronTilt = 0;
    float liquidTilt = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShakeShake();
        ColourUpdate();
    }
    void ColourUpdate()
    {
        liquidTimer += Time.deltaTime;
        cauldronLiquid.GetComponent<SpriteRenderer>().color = Color.Lerp(cauldronLiquid.GetComponent<SpriteRenderer>().color, liquidColour, liquidTimer / 3);
    }
    void ShakeShake()
    {
        switch (shakeState1)
        {
            case 1:
                cauldronTilt = Mathf.Lerp(cauldronTilt, shakeAmount + 1, Time.deltaTime * 5);
                if (cauldronTilt >= shakeAmount)
                {
                    shakeState1 = 2;
                }
                break;
            case 2:
                cauldronTilt = Mathf.Lerp(cauldronTilt, shakeAmount2 - 1, Time.deltaTime * 5);
                if ( cauldronTilt <= shakeAmount2 )
                {
                    shakeState1 = 3;
                }
            break;
            case 3:
                cauldronTilt = Mathf.Lerp(cauldronTilt, 0 + 1, Time.deltaTime * 5);
                if (cauldronTilt >= 0)
                {
                    cauldronTilt = 0;
                    shakeState1 = 0;
                }
                break;
            default:
                break;
        }
        switch (shakeState2)
        {
            case 1:
                liquidTilt = Mathf.Lerp(liquidTilt, shakeAmount + 0.5f, Time.deltaTime * 4f);
                if (liquidTilt >= shakeAmount)
                {
                    shakeState2 = 2;
                }
                break;
            case 2:
                liquidTilt = Mathf.Lerp(liquidTilt, shakeAmount2 - 0.5f, Time.deltaTime * 4f);
                if (liquidTilt <= shakeAmount2)
                {
                    shakeState2 = 3;
                }
                break;
            case 3:
                liquidTilt = Mathf.Lerp(liquidTilt, shakeAmount/2 + 0.5f, Time.deltaTime * 2f);
                if (liquidTilt >= shakeAmount/2)
                {
                    shakeState2 = 4;
                }
                break;
            case 4:
                liquidTilt = Mathf.Lerp(liquidTilt, shakeAmount2/2 - 0.5f, Time.deltaTime * 2f);
                if (liquidTilt <= shakeAmount2/2)
                {
                    shakeState2 = 5;
                }
                break;
            case 5:
                liquidTilt = Mathf.Lerp(liquidTilt, 0 + 0.5f, Time.deltaTime * 1f);
                if (liquidTilt >= 0)
                {
                    liquidTilt = 0;
                    shakeState2 = 0;
                }
                break;
            default:
                break;
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
    public void StartTheRock()
    {
        shakeState1 = 1;
        shakeState2 = 1;
    }
}
