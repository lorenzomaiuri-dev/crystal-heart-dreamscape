using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    CapsuleCollider2D box2d;
    Rigidbody2D rb2d;
    RigidbodyConstraints2D rb2dConstraints;
    SpriteRenderer sprite;

    private int jumpCount = 0;
    public int maxJumps = 1;

    bool isGrounded;
    bool isJumping;
    bool isShooting;
    bool isTeleporting;
    bool isTakingDamage;
    bool isInvincible;
    bool isFacingRight = true;
    bool isDashing;
    bool hitSideRight;
    
    [Header("Movement")]
    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float jumpSpeed = 3.7f;    
    
    [Header("Dash")]
    [SerializeField] float dashSpeedMultiplier = 2.0f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 1.0f;
    [SerializeField] float lastDashTime = -1f;

    [Header("Audio Clips")]
    [SerializeField] AudioClip jumpLandedClip;
    
    public void SetShooting(bool value)
    {
        isShooting = value;
    }

    
    void Awake()
    {
        // get handles to components
        animator = GetComponent<Animator>();
        box2d = GetComponent<CapsuleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    
    private void FixedUpdate()
    {
        PerformGroundCheck();
        DrawGroundCheckDebug();
    }

    public void HandleMovements()
    {
        HandleMovement();
        HandleDash();
        HandleJump();
        HandleAirborneAnimation();
    }
    
    private void PerformGroundCheck()
    {
        isGrounded = false;

        float raycastDistance = 0.05f;
        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        Vector3 box_origin = box2d.bounds.center;
        box_origin.y = box2d.bounds.min.y + (box2d.bounds.extents.y / 4f);

        Vector3 box_size = box2d.bounds.size;
        box_size.y = box2d.bounds.size.y / 4f;

        RaycastHit2D raycastHit = Physics2D.BoxCast(
            box_origin, 
            box_size, 
            0f, 
            Vector2.down, 
            raycastDistance, 
            layerMask
        );

        if (raycastHit.collider != null)
        {
            isGrounded = true;

            if (isJumping)
            {
                OnLanding(); // Reset
                //SoundManager.Instance.Play(jumpLandedClip);
            }
        }
    }

    private void DrawGroundCheckDebug()
    {
        float raycastDistance = 0.05f;
        Vector3 box_origin = box2d.bounds.center;
        box_origin.y = box2d.bounds.min.y + (box2d.bounds.extents.y / 4f);

        Color raycastColor = isGrounded ? Color.green : Color.red;

        Debug.DrawRay(
            box_origin + new Vector3(box2d.bounds.extents.x, 0),
            Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance),
            raycastColor
        );

        Debug.DrawRay(
            box_origin - new Vector3(box2d.bounds.extents.x, 0),
            Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance),
            raycastColor
        );

        Debug.DrawRay(
            box_origin - new Vector3(box2d.bounds.extents.x, box2d.bounds.extents.y / 4f + raycastDistance),
            Vector2.right * (box2d.bounds.extents.x * 2),
            raycastColor
        );
    }

    
    private void HandleMovement()
    {
        if (isDashing) return;

        float moveDirection = InputManager.Instance.horizontalInput;

        if (moveDirection != 0)
        {
            if ((isFacingRight && moveDirection < 0) || (!isFacingRight && moveDirection > 0))
            {
                Flip();
            }

            if (isGrounded)
            {
                PlayAnimation(isShooting ? "PlayerShooting" : "PlayerRunning");
            }

            rb2d.velocity = new Vector2(moveSpeed * moveDirection, rb2d.velocity.y);
        }
        else
        {
            if (isGrounded)
            {
                PlayAnimation(isShooting ? "PlayerShooting" : "PlayerIdle");
            }

            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
        }
    }

    private void HandleDash()
    {
        if (InputManager.Instance.dashInput && PowerUpManager.Instance.hasDash)
        {
            if (Time.time >= lastDashTime + dashCooldown && !isDashing)
            {
                StartCoroutine(Dash());
            }
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        // Puoi anche salvare la velocità attuale se vuoi ripristinarla dopo
        //Vector2 originalVelocity = rb2d.velocity;

        Vector2 dashDirection = new Vector2(isFacingRight ? 1f : -1f, 0f);
        rb2d.velocity = dashDirection * dashSpeedMultiplier;

        // Se vuoi, disattiva la gravità per un dash più “secco”
        float originalGravity = rb2d.gravityScale;
        rb2d.gravityScale = 0;

        yield return new WaitForSeconds(dashDuration);

        // Fine dash
        isDashing = false;
        rb2d.gravityScale = originalGravity;

        // Non azzerare Y, così la gravità continua il suo lavoro
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
    }

    private void HandleJump()
    {
        if (InputManager.Instance.jumpInput && jumpCount < maxJumps)
        {
            PlayAnimation(isShooting ? "PlayerShooting" : "PlayerRunning");
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            jumpCount++;
            
            if (PowerUpManager.Instance.hasDoubleJump && jumpCount == 1)
            {
                maxJumps = 2;
            }
        }
    }

    private void OnLanding()
    {
        // reset
        isJumping = false;
        jumpCount = 0;
        maxJumps = 1;
    }

    private void HandleAirborneAnimation()
    {
        if (!isGrounded)
        {
            isJumping = true;
            PlayAnimation(isShooting ? "PlayerShooting" : "PlayerRunning");
        }
    }

    private void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        
        Vector3 localScale = transform.localScale;
        localScale.x = -localScale.x;
        transform.localScale = localScale;
    }
    
    public bool IsFacingRight { get { return isFacingRight; } }

}