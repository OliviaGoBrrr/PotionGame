using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogueScriptableObject", menuName = "Scriptable Objects/DialogueScriptableObject")]
public class DialogueScriptableObject : ScriptableObject
{
    [Tooltip("The ID mumber for the text bubble for this line of dialogue")]
    [SerializeField] List<int> bubble;
    public List<int> Bubble { get => bubble; private set => bubble = value; }
    [Tooltip("The name displayed on this line of dialogue (RichText okay)")]
    [SerializeField] List<string> nameText;
    public List<string> NameText { get => nameText; private set => nameText = value; }
    [Tooltip("The dialogue (RichText not okay)")]
    [SerializeField] List<string> dialogueText;
    public List<string> DialogueText { get => dialogueText; private set => dialogueText = value; }
    [Tooltip("The number value that matches the hardcoded text animation")]
    [SerializeField] List<int> textAnim;
    public List<int> TextAnim { get => textAnim; private set => textAnim = value; }
    [Tooltip("The audio blip the character makes when they speak")]
    [SerializeField] List<AudioClip> audioClip;
    public List<AudioClip> AudioClip { get => audioClip; private set => audioClip = value; }
    [Tooltip("The amount of time between characters appearing (I would reccomend 0.05 seconds)")]
    [SerializeField] List<float> defaultTextInterval;
    public List<float> DefaultTextInterval { get => defaultTextInterval; private set => defaultTextInterval = value; }
    [Tooltip("The amount of time between characters appearing after one click (I would reccomend 0.01 seconds)")]
    [SerializeField] List<float> fastTextInterval;
    public List<float> FastTextInterval { get => fastTextInterval; private set => fastTextInterval = value; }
    [Tooltip("The amount of characters that need to appear before the audio plays (I would reccomend 3)")]
    [SerializeField] List<int> defaultBlipInterval;
    public List<int> DefaultBlipInterval { get => defaultBlipInterval; private set => defaultBlipInterval = value; }
    [Tooltip("The amount of characters that need to appear before the audio plays (I would reccomend 10)")]
    [SerializeField] List<int> fastBlipInterval;
    public List<int> FastBlipInterval { get => defaultBlipInterval; private set => defaultBlipInterval = value; }
    [Tooltip("Size of the dialogue font (for the name text, use TextMeshPro RichText)")]
    [SerializeField] List<float> fontSize;
    public List<float> FontSize { get => fontSize; private set => fontSize = value; }
}
