using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAttack : Attack
{
    public override void Execute(Transform attackPivot, Vector3 dir, int level, float distanceRatio)
    {
        var thisBase = (ThrowAttackBase)attackBase;
        var damage = thisBase.damage + thisBase.growthDamage * level;
        var pro = projectilePool.Get();
        pro.Set(gameObject, attackPivot.position, dir, damage, level);
        pro.SetActiveOnServer(true);
        if (DoAttack != null)
            DoAttack();
        FinishAttack();
    }
}
