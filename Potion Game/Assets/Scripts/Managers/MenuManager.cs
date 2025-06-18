using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_settingsScreen;

    public void DisplaySettings()
    {
        m_settingsScreen.SetActive(true);
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
