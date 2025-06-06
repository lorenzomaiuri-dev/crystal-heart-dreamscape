using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// all enemies will require these components
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyController : MonoBehaviour
{
    Animator animator;
    BoxCollider2D box2d;
    Rigidbody2D rb2d;
    SpriteRenderer sprite;

    bool isInvincible;

    GameObject explodeEffect;

    RigidbodyConstraints2D rb2dConstraints;

    public bool freezeEnemy;
    public int scorePoints = 500;

    [Header("Health")] [SerializeField] int currentHealth;
    [SerializeField] int maxHealth = 1;
    [SerializeField] RectTransform hpBarFillRectTransformInstance;
    private float hpBarInitialFillWidth;

    [Header("Damage")] [SerializeField] public int contactDamage = 1;
    [SerializeField] int explosionDamage = 0;

    [Header("Bullet")] [SerializeField] public int bulletDamage = 1;
    [SerializeField] public float bulletSpeed = 3f;
    [SerializeField] public float fireRate = 3f;
    public float fireTimer;
    [SerializeField] public int shotsNumber = 3;
    [SerializeField] public float burstInterval = 0.2f;
    private int shotsFired;

    [Header("Bonus Item Settings")] [Tooltip("Prefab dell'oggetto da rilasciare.")] [SerializeField]
    GameObject bonusItemPrefab;

    [Tooltip("Probabilità (da 0 a 1) che rilasci un oggetto alla morte.")] [Range(0f, 1f)] [SerializeField]
    float bonusItemSpawnRate = 1f;

    [Header("Audio Clips")] [SerializeField]
    AudioClip damageClip;

    [SerializeField] AudioClip blockAttackClip;
    [SerializeField] public AudioClip shootBulletClip;
    [SerializeField] AudioClip energyFillClip;

    [Header("Positions and Prefabs")] [SerializeField]
    public Transform bulletShootPos;

    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] GameObject explodeEffectPrefab;

    protected void Awake()
    {
        // get handles to components
        animator = GetComponent<Animator>();
        box2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        // start at full health
        currentHealth = maxHealth;
        fireTimer = 0f;
        shotsFired = 0;

        // Create hpBarFill
        if (hpBarFillRectTransformInstance != null)
        {
            hpBarInitialFillWidth = hpBarFillRectTransformInstance.rect.width;
            UpdateHealthBar();
        }
        else
        {
            Debug.LogError("Health Bar Prefab non assegnato a " + gameObject.name);
        }
    }

    protected void Update()
    {
        // shooting
        if (bulletShootPos != null && bulletPrefab != null)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0)
            {
                if (shotsFired < shotsNumber)
                {
                    ShootBullet();
                    fireTimer = burstInterval;
                    shotsFired++;
                }
                else
                {
                    fireTimer = fireRate;
                    shotsFired = 0;
                }
            }
        }
    }

    public void Flip()
    {
        transform.Rotate(0, 180f, 0);
    }

    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    public void TakeDamage(int damage)
    {
        // take damage if not invincible
        if (!isInvincible)
        {
            // take damage amount from health and call defeat if no health
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            // SoundManager.Instance.Play(damageClip);
            if (currentHealth <= 0)
            {
                Defeat();
            }

            UpdateHealthBar();
        }
        else
        {
            // block attack soun
            // SoundManager.Instance.Play(blockAttackClip);
        }
    }

    void StartDefeatAnimation()
    {
        // play explosion animation
        //   create copy of prefab, place its spawn location at center of sprite, 
        //   set explosion damage value (if any), destroy after two seconds
        //explodeEffect = Instantiate(explodeEffectPrefab);
        //explodeEffect.name = explodeEffectPrefab.name;
        //explodeEffect.transform.position = sprite.bounds.center;
        // explodeEffect.GetComponent<ExplosionScript>().SetDamageValue(this.explosionDamage);
        Destroy(explodeEffect, 2f);
    }

    void StopDefeatAnimation()
    {
        Destroy(explodeEffect);
    }

    void Defeat()
    {
        // play explosion animation
        StartDefeatAnimation();
        TryDropItem();

        Destroy(gameObject);
        // GameManager.Instance.AddScorePoints(this.scorePoints);
    }

    public void FreezeEnemy(bool freeze)
    {
        // freeze/unfreeze the enemy on screen
        // zero animation speed and freeze XYZ rigidbody constraints
        // this will be called from the GameManager but could be used in other scripts
        if (freeze)
        {
            freezeEnemy = true;
            animator.speed = 0;
            rb2dConstraints = rb2d.constraints;
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            freezeEnemy = false;
            animator.speed = 1;
            rb2d.constraints = rb2dConstraints;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Damage Player
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(this.contactDamage);
            }
        }
    }

    void UpdateHealthBar()
    {
        if (hpBarFillRectTransformInstance != null)
        {
            float fillPercentage = (float)currentHealth / maxHealth;
            hpBarFillRectTransformInstance.sizeDelta = new Vector2(hpBarInitialFillWidth * fillPercentage,
                hpBarFillRectTransformInstance.sizeDelta.y);

            // Set anchor
            hpBarFillRectTransformInstance.anchorMax =
                new Vector2(fillPercentage, hpBarFillRectTransformInstance.anchorMax.y);
        }
        else
        {
            Debug.Log("Nessun hpBarFillRectTransformInstance trovato");
        }
    }

    void ShootBullet()
    {
        if (bulletShootPos != null && bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletShootPos.position, bulletShootPos.rotation);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (bulletController != null)
            {
                bulletController.SetBulletType(BulletController.BulletTypes.Default);
                bulletController.SetDamageValue(bulletDamage);
                bulletController.SetBulletSpeed(bulletSpeed);

                Vector2 shootDirection = Vector2.left;
                bulletController.SetBulletDirection(shootDirection);

                bulletController.SetCollideWithTags("Player");
                bulletController.SetDestroyDelay(5f);
                bulletController.Shoot();
            }
            //SoundManager.Instance.Play(enemyController.shootBulletClip);
        }
    }

    private void TryDropItem()
    {
        if (bonusItemPrefab != null)
        {
            float randomValue = Random.value;

            if (randomValue <= bonusItemSpawnRate)
            {
                Instantiate(bonusItemPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}