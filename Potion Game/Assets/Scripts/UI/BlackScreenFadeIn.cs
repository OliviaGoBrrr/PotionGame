using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackScreenFadeIn : MonoBehaviour
{
    bool active = false;
    SpriteRenderer image;
    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
    }
    public void StartTransition()
    {
        active = true;
    }
    private void Update()
    {
        if (image.color.a < 1 && active == true)
        {
            image.color = new Color(1, 1, 1, image.color.a + Time.deltaTime);
        }
        if (image.color.a >= 1 && active == true)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
