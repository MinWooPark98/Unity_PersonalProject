using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static Cinemachine.DocumentationSortingAttribute;

public abstract class Attack : MonoBehaviourPun
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

        int maxCount = 3;
        switch (this)
        {
            case FrontAttack:
                maxCount = ((FrontAttackBase)attackBase).maxCount * 3;
                break;
            case RangeAttack:
                maxCount = ((RangeAttackBase)attackBase).maxCount * 3;
                break;
            default:
                break;
        }
        projectilePool = new ObjectPool<Projectile>(CreateProjectile, null, OnReleaseProjectile, OnDestroyProjectile, maxSize: maxCount);
        Queue<Projectile> list = new Queue<Projectile>();
        for (int i = 0; i < maxCount; ++i)
        {
            list.Enqueue(projectilePool.Get());
        }
        for (int i = 0; i < list.Count; ++i)
        {
            var projectile = list.Dequeue();
            projectilePool.Release(projectile);
        }

        if (attackBase.followUp != null && attackBase.followUp.effectPrefab != null)
        {
            followUpEffectPool = new ObjectPool<ParticleEffect>(CreateFollowUpEffect, null, OnReleaseFollowUpEffect, OnDestroyFollowUpEffect, maxSize: maxCount * 3); ;
            followUp.SetPool(followUpEffectPool);
        }
    }

    private Projectile CreateProjectile()
    {
        Projectile projectile = PhotonNetwork.Instantiate(attackBase.projectile.name, Vector3.zero, Quaternion.identity).GetComponent<Projectile>();
        projectile.SetPool(projectilePool);
        bool isParabolic = false;
        float height = 0;
        switch (this)
        {
            case ThrowAttack:
                isParabolic = true;
                height = ((ThrowAttackBase)attackBase).throwHeight;
                break;
            default:
                break;
        }
        projectile.SetBase(attackBase.obtainGauge, attackBase.arrivalTime, attackBase.distance, attackBase.damage, followUp, attackBase.isPenetrable, isParabolic, height);
        return projectile;
    }
    private void OnReleaseProjectile(Projectile projectile) => projectile.SetActiveOnServer(false);
    private void OnDestroyProjectile(Projectile projectile) => PhotonNetwork.Destroy(projectile.gameObject);

    private ParticleEffect CreateFollowUpEffect()
    {
        ParticleEffect effect = PhotonNetwork.Instantiate(attackBase.followUp.effectPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<ParticleEffect>(); ;
        effect.SetPool(followUpEffectPool);
        return effect;
    }
    private void OnReleaseFollowUpEffect(ParticleEffect effect) => effect.SetActiveOnServer(false);
    private void OnDestroyFollowUpEffect(ParticleEffect effect) => PhotonNetwork.Destroy(effect.gameObject);

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
