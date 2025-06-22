using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameStartFadeIn : MonoBehaviour
{
    Image image;
    GameManager manager;
    float alpha = 0;
    public bool isAnOwl = false;
    [SerializeField] bool inclusive;
    [SerializeField] int state;
    [SerializeField] TMP_Text text;
    Color imageColor;
    Color textColor;
    private void Awake()
    {
        image = GetComponent<Image>();
        manager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        text = GetComponent<TMP_Text>();

        if(image != null)
        {
            imageColor = image.color;
        }

        if(text != null)
        {
            textColor = text.color;
        }
    }
    private void Update()
    {
        switch (inclusive)
        {
            case true:
                if (manager.gameState == state && alpha <= 1)
                {
                    alpha += Time.deltaTime * 0.5f;
                }
                else if (manager.gameState != state && alpha >= 0)
                {
                    alpha -= Time.deltaTime * 0.5f;
                }
                break;
            case false:
                if (manager.gameState != state && alpha <= 1)
                {
                    alpha += Time.deltaTime * 0.5f;
                }
                else if (manager.gameState == state && alpha >= 0)
                {
                    alpha -= Time.deltaTime * 0.5f;
                }
                break;
        }
        if (isAnOwl == true && alpha <= 0)
        {
            gameObject.SetActive(false);
        }

        if (image != null) { image.color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha); }

        if(text != null)
        {
            text.color = new Color(textColor.r, textColor.g,textColor.b, alpha);
        }
    }
}
