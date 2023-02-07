using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Health : MonoBehaviourPun
{
    public string id;
    protected int maxHp;
    protected int currHp;
    public float hpRatio { get => (float)currHp / maxHp; }
    public DamageEffect effect;
    public HpBarUi hpBarUi;

    void Start()
    {
        var table = DataTableMgr.GetTable<HealthData>();
        maxHp = table.Get(id).maxHp;
        SetCurrHp(maxHp);
    }

    protected void SetCurrHp(int hp)
    {
        currHp = hp;
        hpBarUi.SetCurrHpText(currHp);
    }

    public void OnDamageOnServer(int dmg) => photonView.RPC("OnDamage", RpcTarget.MasterClient, dmg);

    [PunRPC]
    public void ApplyHealth(int currHp) => SetCurrHp(currHp);

    [PunRPC]
    public void OnDamage(int dmg)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetCurrHp(currHp - dmg);
            if (effect != null)
                effect.OnHitOnServer(dmg);
            photonView.RPC("ApplyHealth", RpcTarget.Others, currHp);
            photonView.RPC("OnDamage", RpcTarget.Others, dmg);
        }

        if (currHp <= 0)
        {
            SetCurrHp(0);
            OnDie();
        }
    }

    protected abstract void OnDie();
}
