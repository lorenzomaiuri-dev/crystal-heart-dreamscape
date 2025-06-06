using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    public float speed = 2f; // Velocità di movimento
    public Transform pointA; // Punto di riferimento A
    public Transform pointB; // Punto di riferimento B

    private Vector2 targetPosition; // Il punto verso cui il nemico si sta muovendo
    private SpriteRenderer spriteRenderer;
    private bool movingRight; // Indica se il nemico si sta muovendo verso destra
    private GameObject player; // Riferimento al giocatore
    private EnemyController enemyController;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        // Otteniamo il riferimento al giocatore
        player = GameObject.FindGameObjectWithTag("Player");
        
        // Assicurati che i punti A e B siano stati assegnati nell'Inspector
        if (pointA == null || pointB == null)
        {
            Debug.LogError("I punti di riferimento A e B non sono stati assegnati all'enemy: " + gameObject.name);
            enabled = false; // Disabilita lo script se i punti non sono impostati
            return;
        }

        // Ottieni il componente SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Lo SpriteRenderer non è stato trovato sull'enemy: " + gameObject.name);
            enabled = false;
            return;
        }

        // Imposta la posizione iniziale come punto A
        targetPosition = pointA.position;
        movingRight = false; // Inizia muovendosi verso sinistra

        // Disabilita la gravità (opzionale, a seconda del tuo gioco)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
        }
    }

    void Update()
    {
        // Muovi il nemico verso il target
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Controlla se il nemico ha raggiunto il target
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f) // Usa una piccola soglia per evitare oscillazioni
        {
            // Inverti il target e la direzione di movimento
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

            // Opzionale: puoi aggiungere qui un cambio di sprite o animazione per indicare il cambio di direzione
        }

        // Aggiorna l'orientamento dello sprite in base alla direzione di movimento
        Flip();
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
            speed = 2f;
        }
    }

    // Funzione per visualizzare i punti di riferimento nell'editor di Unity
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

    // Funzione per aggiornare l'orientamento dello sprite in base alla direzione del movimento
    private void Flip()
    {
        if (!movingRight)
        {
            // Si muove a sinistra, non flippare
            spriteRenderer.flipX = false;
        }
        else
        {
            // Si muove a destra, flippa orizzontalmente
            spriteRenderer.flipX = true;
        }
    }
}