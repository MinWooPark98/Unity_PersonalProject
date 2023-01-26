using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaAttackBase.asset", menuName = "AttackFollowUpBase/AreaAttackBase")]
public class AreaAttackBase : AttackFollowUpBase
{
    public int damage;
    public float radius;
    // 이펙트 등 추가
}
