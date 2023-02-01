using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackController : MonoBehaviour
{
    public SkillAttackControllerBase skillControllerBase;
    public Attack attack;
    private int currGauge;
    public bool attackable { get; private set; }
    public float gaugeRatio { get => (float)currGauge / skillControllerBase.maxGauge; }
    public Action<bool> Attackable;
    public Action Disattackable;

    private void Start()
    {
        currGauge = 0;
        SetAttackable(false);
    }

    public void ObtainGauge(int gauge)
    {
        if (attackable)
            return;
        currGauge += gauge;
        if (currGauge >= skillControllerBase.maxGauge)
        {
            currGauge = skillControllerBase.maxGauge;
            SetAttackable(true);
        }
    }

    private void SetAttackable(bool attackable)
    {
        this.attackable = attackable;
        Attackable?.Invoke(attackable);
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
        currGauge = 0;
        SetAttackable(false);
    }
}
