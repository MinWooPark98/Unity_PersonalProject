using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAttack : Attack
{
    public override bool Execute(Transform attackPivot, Vector3 dir, float distanceRatio)
    {
        if (!base.Execute(attackPivot, dir, distanceRatio))
            return false;
        var thisBase = (ThrowAttackBase)attackBase;
        var pro = Instantiate(attackBase.projectile);
        pro.Set(gameObject, attackPivot.position, dir, thisBase.arrivalTime, thisBase.distance * distanceRatio, thisBase.damage, thisBase.followAttack, thisBase.isPenetrable, true, thisBase.throwHeight);
        --savedCount;
        isInCoolDown = true;
        return true;
    }
}
