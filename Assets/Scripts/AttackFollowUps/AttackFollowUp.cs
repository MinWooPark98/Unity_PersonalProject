using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackFollowUp
{
    protected AttackFollowUpBase followUpBase;

    public abstract void Execute(Vector3 pos);
}
