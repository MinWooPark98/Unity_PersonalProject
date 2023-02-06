using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageEffect : MonoBehaviourPun
{
    public ShowDamage prefab;

    public float height = 2f;
    public float duration = 1f;
    public float refloatY = 1f;
    public Color color;
    public float characterSize = 6f;

    [PunRPC]
    public void OnHitOnServer(int dmg)
    {
        photonView.RPC("OnHit", RpcTarget.All, dmg);
    }

    [PunRPC]
    public void OnHit(int dmg)
    {
        var effect = GameManager.instance.showDamageLauncher.Get();
        effect.transform.position = transform.position + new Vector3(Random.Range(0, 0.5f), 1f, Random.Range(0, 0.5f)) * height;
        effect.Set(dmg.ToString(), color, characterSize, duration, refloatY);
    }
}
