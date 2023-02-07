using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackFollowUpBase : ScriptableObject
{
    public int obtainGauge;
    public int damage;
    public int growthDamage;
    public bool isBreakable;
    public ParticleEffect effectPrefab;
}
