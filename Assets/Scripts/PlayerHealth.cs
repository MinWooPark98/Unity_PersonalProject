using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [PunRPC]
    protected override void OnDie()
    {
        
    }
}
