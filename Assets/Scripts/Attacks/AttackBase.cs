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
    public int maxSave;
    public float saveDelay;
    public float actionTime;
    public float coolDown;
    public UnityEvent followAttack;

    public Projectile projectile;   // ������ƮǮ�� �Űܰ� ����
}
