using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpPowerUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        // Se il giocatore entra in collisione con il power-up
        if (collider.CompareTag("Player"))
        {
            PowerUpManager.Instance.EnableDoubleJump(); // Attiva il power-up
            Destroy(gameObject); // Distrugge l'oggetto power-up
        }
    }
}
