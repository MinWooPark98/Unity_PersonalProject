using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : AttackFollowUp
{

    public Summon(AttackFollowUpBase thisBase)
    {
        followUpBase = thisBase;
    }

    public override void Execute(Vector3 pos)
    {
    }
}
