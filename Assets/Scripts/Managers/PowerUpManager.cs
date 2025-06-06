using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public bool hasDash = false;
    public bool hasDoubleJump = false;
    
    // Singleton instance.
    public static PowerUpManager Instance = null;

    // Initialize the singleton instance.
    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    // Metodo per abilitare il power-up (double jump)
    public void EnableDoubleJump()
    {
        hasDoubleJump = true;
        if (TextDisplayManager.Instance != null)
        {
            TextDisplayManager.Instance.ShowPowerUpText("Double Jump Acquisito!");
        }
        //Debug.Log("Double Jump acquisito!");
    }

    // Metodo per disabilitare il power-up
    public void DisableDoubleJump()
    {
        hasDoubleJump = false;
        Debug.Log("Double Jump disabilitato!");
    }
    
    public void EnableDash()
    {
        hasDash = true;
        //Debug.Log("Dash acquisito!");
    }
    
    public void DisableDash()
    {
        hasDash = false;
        Debug.Log("Dash disabilitato!");
    }
}


