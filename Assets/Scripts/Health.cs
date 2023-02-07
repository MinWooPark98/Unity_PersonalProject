using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Health : MonoBehaviourPun
{
    public string id;
    protected int maxHp;
    private int currHp;
    public float hpRatio { get => (float)currHp / maxHp; }
    public DamageEffect effect;

    void Start()
    {
        var table = DataTableMgr.GetTable<HealthData>();
        maxHp = table.Get(id).maxHp;
        currHp = maxHp;
    }

    public void OnDamageOnServer(int dmg) => photonView.RPC("OnDamage", RpcTarget.MasterClient, dmg);

    [PunRPC]
    public void ApplyHealth(int currHp) => this.currHp = currHp;

    [PunRPC]
    public void OnDamage(int dmg)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            currHp -= dmg;
            if (effect != null)
                effect.OnHitOnServer(dmg);
            photonView.RPC("ApplyHealth", RpcTarget.Others, currHp);
            photonView.RPC("OnDamage", RpcTarget.Others, dmg);
        }

        if (currHp <= 0)
        {
            currHp = 0;
            OnDie();
        }
    }

    protected abstract void OnDie();
}
