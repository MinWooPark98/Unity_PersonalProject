using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackInfoUi : MonoBehaviour
{
    public TextMeshProUGUI attackName;
    public TextMeshProUGUI attackDesc;
    public TextMeshProUGUI attackDamage;
    public TextMeshProUGUI attackShape;
    public TextMeshProUGUI attackRange;

    public void Set(string id, AttackBase.AttackType type)
    {
        var data = DataTableMgr.GetTable<CharacterData>().Get(id);
        var info = AttackInfoManager.instance.GetAttackBase(data.character, type);
        attackName.text = data.name;
        attackDesc.text = data.desc;
        if (info.damage <= 0)
            attackDamage.transform.parent.gameObject.SetActive(false);
        else
            attackDamage.transform.parent.gameObject.SetActive(true);
        switch (info.attackShape)
        {
            case AttackBase.AttackShape.Front:
                attackDamage.text = $"{((FrontAttackBase)info).maxCount}x{info.damage}";
                attackShape.text = string.Format("������");
                break;
            case AttackBase.AttackShape.Range:
                attackDamage.text = $"{((RangeAttackBase)info).maxCount}x{info.damage}";
                attackShape.text = string.Format("������");
                break;
            case AttackBase.AttackShape.Throw:
                attackDamage.text = (info.damage).ToString();
                attackShape.text = string.Format("��ô��");
                break;
        }
        if (info.distance < 3)
            attackRange.text = string.Format("�ٰŸ�");
        else if (info.distance < 7)
            attackRange.text = string.Format("�߰Ÿ�");
        else if (info.distance < 11)
            attackRange.text = string.Format("��Ÿ�");
        else
            attackRange.text = string.Format("����Ÿ�");
    }
}
