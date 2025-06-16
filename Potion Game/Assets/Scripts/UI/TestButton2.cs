using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TestButton2 : MonoBehaviour, IPointerClickHandler
{
    public List<string> dialogue;
    public DialogueBubble dia;
    public void OnPointerClick(PointerEventData eventData)
    {
        dia.SetDialogue(dialogue);
    }
}
