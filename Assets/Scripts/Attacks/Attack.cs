using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] protected AttackBase attackBase;
    protected AttackFollowUp followUp;
    public Action EndAttack;

    private void Start()
    {
        if (attackBase == null)
            return;
        switch (attackBase.followUp)
        {
            case AreaAttackBase:
                followUp = new AreaAttack();
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
            yield return new WaitForSeconds(attackBase.actionTime);
            EndAttack();
        }
    }
}
