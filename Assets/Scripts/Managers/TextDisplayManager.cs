using UnityEngine;
using TMPro; // Assicurati di includere questo namespace per TextMeshPro
using System.Collections;

public class TextDisplayManager : MonoBehaviour
{
    public static TextDisplayManager Instance;
    public TextMeshProUGUI powerUpText;
    public TextMeshProUGUI gameOverText;
    public float displayDuration = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        if (powerUpText == null)
        {
            Debug.LogError("TextMeshProUGUI non assegnato a TextDisplay!");
        }
        else
        {
            powerUpText.gameObject.SetActive(false);
        }
    }
    
    public void ShowGameOverText(string text)
    {
        if (gameOverText != null)
        {
            gameOverText.text = text;
            gameOverText.gameObject.SetActive(true);
            StartCoroutine(HideTextAfterDelay());
        }
    }

    public void ShowPowerUpText(string text)
    {
        if (powerUpText != null)
        {
            powerUpText.text = text;
            powerUpText.gameObject.SetActive(true);
            StartCoroutine(HideTextAfterDelay());
        }
    }

    private IEnumerator HideTextAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        if (powerUpText != null)
        {
            powerUpText.gameObject.SetActive(false);
        }
    }
}