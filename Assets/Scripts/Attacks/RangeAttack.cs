using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : Attack
{
    public override bool Execute(Transform attackPivot, Vector3 dir, float distanceRatio)
    {
        if (!base.Execute(attackPivot, dir, distanceRatio))
            return false;
        var thisBase = (RangeAttackBase)attackBase;
        var leftDir = Quaternion.AngleAxis(-thisBase.angle * 0.5f, Vector3.up) * dir;
        var rightDir = Quaternion.AngleAxis(thisBase.angle * 0.5f, Vector3.up) * dir;
        for (int i = 0; i < thisBase.maxCount; ++i)
        {
            var pro = Instantiate(attackBase.projectile);
            var newDir = Vector3.Lerp(leftDir, rightDir, (float)i / thisBase.maxCount);
            var newDistance = thisBase.distance * distanceRatio * Random.Range(0.9f, 1.1f);
            pro.Set(gameObject, attackPivot.position, newDir, thisBase.arrivalTime, newDistance, thisBase.damage, thisBase.followAttack, thisBase.isPenetrable);
        }
        --savedCount;
        isInCoolDown = true;
        return true;
    }
}
