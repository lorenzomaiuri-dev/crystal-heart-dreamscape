using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalEnemy : MonoBehaviour
{
    public float speed = 2f; // Velocità di movimento
    public bool moveRight = true; // Direzione iniziale del movimento (true = destra, false = sinistra)

    private Vector2 direction; // Direzione di movimento
    private GameObject player; // Riferimento al giocatore
    private EnemyController enemyController;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        // Otteniamo il riferimento al giocatore
        player = GameObject.FindGameObjectWithTag("Player");
        
        // Imposta la direzione iniziale in base alla variabile moveRight
        direction = moveRight ? Vector2.right : Vector2.left;
        
        // Disabilita la gravità
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        // Muovi il nemico orizzontalmente
        transform.Translate(direction * speed * Time.deltaTime);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Il nemico si ferma quando colpisce il personaggio
            speed = 0f; // Ferma il movimento orizzontale del nemico
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Riavvia il movimento del nemico quando il personaggio si allontana
            speed = 2f; // Ripristina la velocità di movimento del nemico (o imposta il valore desiderato)
        }
    }

}