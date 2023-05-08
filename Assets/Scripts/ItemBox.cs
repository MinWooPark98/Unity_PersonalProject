using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : Health
{
    public GameObject item;

    [PunRPC]
    protected override void OnDie()
    {
        gameObject.SetActive(false);
        if (item != null)
            item.SetActive(true);
    }
}
