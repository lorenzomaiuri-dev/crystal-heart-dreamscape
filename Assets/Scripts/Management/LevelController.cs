using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [Tooltip("The name of the scene to load when the player touches the portal.")]
    [SerializeField] ScenesNames nextSceneName;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        GameManager.Instance.LoadScene(nextSceneName.ToString());
    }
}