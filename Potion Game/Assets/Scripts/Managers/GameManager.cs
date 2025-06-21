using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public bool gameStarted = false;
    [Tooltip("Place characters in here in order of appearance")]
    [SerializeField] List<CharacterScript> characters;
    int currentChar = 0;

    DialogueBubbleManager dialogueManager;

    // Introduction
    [SerializeField] DialogueScriptableObject introduction;

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
            case 0:
                Timer += Time.deltaTime;
                if (Timer >= 2)
                {
                    //dialogueManager.SetDialogue();
                }
                break;
        }
    }
}
