using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public void MaxHpUp()
    {
        var growthHp = DataTableMgr.GetTable<HealthData>().Get(id).growthHp;
        maxHp += growthHp;
        SetCurrHp(currHp + growthHp);
    }

    [PunRPC]
    protected override void OnDie()
    {
        transform.parent.gameObject.SetActive(false);
        if (photonView.IsMine)
        {
            GameManager.instance.GameOver();
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                GameManager.instance.SendWinner();
        }
    }
}
