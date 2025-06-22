using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Text.RegularExpressions;
using System;
using UnityEngine.Windows;

public class DialogueBubbleManager : MonoBehaviour
{
    // Components
    GameManager gameManager;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<DialogueBubble> bubbleList;

    // Stores all the instructions given when dialogue is set
    private int currentDiaPos;
    public List<DialogueBubble> currentBubble;
    List<string> names;
    List<string> dialogue;
    List<int> textFX; // 0 = no anim, 1 = rotating, 2 = size shifting, 3 = vert displacement, 4 = colours
    List<AudioClip> characterBlip;
    List<float> defaultTextInterval;
    List<float> fastTextInterval;
    List<int> defaultBlipInterval;
    List<int> fastBlipInterval;
    List<TMP_FontAsset> textFont;
    List<float> fontSize;
    List<TMP_FontAsset> nameFont;

    int State = 0; // 0 = no dialogue, 1 = typing start, 2 = typing, 3 = complete
    
    // TextSpeed
    float textInterval;

    // SoundCues
    int soundInterval;
    int soundCount;

    // Visual clutter
    List<float> animStates;

    // Animation
    float RotationLimit = 20;
    float RotationSpeed = 3;
    float SizeLimit = 20;
    float SizeSpeed = 1.5f;
    float VertLimit = 0.2f;
    float VertSpeed = 2;
    float ColourSpeed = 2f;

