using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class AttackFollowUp
{
    protected AttackFollowUpBase followUpBase;
    protected IObjectPool<ParticleEffect> effectPool;

    public void SetPool(IObjectPool<ParticleEffect> pool) => effectPool = pool;
    public abstract void Execute(GameObject attacker, Vector3 pos, int level);
}
