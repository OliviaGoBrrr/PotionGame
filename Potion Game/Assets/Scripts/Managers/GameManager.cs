using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public bool gameStarted = false;

    
    DialogueBubbleManager dialogueManager;
    SliderContainer sliders;
    [Header("GameObject Components")]
    [SerializeField] CauldronStats cauldron;
    [SerializeField] PotionCanvas potionCanvas;
    [SerializeField] AudioSource audioSource;

    [Header("Dialogue Components")]
    [SerializeField] DialogueScriptableObject beginning;
    [SerializeField] DialogueScriptableObject tutorial;
    [SerializeField] DialogueScriptableObject ending;
    [Tooltip("Place characters in here in order of appearance")]
    [SerializeField] List<CharacterScript> characters;
    int currentChar = 0;

    [Header("Audio Components")]
    [SerializeField] AudioClip characterEnter;
    [SerializeField] AudioClip potionPositive;
    [SerializeField] AudioClip potionNeutral;
    [SerializeField] AudioClip potionNegative;

    // Character Sections (Pulled later with function)
    List<DialogueScriptableObject> characterIntroduction;
    DialogueScriptableObject positiveConclusion;
    DialogueScriptableObject neutralConclusion;
    DialogueScriptableObject negativeConclusion;
    List<DialogueScriptableObject> idleDialogue;

    int tempFloor;
    int tempCeiling;
    int carbFloor;
    int carbCeiling;
    int pazazFloor;
    int pazazCeiling;
    float potencyFloor;

    [Header("Game Logic")]
    public int gameState = 0;
    bool stateWaiting = false;
    float timer = 0;
    float nextIdleDialogue = 0;
    bool dialogueEnded = false;
    bool potionSubmitted = false;
    int lastPotionScore = 0;

    void Awake()
    {
        dialogueManager = GameObject.FindWithTag("DialogueManager").GetComponent<DialogueBubbleManager>();
        sliders = GameObject.FindWithTag("SliderContainer").GetComponent<SliderContainer>();
    }
    public void PlayButtonClicked()
    {
        if (gameStarted == false)
        {
            gameStarted = true;
            timer = 0;
        }
    }
    public void DialogueEnded()
    {
        dialogueEnded = true;
    }
    public void PotionSubmitted()
    {
        potionSubmitted = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (gameStarted == true)
        {
            CurrentGameState();
        }
    }
    void CurrentGameState()
    {
        switch (gameState)
        {
            case 0: // Introduction Dialogue
                timer += Time.deltaTime;
                if (timer >= 2 && stateWaiting == false) 
                {
                    dialogueManager.SetDialogue(beginning);
                    stateWaiting = true;
                }
                if (dialogueEnded == true) // Once dialogue ends, go to the next gamestate
                {
                    ResetState(1);
                }
                break;
            case 1: // Character introduction
                timer += Time.deltaTime;
                if (timer >= 1 && potionSubmitted == false)
                {
                    PlaySound(characterEnter);
                    potionSubmitted = true;
                }
                if (timer >= 2 && stateWaiting == false) // After a two second wait, introduce the character
                {
                    
                    characters[currentChar].SetActive(true);
                    characters[currentChar].PullCharacterValues(out characterIntroduction, out positiveConclusion, out neutralConclusion, out negativeConclusion, out idleDialogue, out tempFloor, out tempCeiling, out carbFloor, out carbCeiling, out pazazFloor, out pazazCeiling, out potencyFloor);
                    if (characterIntroduction.Count == 1) { dialogueManager.SetDialogue(characterIntroduction[0]); }
                    else { dialogueManager.SetDialogue(characterIntroduction[lastPotionScore]); }
                    stateWaiting = true;
                }
                if (dialogueEnded == true) // Once dialogue ends, go to the next gamestate
                {
                    if (currentChar != 0)
                    {
                        ResetState(2);
                    }
                    else
                    {
                        ResetState(666);
                    }
                }
                break;
            case 2: // Potion crafting
                if (stateWaiting == false) // When the gamestate enters this phase
                {
                    float Width = 250;
                    sliders.UpdateGaugePositions((tempFloor + tempCeiling)/2, (tempCeiling - tempFloor) * Width/10 + 35, (carbFloor + carbCeiling)/2, (carbCeiling - carbFloor) * Width / 10 + 35, (pazazFloor + pazazCeiling)/2, (pazazCeiling - pazazFloor) * Width / 10 + 35);
                    ResetIdleText();
                    dialogueEnded = true;
                    stateWaiting = true;
                }
                if (dialogueEnded == true) // When there is no dialogue on screen
                {
                    timer += Time.deltaTime;
                    if (timer >= nextIdleDialogue)
                    {
                        ResetIdleText();
                        dialogueEnded = false;
                        if (idleDialogue.Count > 0)
                        {
                            int rngInt = Random.Range(0, idleDialogue.Count);
                            dialogueManager.SetDialogue(idleDialogue[rngInt]);
                            idleDialogue.Remove(idleDialogue[rngInt]);
                        }
                    }
                }
                if (potionSubmitted == true) // When the potion is submitted
                {
                    ResetState(3);
                }
                break;
            case 3:
                if (stateWaiting == false)
                {
                    potionCanvas.gameObject.SetActive(true);
                    potionCanvas.FadeIn(characters[currentChar].potionVisual);
                    stateWaiting = true;
                }
                if (potionSubmitted == true)
                {
                    int score = CalculateScore();
                    switch (score)
                    {
                        case 4:
                            PlaySound(potionPositive);
                            dialogueManager.SetDialogue(positiveConclusion);
                            characters[currentChar].SetSprite(1);
                            lastPotionScore = 0;
                            break;
                        case 3:
                            PlaySound(potionNeutral);
                            dialogueManager.SetDialogue(neutralConclusion);
                            characters[currentChar].SetSprite(2);
                            lastPotionScore = 1;
                            break;
                        default:
                            PlaySound(potionNegative);
                            dialogueManager.SetDialogue(negativeConclusion);
                            characters[currentChar].SetSprite(3);
                            lastPotionScore = 2;
                            break;
                    }
                    potionSubmitted = false;
                }
                if (dialogueEnded == true)
                {
                    characters[currentChar].SetActive(false);
                    currentChar += 1;
                    if (currentChar < characters.Count)
                    {
                        ResetState(1);
                    }
                    else
                    {
                        ResetState(4);
                    }
                }
                break;
            case 4:
                timer += Time.deltaTime;
                if (timer >= 2 && stateWaiting == false) // After a two second wait, play the conclusion
                {
                    dialogueManager.SetDialogue(ending);
                    stateWaiting = true;
                }
                if (dialogueEnded == true)
                {
                    // GAME END STUFF GOES HERE
                }
                break;
            case 666:
                timer += Time.deltaTime;
                if (timer >= 2 && stateWaiting == false) // After a two second wait, play the tutorial
                {
                    dialogueManager.SetDialogue(tutorial);
                    stateWaiting = true;
                }
                if (dialogueEnded == true)
                {
                    ResetState(2);
                }
                break;
        }
    }
    void ResetState(int i)
    {
        stateWaiting = false;
        dialogueEnded = false;
        potionSubmitted = false;
        timer = 0;
        gameState = i;
    }
    void ResetIdleText() // Resets the timer for idle text
    {
        timer = 0;
        nextIdleDialogue = Random.Range(20f, 30f);
    }
    int CalculateScore()
    {
        int tally = 0;
        cauldron.PullStats(out float Temp, out float Carb, out float Pazaz, out float Potency);
        if(Temp >= tempFloor &&  Temp <= tempCeiling) { tally += 1; }
        if (Carb >= carbFloor && Carb <= carbCeiling){ tally += 1; }
        if (Pazaz >=  pazazFloor && Pazaz <= pazazCeiling) { tally += 1; }
        if (Potency >= potencyFloor) { tally += 1; }
        return tally;
    }
    void PlaySound(AudioClip clip)
    {
        audioSource.resource = clip;
        audioSource.Play();
    }
}
