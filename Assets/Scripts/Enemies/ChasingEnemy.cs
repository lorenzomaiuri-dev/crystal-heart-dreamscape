using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Aggiungi i componenti necessari
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(EnemyController))]

public class ChasingEnemy : MonoBehaviour
{
    public float speed = 2f;
    public float detectionRadius = 5f;

    private GameObject player;
    private Rigidbody2D rb2d;
    private Animator animator;
    private EnemyController enemyController;
    private bool isDestroyed = false;

    public GameObject explodeEffectPrefab;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
    }

    void Update()
    {
        if (!isDestroyed)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        // Get Player direction
        Vector2 direction = (player.transform.position - transform.position).normalized;
        
        rb2d.velocity = direction * speed;
        
        if (Vector2.Distance(transform.position, player.transform.position) < 0.5f)
        {
            SelfDestruct();
        }
    }

    void SelfDestruct()
    {
        if (isDestroyed) return;

        isDestroyed = true;
        
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(enemyController.contactDamage);
        }
        
        if (explodeEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explodeEffectPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
        }

        // Self Destroy
        Destroy(gameObject);

    }
}
