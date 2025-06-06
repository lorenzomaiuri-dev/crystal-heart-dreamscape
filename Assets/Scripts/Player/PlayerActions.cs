using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    PlayerMovement playerMovement;
    
    [Header("Bullet")]
    [SerializeField] int bulletDamage = 1;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float bulletDestroyDelay = 5f;
    [SerializeField] float shootDurationCooldown = 0.25f;
    
    [Header("Position and Prefabs")]
    [SerializeField] Transform bulletShootPos;
    [SerializeField] GameObject bulletPrefab;
    
    [Header("Audio Clips")]
    [SerializeField] AudioClip shootBulletClip;
    
    bool isShooting;
    float shootTime;
    bool keyShootRelease;
    
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    
    public void HandleShooting()
    {
        if (InputManager.Instance.shootInput && keyShootRelease)
        {
            isShooting = true;
            keyShootRelease = false;
            shootTime = Time.time;

            Invoke("ShootBullet", 0.1f);
        }

        if (!InputManager.Instance.shootInput && !keyShootRelease)
        {
            float releaseDuration = Time.time - shootTime;
            keyShootRelease = true;
        }

        if (isShooting)
        {
            float shootDuration = Time.time - shootTime;
            if (shootDuration >= shootDurationCooldown)
            {
                isShooting = false;
            }
        }
    }
    
    public void ShootBullet()
    {
        // create bullet from prefab gameobject

        GameObject bullet = Instantiate(bulletPrefab, bulletShootPos.position, Quaternion.identity);

        // set its name to that of the prefab so it doesn't include "(Clone)" when instantiated

        bullet.name = bulletPrefab.name;

        // set bullet damage amount, speed, direction bullet will travel along the x, and fire!

        //bullet.GetComponent<BulletController>().SetDamageValue(bulletDamage);
        
        //bullet.GetComponent<BulletController>().SetBulletSpeed(bulletSpeed);
        
        bullet.GetComponent<BulletController>().SetBulletDirection((playerMovement.IsFacingRight) ? Vector2.right : Vector2.left);
        
        bullet.GetComponent<BulletController>().SetDestroyDelay(bulletDestroyDelay);
        
        bullet.GetComponent<BulletController>().Shoot();

        //SoundManager.Instance.Play(shootBulletClip);

    }
}
