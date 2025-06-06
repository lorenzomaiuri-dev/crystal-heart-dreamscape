using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PharaohEnemy : MonoBehaviour
{
    Rigidbody2D rb2d;

    [Header("Movimento")]
    public float jumpForce = 5f;
    public float jumpInterval = 2f;
    private float jumpTimer;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        jumpTimer = jumpInterval;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        
        jumpTimer -= Time.deltaTime;
        if (isGrounded && jumpTimer <= 0)
        {
            Jump();
        }
        
    }

    void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        jumpTimer = jumpInterval;
    }
}