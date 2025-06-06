using UnityEngine;

public class PauseMenuUIController : MonoBehaviour
{
    public GameObject pauseMenuPanel;

    void Start()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Pannello del menù di pausa non assegnato!");
        }
    }

    public void ShowPauseMenu()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void HidePauseMenu()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OnResumeButtonClicked()
    {
        GameManager.Instance.SetGameState(GameState.Playing);
        HidePauseMenu();
    }

    public void OnOptionsButtonClicked()
    {
        Debug.Log("Opzioni cliccate!");
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Uscita cliccata!");
        Application.Quit();
    }
}