using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameStartFadeIn : MonoBehaviour
{
    Image image;
    GameManager manager;
    float alpha = 0;
    [SerializeField] bool inclusive;
    [SerializeField] int state;
    private void Awake()
    {
        image = GetComponent<Image>();
        manager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
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
        image.color = new Color(1, 1, 1, alpha);
    }
}
