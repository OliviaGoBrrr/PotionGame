using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueBubble : MonoBehaviour, IPointerClickHandler
{
    UnityEngine.UI.Image DiaBubble;
    public TMP_Text nameTextBox;
    public TMP_Text dialogueTextBox;

    [SerializeField] DialogueBubbleManager manager;
    float transparency = 0;
    float transparencySpeed = 5;
    private void Awake()
    {
        DiaBubble = GetComponent<UnityEngine.UI.Image>();
    }
    private void Update()
    {
        if (dialogueTextBox.text.Length == 0 && nameTextBox.text.Length == 0 && transparency >= 0)
        {
            transparency -= Time.deltaTime * transparencySpeed;
            if (transparency <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        else if (dialogueTextBox.text.Length != 0 && transparency <= 1)
        {
            transparency += Time.deltaTime * transparencySpeed; 
        }
        DiaBubble.color = new Color(255, 255, 255, transparency);
        nameTextBox.color = new Color(0, 0, 0, transparency);
        dialogueTextBox.color = new Color(0, 0, 0, transparency);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (transparency >= 1)
        {
            manager.BubbleClickEvent();
        }
    }
    public void StartExisting()
    {
        gameObject.SetActive(true);
    }
}
