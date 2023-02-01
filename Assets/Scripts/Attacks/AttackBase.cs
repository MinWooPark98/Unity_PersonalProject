using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AttackBase : ScriptableObject
{
    public int obtainGauge;
    public float distance;
    public float arrivalTime;
    public int damage;
    public int growthDamage;
    public bool isPenetrable = true;
    public float afterDelay;
    public AttackFollowUpBase followUp;
    public Projectile projectile;
}
