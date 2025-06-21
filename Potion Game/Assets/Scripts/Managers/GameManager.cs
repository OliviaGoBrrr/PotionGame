using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public bool gameStarted = false;
    
    DialogueBubbleManager dialogueManager;

    // Dialogue Components
    [SerializeField] DialogueScriptableObject introduction;
    [Tooltip("Place characters in here in order of appearance")]
    [SerializeField] List<CharacterScript> characters;
    int currentChar = 0;

    // Character Sections (Pulled later with function)
    DialogueScriptableObject characterIntroduction;
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

    // Game State Logic
    int gameState = 0;
    bool stateWaiting = false;
    float timer = 0;
    float nextIdleDialogue = 0;
    bool dialogueEnded = false;
    bool potionSubmitted = false;

    void Awake()
    {
        dialogueManager = GameObject.FindWithTag("DialogueManager").GetComponent<DialogueBubbleManager>();
    }
    public void PlayButtonClicked()
    {
        gameStarted = true;
        timer = 0;
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
                    dialogueManager.SetDialogue(introduction);
                    stateWaiting = true;
                }
                else if (dialogueEnded == true)
                {
                    stateWaiting = false;
                    dialogueEnded = false;
                    gameState = 1;
                }
                break;
            case 1: // Character introduction
                if (currentChar <= characters.Count && stateWaiting == false)
                {
                    characters[currentChar].SetActive(true);
                    characters[currentChar].PullCharacterValues(out characterIntroduction, out positiveConclusion, out neutralConclusion, out negativeConclusion, out idleDialogue, out tempFloor, out tempCeiling, out carbFloor, out carbCeiling, out pazazFloor, out pazazCeiling, out potencyFloor);
                    dialogueManager.SetDialogue(characterIntroduction);
                    stateWaiting = true;
                }
                if (dialogueEnded == true)
                {
                    dialogueEnded = false;
                    stateWaiting = true;
                    gameState = 2;
                }
                break;
            case 2: // Potion crafting
                if (stateWaiting == false)
                {
                    // Initialize potion values
                    timer = 0;
                    nextIdleDialogue = Random.Range(20f, 30f);
                }


                break;
        }
    }
}
