using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AttackBase : ScriptableObject
{
    public enum AttackType
    {
        Basic,
        Skill,
    }
    public enum AttackShape
    {
        None = -1,
        Front,
        Range,
        Throw,
    }
    public Characters character;
    public AttackType attackType;
    public AttackShape attackShape { get; protected set; } = AttackShape.None;
    public int obtainGauge;
    public int distance;
    public float arrivalTime;
    public int damage;
    public int growthDamage;
    public bool isPenetrable = true;
    public bool isBreakable;
    public float afterDelay;
    public AttackFollowUpBase followUp;
    public Projectile projectile;
}
