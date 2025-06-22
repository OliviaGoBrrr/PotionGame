using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CharacterScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [Header("Character Sprites")]
    [SerializeField] Sprite defaultPose;
    [SerializeField] Sprite happyPose;
    [SerializeField] Sprite neutralPose;
    [SerializeField] Sprite unhappyPose;

    [Header("Dialogue Scriptable Objects")]
    [Tooltip("Keep this list to one scriptable object if you want linear. If this list is 3 objects, it will choose from the list based on the previous potion's success. 0 is positive, 1 is neutral, 2 is negative.")]
    [SerializeField] List<DialogueScriptableObject> introductionDialogue;
    [SerializeField] DialogueScriptableObject happyDialogue;
    [SerializeField] DialogueScriptableObject neutralDialogue;
    [SerializeField] DialogueScriptableObject unhappyDialogue;
    [SerializeField] List<DialogueScriptableObject> idleDialogue;

    [Header("Potion values")]
    [Tooltip("The minumum value for the temperature attribute to be 'correct' (0 ~ 10, this value is included)")]
    [SerializeField] int tempFloor;
    [Tooltip("The maximum value for the temperature attribute to be 'correct' (0 ~ 10, this value is included)")]
    [SerializeField] int tempCeiling;
    [Tooltip("The minumum value for the carbination attribute to be 'correct' (0 ~ 10, this value is included)")]
    [SerializeField] int carbFloor;
    [Tooltip("The maximum value for the carbination attribute to be 'correct' (0 ~ 10, this value is included)")]
    [SerializeField] int carbCeiling;
    [Tooltip("The minumum value for the pazaz attribute to be 'correct' (0 ~ 10, this value is included)")]
    [SerializeField] int pazazFloor;
    [Tooltip("The maximum value for the pazaz attribute to be 'correct' (0 ~ 10, this value is included)")]
    [SerializeField] int pazazCeiling;
    [Tooltip("The minimum value for the potency to be 'correct' (0 ~ 1, this value is included)")]
    [SerializeField] float potencyFloor;
    [Tooltip("The name of the potion")]
    [SerializeField] public string potionName;
    [Tooltip("The visuals for the potion canvas")]
    [SerializeField] public int potionVisual;

    [SerializeField] public AudioClip potionSound;
    bool active;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetActive(bool Active)
    {
        active = Active;
    }
    void Update()
    {
        FadeSet();
    }
    private void FadeSet()
    {
        if (active == true && spriteRenderer.color.a < 1)
        {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a + Time.deltaTime);
        }
        else if (active == false && spriteRenderer.color.a > 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - Time.deltaTime);
        }
    }
    public void SetSprite(int spriteNum) // sets sprite with ints from 0 to 3
    {
        switch(spriteNum)
        {
            case 0:
                spriteRenderer.sprite = defaultPose; 
                break;
            case 1:
                spriteRenderer.sprite = happyPose;
                break;
            case 2:
                spriteRenderer.sprite = neutralPose;
                break;
            case 3:
                spriteRenderer.sprite = unhappyPose;
                break;
        }
    }
    public void PullCharacterValues(out List<DialogueScriptableObject> intro, out DialogueScriptableObject positive, out DialogueScriptableObject neutral, out DialogueScriptableObject negative, 
        out List<DialogueScriptableObject> idle, out int tempF, out int tempC, out int carbF, out int carbC, out int pazazF, out int pazazC, out float potencyF)
    {
        intro = introductionDialogue;
        positive = happyDialogue;
        neutral = neutralDialogue;
        negative = unhappyDialogue;
        idle = idleDialogue;

        tempF = tempFloor;
        tempC = tempCeiling;
        carbF = carbFloor;
        carbC = carbCeiling;
        pazazF = pazazFloor;
        pazazC = pazazCeiling;
        potencyF = potencyFloor;
    }
}
