using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class DialogueBubble : MonoBehaviour, IPointerClickHandler
{
    // Components
    UnityEngine.UI.Image DiaBubble;
    [SerializeField] TMP_Text textObject;

    int State = 0; // 0 = no dialogue, 1 = typing start, 2 = typing, 3 = complete

    // Dialogue logic
    List<string> dialogueList;
    int lineNum;
    float textIntervalDefault = 0.1f;
    float textInterval;

    float transparency = 1;

    // Animation
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] float animScale;

    void Awake()
    {
        DiaBubble = GetComponent<UnityEngine.UI.Image>();
    }

    // Recieves a list of dialogue from another source
    public void SetDialogue(List<string> dialogue)
    {
        dialogueList = dialogue;
        State = 1;
        lineNum = 0;
    }
    void Update()
    {
        Transparency();
        PrintWords();
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
            textObject.text += dialogueList[lineN][i];
            //textObject.text += CharRichText(dialogueList[lineN][i], dialogueList[lineN].Length, i, 0.1f);
            yield return new WaitForSeconds(textInterval);
        }
        State = 3;
        yield return null;
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

    // don't look at this it's not real
    float textAnimDuration = 0.1f;
    private string CharRichText(char c, int stringLength, int characterpos, float t)
    {
        float startPoint = ((1 - textAnimDuration) / (stringLength - 1)) * characterpos;
        float endPoint = startPoint + textAnimDuration;
        float subT = t.Map(startPoint, endPoint);

        string sizeStart = $"size={animCurve.Evaluate(subT) * animScale}%";
        string sizeEnd = "</size>";

        return sizeStart + c + sizeEnd;
    }
}
public static class Extensions
{
    public static float Map(this float value, float start, float end, float a = 0f, float b = 1f)
    {
        return (value - start) * (b - a) / (end - start) + a;
    }

}
