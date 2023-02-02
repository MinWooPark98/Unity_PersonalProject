using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class AreaAttack : AttackFollowUp
{
    public AreaAttack(AttackFollowUpBase thisBase)
    {
        followUpBase = thisBase;
    }

    public override void Execute(GameObject attacker, Vector3 pos, int level)
    {
        var thisBase = (AreaAttackBase)followUpBase;
        var effect = effectPool.Get();
        effect.transform.position = pos;
        effect.transform.localScale = new Vector3(thisBase.radius * 2f, thisBase.radius * 2f, 0f);
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
