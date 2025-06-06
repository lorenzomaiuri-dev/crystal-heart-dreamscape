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
    public float speed = 2f; // Velocità di inseguimento
    public float detectionRadius = 5f; // Raggio di rilevamento del giocatore
    public int scorePoints = 500; // Punti che il giocatore ottiene quando il nemico viene distrutto

    private GameObject player; // Riferimento al giocatore
    private Rigidbody2D rb2d;
    private Animator animator;
    private EnemyController enemyController;
    private bool isDestroyed = false;

    // Effetto di esplosione (opzionale)
    public GameObject explodeEffectPrefab;

    void Awake()
    {
        // Otteniamo il riferimento al giocatore
        player = GameObject.FindGameObjectWithTag("Player");

        // Otteniamo i componenti
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
    }

    void Update()
    {
        // Se il nemico non è ancora distrutto, continua a inseguire il giocatore
        if (!isDestroyed)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        // Calcola la direzione verso il giocatore
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Muovi il nemico verso il giocatore
        rb2d.velocity = direction * speed;

        // Se il nemico è abbastanza vicino al giocatore, si autodistrugge
        if (Vector2.Distance(transform.position, player.transform.position) < 0.5f)
        {
            SelfDestruct();
        }
    }

    void SelfDestruct()
    {
        if (isDestroyed) return;

        isDestroyed = true;

        // Infliggi danno al giocatore
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(enemyController.contactDamage);
        }

        // Attiva l'effetto di esplosione
        if (explodeEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explodeEffectPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2f); // Distruggi l'effetto dopo 2 secondi
        }

        // Distruggi il nemico
        Destroy(gameObject);

        // Aggiungi punti al giocatore (se gestisci il punteggio in GameManager)
        // GameManager.Instance.AddScorePoints(scorePoints);
    }
}
