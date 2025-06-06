using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpPowerUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PowerUpManager.Instance.EnableDoubleJump();
            Destroy(gameObject);
        }
    }
}
