using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AreaAttack : AttackFollowUp
{
    // 이펙트 프리펩

    public AreaAttack(AttackFollowUpBase thisBase)
    {
        followUpBase = thisBase;
    }

    public override void Execute(Vector3 pos)
    {
        var thisBase = (AreaAttackBase)followUpBase;
        var cols = Physics.OverlapSphere(pos, thisBase.radius); // 구 안에 들어있는 모든 colliders return
        foreach (var col in cols)
        {
            col.GetComponent<Health>()?.OnDamage(thisBase.damage);
        }
    }
}
