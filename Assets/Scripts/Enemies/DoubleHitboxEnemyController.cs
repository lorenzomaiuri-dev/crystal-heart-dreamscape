using System.Collections;
using UnityEngine;

public class DoubleHitboxEnemyController : EnemyController
{
    public Collider2D headHitbox;
    public Collider2D chestHitbox;

    [SerializeField] private float switchInterval = 3f;

    private HitboxDelegator.HitboxType currentState;

    void Start()
    {
        base.Start();
        StartCoroutine(SwitchHitboxRoutine());
        ActivateHitbox(HitboxDelegator.HitboxType.HitboxA);
    }

    private IEnumerator SwitchHitboxRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchInterval);
            SwitchHitbox();
        }
    }

    private void SwitchHitbox()
    {
        ActivateHitbox(currentState == HitboxDelegator.HitboxType.HitboxA ? HitboxDelegator.HitboxType.HitboxB : HitboxDelegator.HitboxType.HitboxA);
    }

    private void ActivateHitbox(HitboxDelegator.HitboxType state)
    {
        currentState = state;
        headHitbox.enabled = (state == HitboxDelegator.HitboxType.HitboxA);
        chestHitbox.enabled = (state == HitboxDelegator.HitboxType.HitboxB);
    }

    public void TakeDamageFromHitbox(HitboxDelegator.HitboxType sourceHitbox, int damage)
    {
        if (sourceHitbox == currentState)
        {
            TakeDamage(damage);
        }
        else
        {
            // SoundManager.Instance.Play(blockAttackClip);
        }
    }
}