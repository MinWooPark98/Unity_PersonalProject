using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfoManager : MonoBehaviour
{
    private static AttackInfoManager m_instance;
    public static AttackInfoManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<AttackInfoManager>();
            return m_instance;
        }
    }

    public AttackBase[] attackBases = null;
    private Dictionary<Characters, Dictionary<AttackBase.AttackType, AttackBase>> attackInfoDict = new Dictionary<Characters, Dictionary<AttackBase.AttackType, AttackBase>>();

    private void Awake()
    {
        foreach (var attackBase in attackBases)
        {
            if (attackBase == null)
                continue;
            if (!attackInfoDict.ContainsKey(attackBase.character))
            {
                var innerDict = new Dictionary<AttackBase.AttackType, AttackBase>();
                attackInfoDict.Add(attackBase.character, innerDict);
            }
            attackInfoDict[attackBase.character][attackBase.attackType] = attackBase;
        }
    }

    public AttackBase GetAttackBase(Characters character, AttackBase.AttackType attackType) => attackInfoDict[character][attackType];
}
