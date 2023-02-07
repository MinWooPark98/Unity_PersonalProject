using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class PoisonCloud : MonoBehaviourPun
{
    public int initCount = 100;
    public ParticleSystem normal;
    public ParticleSystem reverse;
    private ParticleSystem.ShapeModule normalShape;
    private ParticleSystem.ShapeModule reverseShape;
    private ParticleSystem.EmissionModule normalEmission;
    private ParticleSystem.EmissionModule reverseEmission;
    private float lastProgress;

    public float damageCycle = 2f;
    private float damageTimer;
    public int damage = 20;

    private void Start()
    {
        lastProgress = 0f;
        normalShape = normal.shape;
        reverseShape = reverse.shape;
        normalEmission = normal.emission;
        reverseEmission = reverse.emission;
        damageTimer = 0f;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageCycle)
            {
                var players = GameObject.FindGameObjectsWithTag("Player");
                var mapSize = GameManager.instance.mapSize;
                foreach (var player in players)
                {
                    var real = player.GetComponentInChildren<PlayerController>();
                    if (real != null)
                    {
                        var playerPos = real.transform.position;
                        if (playerPos.x < -mapSize.x * 0.5f * (1 - lastProgress) ||
                            playerPos.x > mapSize.x * 0.5f * (1 - lastProgress) ||
                            playerPos.z < -mapSize.y * 0.5f * (1 - lastProgress) ||
                            playerPos.z > mapSize.y * 0.5f * (1 - lastProgress))
                            real.GetComponent<Health>()?.OnDamage(damage);
                    }
                }
                damageTimer = 0f;
            }
        }

        float gameProgress = GameManager.instance.GameProgress;
        if (gameProgress > lastProgress + 0.05f)
        {
            Vector3 boxThickness = new Vector3(gameProgress * 0.5f, gameProgress * 0.5f, 0f);
            normalShape.boxThickness = boxThickness;
            reverseShape.boxThickness = boxThickness;
            normalEmission.rateOverTime = initCount + initCount * gameProgress * 5;
            reverseEmission.rateOverTime = initCount + initCount * gameProgress * 5;
            lastProgress = gameProgress;
        }
    }
}
