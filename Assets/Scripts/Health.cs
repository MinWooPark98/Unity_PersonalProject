using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Health : MonoBehaviour
{
    public string id;
    private int maxHp;
    private int currHp;
    
    public Slider hpBarReal;
    public Slider hpBarEffect;

    void Start()
    {
        var table = DataTableMgr.GetTable<HealthData>();
        maxHp = table.Get(id).maxHp;
        currHp = maxHp;
    }

    private void Update()
    {
        var hpRatio = (float)currHp / maxHp;
        hpBarReal.value = hpRatio;
        hpBarEffect.value = Mathf.Lerp(hpBarEffect.value, hpRatio, Time.deltaTime);
    }

    public void OnDamage(int dmg)
    {
        Debug.Log($"{dmg} {currHp}");
        currHp -= dmg;
        if (currHp < 0)
        {
            OnDie();
        }
        // hp bar Ui 에서 천천히 줄어들도록 하는 함수 실행
    }

    protected abstract void OnDie();
}
