using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAttack : Attack
{
    public override void Execute(Transform attackPivot, Vector3 dir, int level, float distanceRatio)
    {
        var thisBase = (ThrowAttackBase)attackBase;
        var damage = thisBase.damage + thisBase.growthDamage * level;
        var pro = Instantiate(attackBase.projectile);
        pro.Set(gameObject, attackPivot.position, dir, thisBase.obtainGauge, thisBase.arrivalTime,
            thisBase.distance * distanceRatio, damage, level, followUp, thisBase.isPenetrable, true, thisBase.throwHeight);
        if (DoAttack != null)
            DoAttack();
        FinishAttack();
    }
}
