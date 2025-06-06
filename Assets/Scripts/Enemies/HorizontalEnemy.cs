using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalEnemy : MonoBehaviour
{
    public float speed = 2f;
    public bool moveRight = true;

    private Vector2 direction;
    private GameObject player;
    private EnemyController enemyController;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        
        player = GameObject.FindGameObjectWithTag("Player");
        
        direction = moveRight ? Vector2.right : Vector2.left;
        
        // No gravity
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        // Horizontal movement
        transform.Translate(direction * speed * Time.deltaTime);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Stop
            speed = 0f;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Restart moving
            speed = 2f;
        }
    }

}