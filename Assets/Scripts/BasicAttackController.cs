using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackController : MonoBehaviour
{
    public BasicAttackControllerBase basicControllerBase;
    public Attack attack;
    private int savedCount;
    private float saveTimer;
    private bool isInCoolDown = false;
    private float coolTimer;
    public bool attackable { get; private set; }
    public float SavedRatio
    {
        get
        {
            var onceRatio = 1f / basicControllerBase.maxSave;
            return (savedCount + saveTimer / basicControllerBase.saveDelay) * onceRatio;
        }
    }

    private void Start()
    {
        savedCount = basicControllerBase.maxSave;
        attackable = true;
    }

    private void Update()
    {
        if (savedCount < basicControllerBase.maxSave)
        {
            saveTimer += Time.deltaTime;
            if (saveTimer >= basicControllerBase.saveDelay)
            {
                saveTimer = 0f;
                ++savedCount;
                if (!isInCoolDown)
                    attackable = true;
            }
        }

        if (isInCoolDown)
        {
            coolTimer += Time.deltaTime;
            if (coolTimer >= basicControllerBase.coolDown)
            {
                coolTimer = 0f;
                isInCoolDown = false;
                if (savedCount > 0)
                    attackable = true;
            }
        }
    }

    public void ShowAttackRange(Vector3 dir)
    {
        attack.ShowRange(dir);
    }

    public void StopShowAttackRange()
    {
        attack.StopShowRange();
    }

    public void ExecuteAttack(Transform attackPivot, Vector3 dir, float distanceRatio)
    {
        if (!attackable)
            return;
        ExeSucceed();
        attack.Execute(attackPivot, dir, distanceRatio);
    }

    private void ExeSucceed()
    {
        isInCoolDown = true;
        --savedCount;
        attackable = false;
    }
}
