using UnityEngine;

public class SubmitPotionButton : MonoBehaviour
{
    GameManager gameManager;
    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    public void OnButtonPress()
    {
        if (gameManager.gameState == 2)
        {
            gameManager.PotionSubmitted();
        }
    }
}
