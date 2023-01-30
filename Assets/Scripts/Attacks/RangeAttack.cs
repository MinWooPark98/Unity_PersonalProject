using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : Attack
{
    public override void Execute(Transform attackPivot, Vector3 dir, float distanceRatio)
    {
        var thisBase = (RangeAttackBase)attackBase;
        var leftDir = Quaternion.AngleAxis(-thisBase.angle * 0.5f, Vector3.up) * dir;
        var rightDir = Quaternion.AngleAxis(thisBase.angle * 0.5f, Vector3.up) * dir;
        for (int i = 0; i < thisBase.maxCount; ++i)
        {
            var pro = Instantiate(attackBase.projectile);
            var newDir = Vector3.Lerp(leftDir, rightDir, (float)i / thisBase.maxCount);
            var newDistance = thisBase.distance * Random.Range(0.9f, 1.1f);
            pro.Set(gameObject, attackPivot.position, newDir, thisBase.arrivalTime, newDistance, thisBase.damage, followUp, thisBase.isPenetrable);
        }
        if (DoAttack != null)
            DoAttack();
        FinishAttack();
    }
}
