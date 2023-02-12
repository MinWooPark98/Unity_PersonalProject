using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfoPanel : MonoBehaviour
{
    public AttackInfoUi basicAttackUi;
    public AttackInfoUi skillAttackUi;

    public void Set(string id)
    {
        basicAttackUi.Set(id, AttackBase.AttackType.Basic);
        skillAttackUi.Set(id, AttackBase.AttackType.Skill);
    }
}
