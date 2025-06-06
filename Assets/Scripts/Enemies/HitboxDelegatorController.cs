using UnityEngine;

public class HitboxDelegator : MonoBehaviour
{
    public enum HitboxType { HitboxA, HitboxB }
    public HitboxType type;

    private DoubleHitboxEnemyController bossController;

    void Start()
    {
        bossController = GetComponentInParent<DoubleHitboxEnemyController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: SPOSTARE IN BULLET CONTROLLER CON TAG DEDICATO TIPO DAMAGABLE
        if (collision.CompareTag("Bullet"))
        {
            BulletController bullet = collision.GetComponent<BulletController>();
            if (bullet != null)
            {
                bossController.TakeDamageFromHitbox(type, bullet.GetDamageValue());
                Destroy(collision.gameObject);
            }
        }
    }
}