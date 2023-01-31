using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Attack : MonoBehaviour
{
    public AttackBase attackBase;
    protected AttackFollowUp followUp;
    public Action DoAttack;
    public Action EndAttack;

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
    }

    public abstract void Execute(Transform attackPivot, Vector3 dir, float distanceRatio);

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
                indicator = GetComponent<SquareIndicator>();
                break;
            case RangeAttack:
                indicator = GetComponent<SectorIndicator>();
                break;
            case ThrowAttack:
                break;
        }
        indicator?.DrawRange(dir, this);
    }

    public void StopShowRange()
    {
        AttackIndicator indicator = null;
        switch (this)
        {
            case FrontAttack:
                indicator = GetComponent<SquareIndicator>();
                break;
            case RangeAttack:
                indicator = GetComponent<SectorIndicator>();
                break;
            case ThrowAttack:
                break;
        }
        indicator?.Clear();
    }
}
