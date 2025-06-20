using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TestButton2 : MonoBehaviour, IPointerClickHandler
{
    public DialogueBubbleManager dia;
    public DialogueScriptableObject dialogue;
    public void OnPointerClick(PointerEventData eventData)
    {
        dia.SetDialogue(dialogue.Bubble, dialogue.NameText, dialogue.DialogueText, dialogue.TextAnim, dialogue.AudioClip, dialogue.DefaultTextInterval, dialogue.FastTextInterval, dialogue.DefaultBlipInterval, dialogue.FastBlipInterval, dialogue.FontSize);
    }
}