    Coroutine type;

    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    // Recieves a list of dialogue from another source
    public void SetDialogue(DialogueScriptableObject currentDialogue)
    {
        // Clears any existing dialogue
        foreach(DialogueBubble bubble in bubbleList)
        {
            if (type != null) { StopCoroutine(type); }
            bubble.nameTextBox.text = string.Empty;
            bubble.dialogueTextBox.text = string.Empty;
        }

        currentBubble = new List<DialogueBubble>( new DialogueBubble[currentDialogue.Bubble.Count] );
        for ( int i = 0; i < currentDialogue.Bubble.Count; i++ )
        {
            currentBubble[i] = bubbleList[currentDialogue.Bubble[i]];
        }
        names = currentDialogue.NameText;
        dialogue = currentDialogue.DialogueText;
        textFX = currentDialogue.TextAnim;
        characterBlip = currentDialogue.AudioClip;
        defaultTextInterval = currentDialogue.DefaultTextInterval;
        fastTextInterval = currentDialogue.FastTextInterval;
        defaultBlipInterval = currentDialogue.DefaultBlipInterval;
        fastBlipInterval = currentDialogue.FastBlipInterval;
        textFont = currentDialogue.Font;
        fontSize = currentDialogue.FontSize;
        nameFont = currentDialogue.NameFont;

        currentDiaPos = 0;
        NewLinePrep();
    }
    void NewLinePrep()
    {
        State = 1;
        textInterval = defaultTextInterval[currentDiaPos];
        soundCount = 0;
        soundInterval = defaultBlipInterval[currentDiaPos];
    }
    void Update()
    {
        PrintWords();
        AnimateWords();
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) && Application.isEditor == true)
        {
            Skip();
        }
    }
    // Resets text interval and initiates coroutine
    void PrintWords()
    {
        if (State == 1)
        {
            ResetAnim();
            type = StartCoroutine(TypeText(currentBubble[currentDiaPos], names[currentDiaPos], dialogue[currentDiaPos], textFX[currentDiaPos], characterBlip[currentDiaPos], textFont[currentDiaPos], fontSize[currentDiaPos], nameFont[currentDiaPos]));
            State = 2;
        }
    }
    // Resets the list that stores animation states
    void ResetAnim()
    {
        animStates = new List<float>(dialogue[currentDiaPos].Length);
        for (int i = 0; i < dialogue[currentDiaPos].Length; i++) // Sets default direction for animation
        {
            animStates.Add(0);
        }
    }
    // Coroutine which types each letter out one at a time
    IEnumerator TypeText(DialogueBubble currentBubble, string nameText, string dialogueText, int textFX, AudioClip currentBlip, TMP_FontAsset textFont, float fontSize, TMP_FontAsset nameFont)
    {
        currentBubble.nameTextBox.text = nameText;
        currentBubble.nameTextBox.font = nameFont;
        currentBubble.dialogueTextBox.text = string.Empty;
        currentBubble.dialogueTextBox.font = textFont;
        currentBubble.dialogueTextBox.fontSize = fontSize;
        currentBubble.StartExisting();
        for (int i = 0; i < dialogueText.Length; i++)
        {
            switch (textFX)
            {
                case 0:
                    currentBubble.dialogueTextBox.text += dialogueText[i];
                    break;
                case 1:
                    currentBubble.dialogueTextBox.text += "<rotate=\"" + RotationLimit + "\">" + dialogueText[i] + "</rotate>";
                    break;
                case 2:
                    currentBubble.dialogueTextBox.text += "<size=100%>" + dialogueText[i];
                    break;
                case 3:
                    currentBubble.dialogueTextBox.text += "<voffset=" + 0 + "em>" + dialogueText[i] + "</voffset>";
                    break;
                case 4:
                    currentBubble.dialogueTextBox.text += "<color=#000000>" + dialogueText[i] + "</color>";
                    break;
                default:
                    currentBubble.dialogueTextBox.text += dialogueText[i];
                    break;
            }
            soundCheck(currentBlip);   
            yield return new WaitForSeconds(textInterval);
        }
        State = 3;
        //AnimateWords(); // this is for testing
        yield return null;
    }
    private void soundCheck(AudioClip blip)
    {
        if (soundCount == 0)
        {
            audioSource.resource = blip;
            int random = UnityEngine.Random.Range(-2, 3);
            audioSource.pitch = 1 * Mathf.Pow(1.059463f, random);
            audioSource.PlayOneShot(blip);
            soundCount = soundInterval;
        }
        else { soundCount -= 1; }
    }
    void AnimateWords() // Breaks the strings down into substrings, modifies the number for the correct string and then stitches it back together
    {
        if (State != 0)
        {
            switch (textFX[currentDiaPos])
            {
                case 0:
                    break;
                case 1:
                    string[] rotSubStrings = Regex.Split(currentBubble[currentDiaPos].dialogueTextBox.text, @"(?<=[<>])"); // Divides the string into a set of substring spagetti
                    for (int i = 0; i < rotSubStrings.Length - 1; i += 4) // Repeat for each character
                    {
                        int startindex = rotSubStrings[i + 1].IndexOf('"');
                        int endindex = rotSubStrings[i + 1].LastIndexOf('"');
                        string currentValueString = rotSubStrings[i + 1].Substring(startindex + 1, endindex - startindex - 1); // Locate the numbers in the substring that we want
                        float.TryParse(currentValueString, out float currentValueFloat); // Convert it to a float
                        if (currentValueFloat >= RotationLimit) { animStates[i/4] = 0; } // Ensures that the rotation stays within bounds
                        else if (currentValueFloat <= RotationLimit * -1) { animStates[i/4] = 1; }

                        // No Lerp
                        //if (animStates[i/4] == 0) { currentValueFloat -= Time.deltaTime * RotationSpeed; }
                        //else if (animStates[i/4] == 1) { currentValueFloat += Time.deltaTime * RotationSpeed; }

                        // Yes Lerp
                        if (animStates[i / 4] == 0) { currentValueFloat = Mathf.Lerp(currentValueFloat, RotationLimit * -1.1f, Time.deltaTime * RotationSpeed); }
                        else if (animStates[i / 4] == 1) { currentValueFloat = Mathf.Lerp(currentValueFloat, RotationLimit * 1.1f, Time.deltaTime * RotationSpeed); }
                        rotSubStrings[i + 1] = "rotate=\"" + currentValueFloat + "\">"; // I'm hard coding this i'm so tired of regex
                    }
                    currentBubble[currentDiaPos].dialogueTextBox.text = string.Join(string.Empty, rotSubStrings);
                    break;
                case 2:
                    string[] sizeSubStrings = Regex.Split(currentBubble[currentDiaPos].dialogueTextBox.text, @"(?<=[<>])");
                    for (int i = 0; i < sizeSubStrings.Length - 1; i += 2) // Repeat for each character
                    {
                        int startindex = sizeSubStrings[i + 1].IndexOf('=');
                        int endindex = sizeSubStrings[i + 1].IndexOf('%');
                        string currentValueString = sizeSubStrings[i + 1].Substring(startindex + 1, endindex - startindex - 1);// Locate the numbers in the substring that we want
                        float.TryParse(currentValueString, out float currentValueFloat); // Convert it to a float
                        if (currentValueFloat >= 100 + SizeLimit) { animStates[i / 2] = 0; } // Ensures that the size stays within bounds
                        else if (currentValueFloat <= 100 - SizeLimit) { animStates[i / 2] = 1; }

                        // No Lerp
                        //if (animStates[i / 2] == 0) { currentValueFloat -= Time.deltaTime * SizeSpeed; }
                        //else if (animStates[i / 2] == 1) { currentValueFloat += Time.deltaTime * SizeSpeed; }

                        // Yes Lerp
                        if (animStates[i / 2] == 0) { currentValueFloat = Mathf.Lerp(currentValueFloat, 100 + SizeLimit * -1.1f, Time.deltaTime * SizeSpeed); }
                        else if (animStates[i / 2] == 1) { currentValueFloat = Mathf.Lerp(currentValueFloat, 100 + SizeLimit * 1.1f, Time.deltaTime * SizeSpeed); }

                        sizeSubStrings[i + 1] = "size=" + currentValueFloat + "%>"; // I'm hard coding this i'm so tired of regex
                    }
                    currentBubble[currentDiaPos].dialogueTextBox.text = string.Join(string.Empty, sizeSubStrings);
                    break;
                case 3:
                    string[] vertSubStrings = Regex.Split(currentBubble[currentDiaPos].dialogueTextBox.text, @"(?<=[<>])"); // Divides the string into a set of substring spaghetti
                    for (int i = 0; i < vertSubStrings.Length - 1; i += 4) // Repeat for each character
                    {
                        int startindex = vertSubStrings[i + 1].IndexOf('=');
                        int endindex = vertSubStrings[i + 1].LastIndexOf('e');
                        string currentValueString = vertSubStrings[i + 1].Substring(startindex + 1, endindex - startindex - 1); // Locate the numbers in the substring that we want
                        float.TryParse(currentValueString, out float currentValueFloat); // Convert it to a float
                        if (currentValueFloat >= VertLimit) { animStates[i / 4] = 0; } // Ensures that the vert displacement stays within bounds
                        else if (currentValueFloat <= VertLimit * -1) { animStates[i / 4] = 1; }
                        if (animStates[i / 4] == 0) { currentValueFloat = Mathf.Lerp(currentValueFloat, VertLimit * -1.1f, Time.deltaTime * VertSpeed); }
                        else if (animStates[i / 4] == 1) { currentValueFloat = Mathf.Lerp(currentValueFloat, (VertLimit * 1.1f), Time.deltaTime * VertSpeed); }
                        vertSubStrings[i + 1] = "voffset=" + currentValueFloat + "em>"; // I'm hard coding this i'm so tired of regex
                    }
                    currentBubble[currentDiaPos].dialogueTextBox.text = string.Join(string.Empty, vertSubStrings);
                    break;
                case 4:
                    string[] colorSubStrings = Regex.Split(currentBubble[currentDiaPos].dialogueTextBox.text, @"(?<=[<>])"); // Divides the string into a set of substring spagetti
                    for (int i = 0; i < colorSubStrings.Length - 1; i += 4) // Repeat for each character
                    {
                        animStates[i/4] += Time.deltaTime * ColourSpeed;
                        Color textColor;
                        switch (Mathf.Floor(animStates[i/4]))
                        {
                            case 0:
                                textColor = Color.Lerp(Color.red, Color.blue, animStates[i/4]);
                                break;
                            case 1:
                                textColor = Color.Lerp(Color.blue, Color.green, animStates[i/4] - 1);
                                break;
                            case 2:
                                textColor = Color.Lerp(Color.green, Color.red, animStates[i/4] - 2);
                                break;
                            default:
                                textColor = Color.red;
                                animStates[i/4] = 0;
                                break;
                        }
                        colorSubStrings[i + 1] = "color=#" + UnityEngine.ColorUtility.ToHtmlStringRGB(textColor) + ">"; // I'm hard coding this i'm so tired of regex
                    }
                    currentBubble[currentDiaPos].dialogueTextBox.text = string.Join(string.Empty, colorSubStrings);
                    break;
                default:
                    break;
            }
        }
    }
    // Allows the player to either reduce the text interval or go to the next message by clicking on the dialogue bubble
    public void BubbleClickEvent()
    {
        if (State == 2)
        {
            // Modifies the interval statistics to make the text scroll faster
            textInterval = fastTextInterval[currentDiaPos];
            soundInterval = fastBlipInterval[currentDiaPos];
        }
        if (State == 3)
        {
            currentBubble[currentDiaPos].nameTextBox.text = string.Empty;
            currentBubble[currentDiaPos].dialogueTextBox.text = string.Empty;
            if (currentDiaPos + 1 >= dialogue.Count) // If there is no more dialogue, go back to state 0
            {
                State = 0;
                gameManager.DialogueEnded();
            }
            else
            {
                currentDiaPos += 1;
                State = 1;
                NewLinePrep();
            }
        }
    }
    public void Skip()
    {
        currentBubble[currentDiaPos].nameTextBox.text = string.Empty;
        currentBubble[currentDiaPos].dialogueTextBox.text = string.Empty;
        State = 0;
        gameManager.DialogueEnded();
        if (type != null) { StopCoroutine(type); }
    }
}
