using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AreaAttack : AttackFollowUp
{
    // ����Ʈ ������

    public override void Execute(Vector3 pos)
    {
        var thisBase = (AreaAttackBase)followUpBase;
        var cols = Physics.OverlapSphere(pos, thisBase.radius); // �� �ȿ� ����ִ� ��� colliders return
    }
}