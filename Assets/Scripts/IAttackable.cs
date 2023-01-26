using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public bool Execute(Transform attackPivot, Vector3 dir, float distanceRatio);
}
