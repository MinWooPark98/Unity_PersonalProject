using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Attack : MonoBehaviour, IAttackable
{
    [SerializeField] protected AttackBase attackBase;
    public int savedCount;
    protected float saveTimer;
    protected bool isInCoolDown = false;
    protected float coolTimer;
    public Action EndAttack;

    private void Start()
    {
        savedCount = attackBase.maxSave;
    }

    private void Update()
    {
        if (savedCount < attackBase.maxSave)
        {
            saveTimer += Time.deltaTime;
            if (saveTimer >= attackBase.saveDelay)
            {
                saveTimer = 0f;
                ++savedCount;
            }
        }

        if (isInCoolDown)
        {
            coolTimer += Time.deltaTime;
            if (coolTimer >= attackBase.coolDown)
            {
                coolTimer = 0f;
                isInCoolDown = false;
            }
        }
    }

    public virtual bool Execute(Transform attackPivot, Vector3 dir, float distanceRatio) => savedCount >= 1;
}
