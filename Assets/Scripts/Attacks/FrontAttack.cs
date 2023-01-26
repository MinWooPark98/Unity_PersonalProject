using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontAttack : Attack
{
    public override bool Execute(Transform attackPivot, Vector3 dir, float distanceRatio)
    {
        if (!base.Execute(attackPivot, dir, distanceRatio))
            return false;
        StartCoroutine(CoFire(attackPivot, dir, distanceRatio));
        --savedCount;
        return true;
    }

    private IEnumerator CoFire(Transform attackPivot, Vector3 dir, float distanceRatio)
    {
        int count = 0;
        var thisBase = (FrontAttackBase)attackBase;
        while (true)
        {
            var pro = Instantiate(attackBase.projectile);
            pro.Set(gameObject, attackPivot.position, dir, thisBase.arrivalTime, thisBase.distance * distanceRatio, thisBase.damage, thisBase.followAttack, thisBase.isPenetrable);
            ++count;
            if (count >= thisBase.maxCount)
                break;
            yield return new WaitForSeconds(thisBase.interval);
        }
        isInCoolDown = true;
    }
}
