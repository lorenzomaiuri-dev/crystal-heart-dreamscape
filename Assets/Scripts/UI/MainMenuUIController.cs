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
        Debug.Log("Uscita dal gioco!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}