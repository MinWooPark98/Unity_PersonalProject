using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontAttack : Attack
{
    public override void Execute(Transform attackPivot, Vector3 dir, float distanceRatio)
    {
        StartCoroutine(CoFire(attackPivot, dir, distanceRatio));
    }

    private IEnumerator CoFire(Transform attackPivot, Vector3 dir, float distanceRatio)
    {
        int count = 0;
        var thisBase = (FrontAttackBase)attackBase;
        while (true)
        {
            var pro = Instantiate(attackBase.projectile);
            pro.Set(gameObject, attackPivot.position, dir, thisBase.arrivalTime, thisBase.distance, thisBase.damage, followUp, thisBase.isPenetrable);
            if (DoAttack != null)
                DoAttack();
            ++count;
            if (count >= thisBase.maxCount)
                break;
            yield return new WaitForSeconds(thisBase.interval);
        }
        FinishAttack();
    }
}
