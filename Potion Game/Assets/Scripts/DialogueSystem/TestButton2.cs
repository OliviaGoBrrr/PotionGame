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
        dia.SetDialogue(dialogue);
    }
}
