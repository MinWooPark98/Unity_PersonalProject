using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAttack : Attack
{
    public override void Execute(Transform attackPivot, Vector3 dir, float distanceRatio)
    {
        var thisBase = (ThrowAttackBase)attackBase;
        var pro = Instantiate(attackBase.projectile);
        pro.Set(gameObject, attackPivot.position, dir, thisBase.arrivalTime, thisBase.distance * distanceRatio, thisBase.damage, followUp, thisBase.isPenetrable, true, thisBase.throwHeight);
        if (DoAttack != null)
            DoAttack();
        FinishAttack();
    }
}
