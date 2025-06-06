using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;

    private Vector2 targetPosition;
    private SpriteRenderer spriteRenderer;
    private bool movingRight;
    private GameObject player;
    private EnemyController enemyController;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (pointA == null || pointB == null)
        {
            Debug.LogError("I punti di riferimento A e B non sono stati assegnati all'enemy: " + gameObject.name);
            enabled = false;
            return;
        }
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Lo SpriteRenderer non è stato trovato sull'enemy: " + gameObject.name);
            enabled = false;
            return;
        }

        // Start with A
        targetPosition = pointA.position;
        movingRight = false;

        // No gravity
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
        }
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check target
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f) // Usa una piccola soglia per evitare oscillazioni
        {
            // switch target
            if (targetPosition == (Vector2)pointA.position)
            {
                targetPosition = pointB.position;
                movingRight = true;
            }
            else
            {
                targetPosition = pointA.position;
                movingRight = false;
            }
            
        }
        
        Flip();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            speed = 0f;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            speed = 2f;
        }
    }

    // Debug
    private void OnDrawGizmosSelected()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(pointA.position, 0.2f);
            Gizmos.DrawSphere(pointB.position, 0.2f);
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
    
    private void Flip()
    {
        if (!movingRight)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
}