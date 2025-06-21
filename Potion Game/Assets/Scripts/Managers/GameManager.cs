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
    float Timer = 0;
    bool dialogueEnded = false;

    void Awake()
    {
        dialogueManager = GameObject.FindWithTag("DialogueManager").GetComponent<DialogueBubbleManager>();
    }
    public void PlayButtonClicked()
    {
        gameStarted = true;
        Timer = 0;
    }
    public void DialogueEnded()
    {
        dialogueEnded = true;
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
                Timer += Time.deltaTime;
                if (Timer >= 2 && stateWaiting == false)
                {
                    dialogueManager.SetDialogue(introduction);
                    stateWaiting = true;
                }
                else if (dialogueEnded == true)
                {
                    stateWaiting = false;
                    dialogueEnded = false;
                    gameState = 0;
                }
                break;
            case 1:
                if (currentChar <= characters.Count)
                {

                }
                if (dialogueEnded == true)
                {
                    dialogueEnded = false;
                }
                break;
        }
    }
}
