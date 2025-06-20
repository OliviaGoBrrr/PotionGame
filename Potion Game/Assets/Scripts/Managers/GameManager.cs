using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameStarted = false;
    public void PlayButtonClicked()
    {
        gameStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
