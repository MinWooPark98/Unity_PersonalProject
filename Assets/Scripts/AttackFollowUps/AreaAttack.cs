using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AreaAttack : AttackFollowUp
{
    // ����Ʈ ������

    public AreaAttack(AttackFollowUpBase thisBase)
    {
        followUpBase = thisBase;
    }

    public override void Execute(GameObject attacker, Vector3 pos, int level)
    {
        var thisBase = (AreaAttackBase)followUpBase;
        var cols = Physics.OverlapSphere(pos, thisBase.radius); // �� �ȿ� ����ִ� ��� colliders return
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
