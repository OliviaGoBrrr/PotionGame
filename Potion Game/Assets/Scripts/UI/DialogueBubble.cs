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

public class DialogueBubble : MonoBehaviour, IPointerClickHandler
{
    // Components
    UnityEngine.UI.Image DiaBubble;
    [SerializeField] TMP_Text textObject;

    int State = 0; // 0 = no dialogue, 1 = typing start, 2 = typing, 3 = complete
    int TextAnim = 0; // 0 = no anim, 1 = rotating, 2 = size shifting, 3 = vert displacement, 4 = colours

    // Dialogue logic
    List<string> dialogueList;
    int lineNum;
    float textIntervalDefault = 0.1f;
    float textInterval;

    // Visual clutter
    List<float> animStates;
    float transparency = 1;

    // Animation
    float RotationLimit = 25;
    float RotationSpeed = 3;
    float SizeLimit = 20;
    float SizeSpeed = 60;
    float VertLimit = 0.2f;
    float VertSpeed = 0.4f;
    float ColourSpeed = 2f;


    void Awake()
    {
        DiaBubble = GetComponent<UnityEngine.UI.Image>();
    }

    // Recieves a list of dialogue from another source
    public void SetDialogue(List<string> dialogue, int textAnimNum)
    {
        dialogueList = dialogue;
        State = 1;
        lineNum = 0;
        TextAnim = textAnimNum;
    }
    void Update()
    {
        Transparency();
        PrintWords();
        AnimateWords();
    }
    // If the state is 0 (no text), fades everything out. Otherwise, fade in.
    void Transparency()
    {
        if (State == 0 && transparency >= 0)
        {
            transparency -= Time.deltaTime * 5;
            DiaBubble.color = new Color(255, 255, 255, transparency);
            textObject.color = new Color(0, 0, 0, transparency);
        }
        else if (State != 0 && transparency <= 1)
        {
            transparency += Time.deltaTime * 5;
            DiaBubble.color = new Color(255, 255, 255, transparency);
            textObject.color = new Color(0, 0, 0, transparency);
        }
    }
    // Resets text interval and initiates coroutine
    void PrintWords()
    {
        if (State == 1)
        {
            ResetAnim();
            textInterval = textIntervalDefault;
            StartCoroutine(TypeText(lineNum));
            State = 2;
        }
    }
    // Coroutine which types each letter out one at a time
    IEnumerator TypeText(int lineN)
    {
        textObject.text = string.Empty;
        for (int i = 0; i < dialogueList[lineN].Length; i++)
        {
            switch (TextAnim)
            {
                case 0:
                    textObject.text += dialogueList[lineN][i];
                    break;
                case 1:
                    textObject.text += "<rotate=\"" + RotationLimit + "\">" + dialogueList[lineN][i] + "</rotate>";
                    break;
                case 2:
                    textObject.text += "<size=100%>" + dialogueList[lineN][i];
                    break;
                case 3:
                    textObject.text += "<voffset=" + 0 + "em>" + dialogueList[lineN][i] + "</voffset>";
                    break;
                case 4:
                    textObject.text += "<color=#000000>" + dialogueList[lineN][i] + "</color>";
                    break;
                default:
                    textObject.text += dialogueList[lineN][i];
                    break;
            }
            
            yield return new WaitForSeconds(textInterval);
        }
        State = 3;
        //AnimateWords(); // this is for testing
        yield return null;
    }
    // Resets the list that stores animation states
    void ResetAnim()
    {
        animStates = new List<float>(dialogueList[lineNum].Length);
        for(int i = 0; i < dialogueList[lineNum].Length; i++) // Sets default direction for animation
        {
            animStates.Add(0);
        }
    }
    void AnimateWords() // Breaks the strings down into substrings, modifies the number for the correct string and then stitches it back together
    {
        if (State != 0)
        {
            switch (TextAnim)
            {
                case 0:
                    break;
                case 1:
                    string[] rotSubStrings = Regex.Split(textObject.text, @"(?<=[<>])"); // Divides the string into a set of substring spagetti
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
                    textObject.text = string.Join(string.Empty, rotSubStrings);
                    break;
                case 2:
                    string[] sizeSubStrings = Regex.Split(textObject.text, @"(?<=[<>])");
                    for (int i = 0; i < sizeSubStrings.Length - 1; i += 2) // Repeat for each character
                    {
                        int startindex = sizeSubStrings[i + 1].IndexOf('='); Debug.Log(startindex + "start index");
                        int endindex = sizeSubStrings[i + 1].IndexOf('%'); Debug.Log(startindex + "end index");
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
                    textObject.text = string.Join(string.Empty, sizeSubStrings);
                    break;
                case 3:
                    string[] vertSubStrings = Regex.Split(textObject.text, @"(?<=[<>])"); // Divides the string into a set of substring spagetti
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
                    textObject.text = string.Join(string.Empty, vertSubStrings);
                    break;
                case 4:
                    string[] colorSubStrings = Regex.Split(textObject.text, @"(?<=[<>])"); // Divides the string into a set of substring spagetti
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
                    textObject.text = string.Join(string.Empty, colorSubStrings);
                    break;
                default:
                    break;
            }
        }
    }
    // Allows the player to either reduce the text interval or go to the next message by clicking on the dialogue bubble
    public void OnPointerClick(PointerEventData eventData)
    {
        if (State == 2)
        {
            textInterval = 0.01f;
        }
        if (State == 3)
        {
            if (lineNum + 1 >= dialogueList.Count) // If there is no more dialogue, go back to state 0
            {
                State = 0;
            }
            else
            {
                lineNum += 1;
                State = 1;
            }
        }
    }
}
