using UnityEngine;
using UnityEngine.Audio;

public class SliderController : MonoBehaviour
{
    // Components
    [SerializeField] AttributeSlider TempSlider;
    [SerializeField] AttributeSlider BubbleSlider;
    [SerializeField] AttributeSlider SheenSlider;

    [SerializeField] AudioClip soundFX;
    [SerializeField] AudioSource audioSource;

    [SerializeField] CauldronStats cauldron;

    // Notch Update Logic
    float NotchTimer;
    int CurrentUpdate = 0;

    // Pitch Update Logic
    float pitchOrigin;
    float pitchMod = 1;
    float pitchGoal;
    float pitchChange = 1.059463f; // How much the pitch changes each time

    // #Goals
    float TempGoal;
    float BubbleGoal;
    float SheenGoal;

    // Values for notches
    int TempNotch = 5;
    int BubbleNotch = 5;
    int SheenNotch = 5;

    // Old values
    int OldTemp;
    int OldBubble;
    int OldSheen;

    // Changes the guideline sliders (all at once). Call this BEFORE notches.
    public void SetNewPotionGuidelines(int TempLower, int TempUpper, int BubbleLower, int BubbleUpper, int SheenLower, int SheenUpper)
    {
        TempSlider.UpdateGuideline(TempLower, TempUpper);
        BubbleSlider.UpdateGuideline(BubbleLower, BubbleUpper);
        SheenSlider.UpdateGuideline(SheenLower, SheenUpper);
        TempGoal = (float)(TempLower + TempUpper) / 2;
        BubbleGoal = (float)(BubbleLower + BubbleUpper) / 2;
        SheenGoal = (float)(SheenLower + SheenUpper) / 2;
    }

    // Starts the process of changing the notches
    public void UpdateCauldronNotches(int TempValue, int BubbleValue, int SheenValue) // Immediately activates the first notch, others have to wait
    {
        NotchTimer = 1;
        CurrentUpdate = 1;
        pitchMod = 1;
        OldTemp = TempNotch;
        OldBubble = BubbleNotch;
        OldSheen = SheenNotch;
        TempNotch = TempValue;
        BubbleNotch = BubbleValue;
        SheenNotch = SheenValue;
        cauldron.PotencyReduction();
    }

    // Update is called once per frame
    void Update()
    {
        NotchTimer += Time.deltaTime;
        if (NotchTimer >= 1 && CurrentUpdate != 0) 
        {
            bool improvement;
            switch (CurrentUpdate)
            {
                case 1:
                    CurrentUpdate = 2;
                    NotchTimer = 0;
                    improvement = TempSlider.UpdateNotch(TempNotch, TempGoal);
                    cauldron.NewCauldronValues(TempNotch, OldBubble, OldSheen);
                    break;
                case 2:
                    CurrentUpdate = 3;
                    NotchTimer = 0;
                    improvement = BubbleSlider.UpdateNotch(BubbleNotch, BubbleGoal);
                    cauldron.NewCauldronValues(TempNotch, BubbleNotch, OldSheen);
                    break;
                case 3:
                    CurrentUpdate = 0;
                    improvement = SheenSlider.UpdateNotch(SheenNotch, SheenGoal);
                    cauldron.NewCauldronValues(TempNotch, BubbleNotch, SheenNotch);
                    break;
                default:
                    improvement = true;
                    break;
            }
            audioSource.Play();
            // Sets up pitch change variables
            if (improvement == true)
            {
                pitchOrigin = pitchMod;
                pitchGoal = pitchMod * pitchChange;
            }
            else
            {
                pitchOrigin = pitchMod;
                pitchGoal = pitchMod * (2 - pitchChange);
            }
        }
        // Alters the pitch over time
        if (pitchMod != pitchGoal)
        {
            pitchMod = Mathf.Lerp(pitchOrigin, pitchGoal, NotchTimer);
        }
        audioSource.pitch = pitchMod;
    }
}
