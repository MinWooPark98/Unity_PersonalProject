using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public string id;
    private int maxHp;
    private int currHp;
    // public HealthBarUi hpBar;

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
        if (currHp < 0)
        {
            OnDie();
        }
        // hp bar Ui ���� õõ�� �پ�鵵�� �ϴ� �Լ� ����
    }

    protected abstract void OnDie();
}
