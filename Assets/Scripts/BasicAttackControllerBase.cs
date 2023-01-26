using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttackControllerBase", menuName = "AttackControllerBase/BasicAttackControllerBase")]
public class BasicAttackControllerBase : ScriptableObject
{
    public int maxSave;
    public float saveDelay;
    public float coolDown;
}
