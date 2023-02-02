using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThrowIndicator : AttackIndicator
{
    private Transform attacker;
    private Vector3 dir;
    public float radius;
    private float distance;
    private float height;
    private SpriteRenderer circle;
    private LineRenderer line;
    public int linePositionCount;
    private bool isDrawing = false;

    private void Start()
    {
        circle = PlaySceneManager.instance.throwArea;
        line = PlaySceneManager.instance.throwLine;
        line.positionCount = linePositionCount;
    }

    public override void DrawRange(Vector3 dir, Attack attack)
    {
        attacker = attack.transform;
        this.dir = dir;
        distance = attack.attackBase.distance;
        height = ((ThrowAttackBase)attack.attackBase).throwHeight;
        circle.transform.localScale = new Vector3(radius, radius, 0f);
        circle.enabled = true;
        line.enabled = true;
        isDrawing = true;
    }

    public override void Clear()
    {
        circle.enabled = false;
        line.enabled = false;
        isDrawing = false;
    }

    private void Update()
    {
        if (!isDrawing)
            return;
        circle.transform.position = attacker.position + dir * distance;
        var startPos = attacker.position + dir.normalized * 0.4f;
        var endPos = circle.transform.position;
        for (int i = 0; i < linePositionCount; ++i)
        {
            line.SetPosition(i, BezierCurve(startPos, endPos, (float)i / linePositionCount));
        }
    }


    private Vector3 BezierCurve(Vector3 startPos, Vector3 endPos, float ratio)
    {
        var halfPos = (startPos + endPos) * 0.5f + new Vector3(0f, height, 0f);
        var p1 = Vector3.Lerp(startPos, halfPos, ratio);
        var p2 = Vector3.Lerp(halfPos, endPos, ratio);
        return Vector3.Lerp(p1, p2, ratio);
    }
}
