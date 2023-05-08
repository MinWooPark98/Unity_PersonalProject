using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerHide : MonoBehaviourPun
{
    public bool isInBush { get; private set; } = false;
    public bool isHiding = false;
    public Renderer[] renderers;
    public Canvas canvas;

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (isInBush)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                var playerPV = player.GetComponent<PhotonView>();
                if (playerPV.IsMine)
                    continue;
                var playerHide = player.GetComponent<PlayerHide>();
                if (playerHide.isInBush && Vector3.Distance(transform.position, player.transform.position) < 2.5f)
                    RevealOnOther(playerPV.Owner);
                else
                    HideOnOther(playerPV.Owner);
            }
        }
        else
            RevealOnOthers();
    }

    public void InBushOnServer() => photonView.RPC("InBush", RpcTarget.All);

    [PunRPC]
    public void InBush()
    {
        isInBush = true;
        //if (photonView.IsMine)
        //    return;
    }
    public void HideOnOthers() => photonView.RPC("Hide", RpcTarget.Others);
    public void HideOnOther(Photon.Realtime.Player player) => photonView.RPC("Hide", player);

    [PunRPC]
    public void Hide()
    {
        if (photonView.IsMine)
            return;

        if (!isHiding)
        {
            isHiding = true;
            foreach (var renderer in renderers)
            {
                renderer.enabled = false;
            }
            canvas.enabled = false;
        }
    }

    public void OutOfBushOnServer() => photonView.RPC("OutOfBush", RpcTarget.All);
    [PunRPC]
    public void OutOfBush()
    {
        isInBush = false;
        //if (photonView.IsMine)
        //    return;
    }
    public void RevealOnOthers() => photonView.RPC("Reveal", RpcTarget.Others);
    public void RevealOnOther(Photon.Realtime.Player player) => photonView.RPC("Reveal", player);

    [PunRPC]
    public void Reveal()
    {
        if (photonView.IsMine)
            return;

        if (isHiding)
        {
            isHiding = false;
            foreach (var renderer in renderers)
            {
                renderer.enabled = true;
            }
            canvas.enabled = true;
        }
    }
}
