using UnityEngine;

public class PauseMenuUIController : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Riferimento al Panel del menù di pausa

    void Start()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false); // Assicurati che sia nascosto all'inizio
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
            // Potresti anche voler gestire qui la visualizzazione del cursore
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void HidePauseMenu()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
            // E qui la gestione del cursore quando si riprende
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OnResumeButtonClicked()
    {
        // Comunica al sistema ECS di riprendere il gioco
        GameManager.Instance.SetGameState(GameState.Playing); // Assumendo che tu abbia un GameManager
        HidePauseMenu();
    }

    public void OnOptionsButtonClicked()
    {
        // Logica per mostrare il menù delle opzioni (potrebbe essere un altro pannello UI)
        Debug.Log("Opzioni cliccate!");
    }

    public void OnExitButtonClicked()
    {
        // Logica per uscire dal gioco
        Debug.Log("Uscita cliccata!");
        Application.Quit();
    }

    // Potresti avere altri metodi per i bottoni aggiuntivi
}