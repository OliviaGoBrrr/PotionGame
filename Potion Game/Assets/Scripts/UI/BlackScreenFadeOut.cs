using UnityEngine;

public class BlackScreenFadeOut : MonoBehaviour
{
    SpriteRenderer image;
    GameManager manager;
    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
        manager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    private void Update()
    {
        if (manager.gameState != 0 && image.color.a > 0)
        {
            image.color = new Color(1, 1, 1, image.color.a - Time.deltaTime);
        }
    }
}
