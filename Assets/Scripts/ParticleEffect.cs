using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleEffect : MonoBehaviour
{
    private IObjectPool<ParticleEffect> pool;
    private ParticleSystem particle;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        GetComponent<ParticleSystem>().Play();
    }

    private void OnDisable()
    {
        GetComponent<ParticleSystem>().Stop();
    }

    public void SetPool(IObjectPool<ParticleEffect> pool) => this.pool = pool;

    private void Update()
    {
        if (particle.isStopped)
            pool.Release(this);
    }
}
