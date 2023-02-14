using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnBush : MonoBehaviour
{
    public MeshRenderer[] renderers;
    public Material usualMaterial;
    public Material transparentMaterial;
    private bool isHideOn;

    private void Start()
    {
        HideOff();
    }

    public void OnTriggerStay(Collider other)
    {
        var playerHide = other.GetComponent<PlayerHide>();
        if (playerHide != null)
        {
            var playerPV = other.GetComponent<PhotonView>();
            if (playerPV != null && playerPV.IsMine)
            {
                var bushes = GameManager.instance.bushes;
                foreach (var bush in bushes)
                {
                    if (Vector3.Distance(bush.transform.position, transform.position) < 2.1f)
                    {
                        playerHide.InBushOnServer();
                        bush.HideOn();
                    }
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var playerHide = other.GetComponent<PlayerHide>();
        if (playerHide != null)
        {
            var playerPV = other.GetComponent<PhotonView>();
            if (playerPV != null && playerPV.IsMine)
            {
                var bushes = GameManager.instance.bushes;
                foreach (var bush in bushes)
                {
                    if (Vector3.Distance(bush.transform.position, transform.position) < 2.1f)
                    {
                        playerHide.OutOfBushOnServer();
                        bush.HideOff();
                    }
                }
            }
        }
    }

    public void HideOn()
    {
        if (!isHideOn)
        {
            isHideOn = true;
            foreach (var renderer in renderers)
            {
                renderer.material = transparentMaterial;
            }
        }
    }

    public void HideOff()
    {
        isHideOn = false;
        foreach (var renderer in renderers)
        {
            renderer.material = usualMaterial;
        }
    }
}
