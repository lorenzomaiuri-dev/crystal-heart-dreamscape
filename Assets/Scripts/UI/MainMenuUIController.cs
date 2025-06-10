using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    public string firstLevelSceneName = "FirstLevel";

    public void OnContinueButtonClicked()
    {
        SaveManager.Instance.shouldLoadGame = true;
        SceneManager.LoadScene(firstLevelSceneName);
    }
    
    public void OnNewGameButtonClicked()
    {
        SaveManager.Instance.shouldLoadGame = false;
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Uscita dal gioco!"); // Questo messaggio apparirà nella Console di Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}