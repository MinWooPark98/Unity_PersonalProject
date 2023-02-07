using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class Projectile : MonoBehaviourPun, IPunObservable
{
    private GameObject attacker;
    private Vector3 startPos;
    private Vector3 direction;
    private int obtainGauge;
    private float arrivalTime;
    private float timer;
    private float distance;
    private int damage;
    private int level;
    private bool isPenetrable = true;
    private bool isBreakable = false;
    private bool isParabolic = false;
    private float height = 0f;
    private AttackFollowUp followAttack;
    private Rigidbody rb;
    private LinkedList<GameObject> hitObjects = new LinkedList<GameObject>();
    private IObjectPool<Projectile> pool;

    private Vector3 clonePos;
    private Quaternion cloneRot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        timer = 0f;
        hitObjects.Clear();
    }

    private void FollowAttack()
    {
        followAttack?.Execute(attacker, transform.position, level);
    }

    [PunRPC]
    public void SetActive(bool active) => gameObject.SetActive(active);
    [PunRPC]
    public void SetActiveOnServer(bool active)
    {
        photonView.RPC("SetActive", RpcTarget.All, active);
    }

    public void SetPool(IObjectPool<Projectile> pool) => this.pool = pool;

    public void SetBase(int obtainGauge, float arrivalTime, float distance, AttackFollowUp followAttack, bool isPenetrable = true, bool isBreakable = false, bool isParabolic = false, float height = 0f)
    {
        this.obtainGauge = obtainGauge;
        this.arrivalTime = arrivalTime;
        this.distance = distance;
        this.followAttack = followAttack;
        this.height = height;
        this.isPenetrable = isPenetrable;
        this.isBreakable = isBreakable;
        this.isParabolic = isParabolic;
    }

    public void Set(GameObject attacker, Vector3 startPos, Vector3 direction, int damage, int level)
    {
        if (!photonView.IsMine)
            return;
        this.attacker = attacker;
        this.startPos = startPos;
        this.direction = direction;
        this.damage = damage;
        this.level = level;
        transform.position = startPos;
        SetInitialTransformOthers(startPos, direction);
    }

    [PunRPC]
    public void SetInitialTransformOthers(Vector3 pos, Vector3 dir) => photonView.RPC("SetInitialTransform", RpcTarget.All, pos, dir);

    [PunRPC]
    public void SetInitialTransform(Vector3 pos, Vector3 dir)
    {
        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(dir);
        clonePos = transform.position;
        cloneRot = transform.rotation;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            timer += Time.deltaTime;
            if (timer >= arrivalTime)
            {
                FollowAttack();
                pool.Release(this);
            }

            if (isParabolic)
                transform.position = BezierCurve();
            else
            {
                Vector3 dir = direction;
                rb.velocity = dir * distance / arrivalTime;
                transform.forward = dir;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, clonePos, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Slerp(transform.rotation, cloneRot, Time.deltaTime * 10f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
            return;
        if (attacker == other.gameObject)
            return;

        if (hitObjects.Find(other.gameObject) != null)
            return;

        if (isBreakable)
        {
            var breakable = other.GetComponent<BreakableObject>();
            if (breakable != null)
                breakable.BreakOnServer();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Solid"))
        {
            FollowAttack();
            pool.Release(this);
            return;
        }

        hitObjects.AddLast(other.gameObject);

        var subject = other.GetComponent<Health>();
        if (subject != null)
        {
            attacker.GetComponent<SkillAttackController>().ObtainGauge(obtainGauge);
            subject.OnDamageOnServer(damage);

            if (!isPenetrable)
            {
                FollowAttack();
                pool.Release(this);
            }
        }
    }

    private Vector3 BezierCurve()
    {
        var endPos = startPos + direction * distance;
        var halfPos = (startPos + endPos) * 0.5f + new Vector3(0f, height ,0f);
        var ratio = timer / arrivalTime;
        var p1 = Vector3.Lerp(startPos, halfPos, ratio);
        var p2 = Vector3.Lerp(halfPos, endPos, ratio);
        return Vector3.Lerp(p1, p2, ratio);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (!gameObject.activeSelf)
            return;

        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            clonePos = (Vector3)stream.ReceiveNext();
            cloneRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
