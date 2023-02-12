using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowAttackBase.asset", menuName = "AttackBase/ThrowAttackBase")]
public class ThrowAttackBase : AttackBase
{
    public float throwHeight;

    public ThrowAttackBase()
    {
        attackShape = AttackShape.Throw;
    }
}
