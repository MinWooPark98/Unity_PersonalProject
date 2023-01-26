using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AttackBase : ScriptableObject
{
    public float distance;
    public float arrivalTime;
    public int damage;
    public bool isPenetrable = true;
    public float actionTime;
    public AttackFollowUpBase followUp;
    public Projectile projectile;
}
