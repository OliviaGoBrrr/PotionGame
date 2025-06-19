using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TestButton2 : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] List<DialogueBubble> bubble;
    [SerializeField] List<string> names;
    [SerializeField] List<string> dialogue;
    [SerializeField] List<int> textFX; // 0 = no anim, 1 = rotating, 2 = size shifting, 3 = vert displacement, 4 = colours
    [SerializeField] List<AudioClip> characterBlip;
    public DialogueBubbleManager dia;

    public void OnPointerClick(PointerEventData eventData)
    {
        dia.SetDialogue(bubble, names, dialogue, textFX, characterBlip);
    }
}
