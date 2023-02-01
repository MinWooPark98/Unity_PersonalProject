using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Health : MonoBehaviour
{
    public string id;
    private int maxHp;
    private int currHp;
    public float hpRatio { get => (float)currHp / maxHp; }
    public DamageEffect effect;

    void Start()
    {
        var table = DataTableMgr.GetTable<HealthData>();
        maxHp = table.Get(id).maxHp;
        currHp = maxHp;
    }

    public void OnDamage(int dmg)
    {
        Debug.Log($"{dmg} {currHp}");
        currHp -= dmg;
        if (effect != null)
            effect.OnHit(dmg);
        if (currHp < 0)
        {
            OnDie();
        }
        // hp bar Ui ���� õõ�� �پ�鵵�� �ϴ� �Լ� ����
    }

    protected abstract void OnDie();
}
