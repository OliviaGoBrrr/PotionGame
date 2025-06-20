using UnityEngine;
using UnityEngine.UI;

public class GameStartFadeIn : MonoBehaviour
{
    Image image;
    GameManager manager;
    float alpha = 0;
    private void Awake()
    {
        image = GetComponent<Image>();
        manager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    private void Update()
    {
        if (manager.gameStarted == true)
        {
            alpha += Time.deltaTime * 0.5f;
        }
        image.color = new Color(1, 1, 1, alpha);
    }
}
