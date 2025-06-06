using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    [Tooltip("How much health does this item be healed")]
    [SerializeField] int healAmount = 1;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerController player = collider.GetComponent<PlayerController>();
            player.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
