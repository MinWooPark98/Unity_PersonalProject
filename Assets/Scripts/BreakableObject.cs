using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviourPun
{
    public GameObject before;
    public GameObject After;

    private void Start()
    {
        before.SetActive(true);
        After.SetActive(false);
    }

    public void BreakOnServer() => photonView.RPC("Break", RpcTarget.All);

    [PunRPC]
    public void Break()
    {
        before.SetActive(false);
        After.SetActive(true);
        GetComponent<Collider>().enabled = false;
    }
}
