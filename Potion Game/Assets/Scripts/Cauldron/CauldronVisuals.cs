using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Collections;

public class CauldronVisuals : MonoBehaviour
{
    [SerializeField] GameObject cauldronShell;
    [SerializeField] GameObject cauldronLiquid;

    public Color liquidColour;
    float liquidTimer = 0;

    [SerializeField] ParticleSystem hotParticle;
    [SerializeField] ParticleSystem coldParticle;
    [SerializeField] ParticleSystem bubblyParticle;
    [SerializeField] ParticleSystem flatParticle;
    [SerializeField] ParticleSystem glisteningParticle;
    [SerializeField] ParticleSystem lusterlessParticle;

    Coroutine Shake;
    // Update is called once per frame
    void Update()
    {
        ColourUpdate();
    }
    void ColourUpdate() // Updates the colour of the liquid
    {
        liquidTimer += Time.deltaTime;
        cauldronLiquid.GetComponent<SpriteRenderer>().color = Color.Lerp(cauldronLiquid.GetComponent<SpriteRenderer>().color, liquidColour, liquidTimer / 3);
    }
    IEnumerator InitializeShake(float ShakeAmount, bool ShakeDirection, bool cameraPan)
    {
        LeanTween.cancel(cauldronShell);
        LeanTween.cancel(cauldronLiquid);
        List<float> amounts = new List<float>();
        while (ShakeAmount >= 0.2f)
        {
            amounts.Add(ShakeAmount);
            ShakeAmount /= 2;
        }
        for (int i = 0; i < amounts.Count; i++)
        {
            float shakeDur = amounts[i] / 30 + 0.1f;
            if (cameraPan == true)
            {
                if (ShakeDirection == true)
                {
                    cauldronShell.transform.LeanRotateZ(amounts[i], shakeDur).setEase(LeanTweenType.easeOutCirc);
                    yield return new WaitForSeconds(0.1f);
                    cauldronLiquid.transform.LeanRotateZ(amounts[i] * 0.8f, shakeDur).setEase(LeanTweenType.easeOutCirc);
                    yield return new WaitForSeconds(shakeDur - 0.1f);
                    ShakeDirection = false;
                }
                else if (ShakeDirection == false)
                {
                    cauldronShell.transform.LeanRotateZ(amounts[i] * -1, shakeDur).setEase(LeanTweenType.easeOutCirc);
                    yield return new WaitForSeconds(0.1f);
                    cauldronLiquid.transform.LeanRotateZ(amounts[i] * -0.8f, shakeDur).setEase(LeanTweenType.easeOutCirc);
                    yield return new WaitForSeconds(shakeDur - 0.1f);
                    ShakeDirection = true;
                }
                cameraPan = false;
            }
            else
            {
                if (ShakeDirection == true)
                {
                    cauldronShell.transform.LeanRotateZ(amounts[i], shakeDur).setEase(LeanTweenType.easeInOutSine);
                    yield return new WaitForSeconds(0.1f);
                    cauldronLiquid.transform.LeanRotateZ(amounts[i] * 0.8f, shakeDur).setEase(LeanTweenType.easeInOutSine);
                    yield return new WaitForSeconds(shakeDur - 0.1f);
                    ShakeDirection = false;
                }
                else if (ShakeDirection == false)
                {
                    cauldronShell.transform.LeanRotateZ(amounts[i] * -1, shakeDur).setEase(LeanTweenType.easeInOutSine);
                    yield return new WaitForSeconds(0.1f);
                    cauldronLiquid.transform.LeanRotateZ(amounts[i] * -0.8f, shakeDur).setEase(LeanTweenType.easeInOutSine);
                    yield return new WaitForSeconds(shakeDur - 0.1f);
                    ShakeDirection = true;
                }
            }
        }
        cauldronShell.transform.LeanRotateZ(0, 1f).setEase(LeanTweenType.easeOutQuart);
        cauldronLiquid.transform.LeanRotateZ(0, 1f).setEase(LeanTweenType.easeOutQuart);
        yield return null;
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
        if (Shake != null) { StopCoroutine(Shake); }
        Shake = StartCoroutine(InitializeShake(ShakeAmount, ShakeDirection, true));
    }
}
