using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FrontAttackBase.asset", menuName = "AttackBase/FrontAttackBase")]
public class FrontAttackBase : AttackBase
{
    public int maxCount;
    public float interval;

    public FrontAttackBase()
    {
        attackShape = AttackShape.Front;
    }
}
