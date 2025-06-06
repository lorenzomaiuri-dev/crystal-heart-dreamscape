using System.Collections; 
using System.Collections.Generic;

using UnityEngine;



public class PlayerController : MonoBehaviour

{
    Animator animator;
    Rigidbody2D rb2d;
    RigidbodyConstraints2D rb2dConstraints;
    SpriteRenderer sprite;
    PlayerMovement playerMovement;
    PlayerActions playerActions;

    bool isTakingDamage;
    bool isInvincible;
    bool hitSideRight;
    
    bool freezePlayer;
    
    [Header("Health")]
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth = 10;
    [SerializeField] RectTransform hpBarFillRectTransformInstance;
    private float hpBarInitialFillWidth;

    [Header("Audio Clips")]
    [SerializeField] AudioClip takingDamageClip;
    [SerializeField] AudioClip explodeEffectClip;
    [SerializeField] AudioClip energyFillClip;

    [Header("Position and Prefabs")]
    [SerializeField] GameObject explodeEffectPrefab;

    void Awake()
    {
        // get handles to components
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        playerMovement =  GetComponent<PlayerMovement>();
        playerActions =  GetComponent<PlayerActions>();

    }
    

    // Start is called before the first frame update
    void Start()
    {
        // HpBarFill
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

    // Update is called once per frame
    void Update()

    {
        // taking damage from projectiles, touching enemies, or other environment objects

        if (isTakingDamage)

        {

            //animator.Play("Player_Hit");

            return;

        }

        playerMovement.HandleMovements();
        playerActions.HandleShooting();

    }

    public void ApplyLifeEnergy(int amount)

    {

        if (currentHealth < maxHealth)

        {

            int healthDiff = maxHealth - currentHealth;

            if (healthDiff > amount)

            {

                healthDiff = amount;

            }

            StartCoroutine(AddLifeEnergy(healthDiff));

        }

    }



    private IEnumerator AddLifeEnergy(int amount) 

    {

        //SoundManager.Instance.Play(energyFillClip, true);

        for (int i = 0; i < amount; i++)

        {

            currentHealth++;

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            UpdateHealthBar();

            yield return new WaitForSeconds(0.05f);

        }



        //SoundManager.Instance.Stop();

    }

    public void HitSide(bool rightSide)

    {
        hitSideRight = rightSide;
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
    
            currentHealth -= damage;
    
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            UpdateHealthBar();
            
    
            if (currentHealth <= 0)
    
            {
                Defeat();
            }
    
            else
    
            {
                StartDamageAnimation();
                StopDamageAnimation();
            }
    
        }
    
    }
    
    void StartDamageAnimation()
    
    {
        if (!isTakingDamage)
    
        {
    
            isTakingDamage = true;
    
            Invincible(true);
    
            float hitForceX = 0.50f;
    
            float hitForceY = 1.5f;
    
            if (hitSideRight) hitForceX = -hitForceX;
    
            rb2d.velocity = Vector2.zero;
    
            rb2d.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);
    
            //SoundManager.Instance.Play(takingDamageClip);
    
        }
    
    }
    
    
    
    void StopDamageAnimation()
    
    {
        isTakingDamage = false;
        //animator.Play("Player_Hit", -1, 0f);
        StartCoroutine(FlashAfterDamage());
    }
    
    private IEnumerator FlashAfterDamage()

    {
        float flashDelay = 0.0833f;

        // toggle transparency

        for (int i = 0; i < 10; i++)

        {

            //sprite.enabled = false;

            //sprite.material = null;

            //sprite.color = new Color(1, 1, 1, 0);

            sprite.color = Color.clear;

            yield return new WaitForSeconds(flashDelay);

            //sprite.enabled = true;

            //sprite.material = new Material(Shader.Find("Sprites/Default"));

            //sprite.color = new Color(1, 1, 1, 1);

            sprite.color = Color.white;

            yield return new WaitForSeconds(flashDelay);

        }

        // no longer invincible

        Invincible(false);
    }
    
    void StartDefeatAnimation()
    {
        // freeze player and input and go KABOOM!
        FreezePlayer(true);
    
        // GameObject explodeEffect = Instantiate(explodeEffectPrefab);
        //
        // explodeEffect.name = explodeEffectPrefab.name;
        //
        // explodeEffect.transform.position = sprite.bounds.center;
    
        //SoundManager.Instance.Play(explodeEffectClip);
    
        Destroy(gameObject);
    
    }
    
    
    
    void StopDefeatAnimation()
    
    {
        FreezePlayer(false);
    }



    void Defeat()
    {
        //Invoke("StartDefeatAnimation", 0.5f);
        GameManager.Instance.PlayerDefeated();
    }
    
    public void FreezePlayer(bool freeze)
    
    {
        // freeze/unfreeze the player on screen
        // zero animation speed and freeze XYZ rigidbody constraints
        if (freeze)
        {
    
            freezePlayer = true;
            rb2dConstraints = rb2d.constraints;
            animator.speed = 0;
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            freezePlayer = false;
            animator.speed = 1;
            rb2d.constraints = rb2dConstraints;
        }
    }
    
    void UpdateHealthBar()
    {
        if (hpBarFillRectTransformInstance != null)
        {
            float fillPercentage = (float)currentHealth / maxHealth;
            hpBarFillRectTransformInstance.sizeDelta = new Vector2(hpBarInitialFillWidth * fillPercentage, hpBarFillRectTransformInstance.sizeDelta.y);

            // Set anchor
            hpBarFillRectTransformInstance.anchorMax = new Vector2(fillPercentage, hpBarFillRectTransformInstance.anchorMax.y);
        }
        else
        {
            Debug.Log("Nessun hpBarFillRectTransformInstance trovato");
        }
    }
    
    public void Heal(int healAmount)
    {
        UpdateCurrentHealth(currentHealth + healAmount);
    }

    public void UpdateCurrentHealth(int newHp)
    {
        currentHealth = Mathf.Clamp(newHp, 0, maxHealth);
        UpdateHealthBar();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

}