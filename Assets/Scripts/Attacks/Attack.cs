using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Attack : MonoBehaviour
{
    public AttackBase attackBase;
    protected AttackFollowUp followUp;
    public Action DoAttack;
    public Action EndAttack;
    protected IObjectPool<Projectile> projectilePool;
    protected IObjectPool<ParticleEffect> followUpEffectPool;

    private void Start()
    {
        switch (attackBase.followUp)
        {
            case AreaAttackBase:
                followUp = new AreaAttack(attackBase.followUp);
                break;
            case SummonBase:
                followUp = new Summon(attackBase.followUp);
                break;
        }

        int maxCount = 1;
        switch (this)
        {
            case FrontAttack:
                maxCount = ((FrontAttackBase)attackBase).maxCount;
                break;
            case RangeAttack:
                maxCount = ((RangeAttackBase)attackBase).maxCount;
                break;
            default:
                break;
        }
        projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, maxSize: maxCount * 3);
        if (attackBase.followUp.effectPrefab != null)
        {
            followUpEffectPool = new ObjectPool<ParticleEffect>(CreateFollowUpEffect, OnGetFollowUpEffect, OnReleaseFollowUpEffect, OnDestroyFollowUpEffect, maxSize: maxCount * 3); ;
            followUp.SetPool(followUpEffectPool);
        }
    }

    private Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(attackBase.projectile);
        projectile.SetPool(projectilePool);
        return projectile;
    }
    private void OnGetProjectile(Projectile projectile) => projectile.gameObject.SetActive(true);
    private void OnReleaseProjectile(Projectile projectile) => projectile.gameObject.SetActive(false);
    private void OnDestroyProjectile(Projectile projectile) => Destroy(projectile.gameObject);

    private ParticleEffect CreateFollowUpEffect()
    {
        ParticleEffect effect = Instantiate(attackBase.followUp.effectPrefab);
        effect.SetPool(followUpEffectPool);
        return effect;
    }
    private void OnGetFollowUpEffect(ParticleEffect effect) => effect.gameObject.SetActive(true);
    private void OnReleaseFollowUpEffect(ParticleEffect effect) => effect.gameObject.SetActive(false);
    private void OnDestroyFollowUpEffect(ParticleEffect effect) => Destroy(effect.gameObject);

    public abstract void Execute(Transform attackPivot, Vector3 dir, int level, float distanceRatio);

    protected void FinishAttack()
    {
        StartCoroutine(CoFinishAttack());
    }

    private IEnumerator CoFinishAttack()
    {
        if (EndAttack != null)
        {
            yield return new WaitForSeconds(attackBase.afterDelay);
            EndAttack();
        }
    }

    public void ShowRange(Vector3 dir)
    {
        AttackIndicator indicator = null;
        switch (this)
        {
            case FrontAttack:
                indicator = GetComponent<FrontIndicator>();
                indicator?.DrawRange(dir.normalized, this);
                break;
            case RangeAttack:
                indicator = GetComponent<RangeIndicator>();
                indicator?.DrawRange(dir.normalized, this);
                break;
            case ThrowAttack:
                indicator = GetComponent<ThrowIndicator>();
                indicator?.DrawRange(dir, this);
                break;
        }
    }

    public void StopShowRange()
    {
        AttackIndicator indicator = null;
        switch (this)
        {
            case FrontAttack:
                indicator = GetComponent<FrontIndicator>();
                break;
            case RangeAttack:
                indicator = GetComponent<RangeIndicator>();
                break;
            case ThrowAttack:
                indicator = GetComponent<ThrowIndicator>();
                break;
        }
        indicator?.Clear();
    }
}
