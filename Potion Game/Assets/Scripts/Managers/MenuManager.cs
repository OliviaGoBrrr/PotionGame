using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_settingsScreen;
    [SerializeField] private GameObject blackScreen;

    public void DisplaySettings()
    {
        m_settingsScreen.SetActive(true);
    }

    public void PlayButton()
    {
        blackScreen.GetComponent<BlackScreenFadeIn>().StartTransition();
    }

    public void Back()
    {
        m_settingsScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
