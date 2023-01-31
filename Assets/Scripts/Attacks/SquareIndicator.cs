using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static SectorIndicator;
using static UnityEditor.PlayerSettings;

public class SquareIndicator : AttackIndicator
{
    private Transform attacker;
    private Vector3 dir;
    public float viewWidth;
    private float viewLength;
    Mesh viewMesh;
    public MeshFilter viewMeshFilter;
    public MeshRenderer viewMeshRenderer;
    public LayerMask obstacleMask;

    private Vector3[] vertices = new Vector3[4];
    private int[] triagnles = new int[6];

    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    public override void DrawRange(Vector3 dir, Attack attack)
    {
        attacker = attack.transform;
        this.dir = dir;
        viewLength = attack.attackBase.distance;
        viewMeshRenderer.enabled = true;
    }

    public override void Clear()
    {
        viewMesh.Clear();
        viewMeshRenderer.enabled = false;
    }

    private void Update()
    {
        if (!viewMeshRenderer.enabled)
            return;
        var len = viewLength;
        RaycastHit hit;
        var atkPosYInc = attacker.position + new Vector3(0f, 0.2f, 0f);
        if (Physics.Raycast(atkPosYInc, dir, out hit, len, obstacleMask))
            len = hit.distance;

        var halfWid = Quaternion.Euler(0, 90, 0) * dir * viewWidth * 0.5f;
        vertices[0] = atkPosYInc + halfWid;
        vertices[1] = vertices[0] + dir * len;
        vertices[2] = atkPosYInc - halfWid;
        vertices[3] = vertices[2] + dir * len;

        triagnles[0] = 0;
        triagnles[1] = 2;
        triagnles[2] = 1;
        triagnles[3] = 2;
        triagnles[4] = 3;
        triagnles[5] = 1;

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triagnles;
        viewMesh.RecalculateBounds();
        viewMesh.RecalculateNormals();
        viewMesh.RecalculateTangents();
    }
}
