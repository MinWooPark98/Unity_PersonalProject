using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public abstract class Attack : MonoBehaviour
{
    public AttackBase attackBase;
    protected AttackFollowUp followUp;
    public Action DoAttack;
    public Action EndAttack;
    protected IObjectPool<Projectile> projectilePool;

    private void Start()
    {
        if (attackBase == null)
            return;
        switch (attackBase.followUp)
        {
            case AreaAttackBase:
                followUp = new AreaAttack(attackBase.followUp);
                break;
            case SummonBase:
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
