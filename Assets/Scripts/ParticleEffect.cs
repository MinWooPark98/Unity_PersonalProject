using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleEffect : MonoBehaviourPun
{
    private IObjectPool<ParticleEffect> pool;
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        particle.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GetComponent<ParticleSystem>().Play();
    }

    private void OnDisable()
    {
        GetComponent<ParticleSystem>().Stop();
    }

    [PunRPC]
    public void SetActive(bool active) => gameObject.SetActive(active);
    [PunRPC]
    public void SetActiveOnServer(bool active)
    {
        photonView.RPC("SetActive", RpcTarget.All, active);
    }

    public void SetPool(IObjectPool<ParticleEffect> pool) => this.pool = pool;

    [PunRPC]
    public void SetAll(Vector3 pos, Vector3 scale)
    {
        photonView.RPC("Set", RpcTarget.All, pos, scale);
    }

    [PunRPC]
    public void Set(Vector3 pos, Vector3 scale)
    {
        transform.position = pos;
        transform.localScale = scale;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;
        if (particle.isStopped)
            pool.Release(this);
    }
}
