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

    public override void Execute(GameObject attacker, Vector3 pos, int level)
    {
        var thisBase = (AreaAttackBase)followUpBase;
        var cols = Physics.OverlapSphere(pos, thisBase.radius); // 구 안에 들어있는 모든 colliders return
        var damage = thisBase.damage + thisBase.growthDamage * level;
        foreach (var col in cols)
        {
            var health = col.GetComponent<Health>();
            if (health != null && col.gameObject != attacker)
            {
                health.OnDamage(damage);
            }
        }
    }
}
