using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    private GameObject attacker;
    private Vector3 startPos;
    private Vector3 direction;
    private float arrivalTime;
    private float timer;
    private float distance;
    private int damage;
    private bool isPenetrable = true;
    private bool isParabolic = false;
    private float height = 0f;
    private AttackFollowUp followAttack;
    private Rigidbody rb;
    private LinkedList<GameObject> hitObjects = new LinkedList<GameObject>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        timer = 0f;
        hitObjects.Clear();
    }

    private void OnDisable()
    {
        followAttack?.Execute(transform.position);
    }

    public void Set(GameObject attacker, Vector3 startPos, Vector3 direction, float arrivalTime, float distance, int damage, AttackFollowUp followAttack, bool isPenetrable = true, bool isParabolic = false, float height = 0f)
    {
        this.attacker = attacker;
        this.startPos = startPos;
        this.direction = direction;
        this.arrivalTime = arrivalTime;
        this.distance = distance;
        this.damage = damage;
        this.followAttack = followAttack;
        this.isPenetrable = isPenetrable;
        this.isParabolic = isParabolic;
        this.height = height;
        transform.position = startPos;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= arrivalTime)
            Destroy(gameObject);

        if (isParabolic)
            rb.MovePosition(BezierCurve());
        else
        {
            Vector3 dir = direction;
            rb.velocity = dir * distance / arrivalTime;
            transform.LookAt(dir);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (attacker == other.gameObject)
            return;

        if (!isPenetrable)
            Destroy(gameObject);
        else if (hitObjects.Find(other.gameObject) != null)
            return;
        hitObjects.AddLast(other.gameObject);

        var subject = other.GetComponent<Health>();
        if (subject == null)
        {
            Destroy(gameObject);
            return;
        }
        else
            subject.OnDamage(damage);
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
}
