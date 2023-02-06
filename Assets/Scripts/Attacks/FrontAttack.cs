using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontAttack : Attack
{
    public override void Execute(Transform attackPivot, Vector3 dir, int level, float distanceRatio)
    {
        StartCoroutine(CoFire(attackPivot, dir, level));
    }

    private IEnumerator CoFire(Transform attackPivot, Vector3 dir, int level)
    {
        int count = 0;
        var thisBase = (FrontAttackBase)attackBase;
        var damage = thisBase.damage + thisBase.growthDamage * level;
        while (true)
        {
            var pro = projectilePool.Get();
            pro.Set(gameObject, attackPivot.position, dir, level);
            pro.SetActiveOnServer(true);
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
