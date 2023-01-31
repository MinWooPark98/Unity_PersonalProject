using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorIndicator : AttackIndicator
{
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct Edge
    {
        public Vector3 PointA, PointB;
        public Edge(Vector3 _PointA, Vector3 _PointB)
        {
            PointA = _PointA;
            PointB = _PointB;
        }
    }

    // �þ� ������ �������� �þ� ����
    private float viewRadius;
    private float viewAngle;

    // ����ũ 2��
    public LayerMask targetMask, obstacleMask;
    public float meshResolution;

    // Target mask�� ray hit�� transform�� �����ϴ� ����Ʈ
    public List<Transform> visibleTargets = new List<Transform>();

    Mesh viewMesh;
    public MeshFilter viewMeshFilter;
    public MeshRenderer viewMeshRenderer;

    public int edgeResolveIterations;
    public float edgeDstThreshold;

    private Transform attacker;
    private Vector3 lookDir;

    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    // y�� ���Ϸ� ���� 3���� ���� ���ͷ� ��ȯ�Ѵ�.
    // ������ ������ ��¦ �ٸ��� ����. ����� ����.
    public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegrees += Quaternion.LookRotation(lookDir).eulerAngles.y;//transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
    }

    public override void DrawRange(Vector3 dir, Attack attack)
    {
        attacker = attack.transform;
        lookDir = dir;
        viewRadius = attack.attackBase.distance;
        viewAngle = ((RangeAttackBase)attack.attackBase).angle;
        viewMeshRenderer.enabled = true;
    }

    private void Update()
    {
        if (!viewMeshRenderer.enabled)
            return;
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo prevViewCast = new ViewCastInfo();

        var atkPosYInc = attacker.position + new Vector3(0f, 0.2f, 0f);
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = Quaternion.LookRotation(lookDir).eulerAngles.y/*transform.eulerAngles.y*/ - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(atkPosYInc, angle);

            // i�� 0�̸� prevViewCast�� �ƹ� ���� ���� ���� ������ �� �� �����Ƿ� �ǳʶڴ�.
            if (i != 0)
            {
                bool edgeDstThresholdExceed = Mathf.Abs(prevViewCast.dst - newViewCast.dst) > edgeDstThreshold;

                // �� �� �� raycast�� ��ֹ��� ������ �ʾҰų� �� raycast�� ���� �ٸ� ��ֹ��� hit �� ���̶��(edgeDstThresholdExceed ���η� ���)
                if (prevViewCast.hit != newViewCast.hit || (prevViewCast.hit && newViewCast.hit && edgeDstThresholdExceed))
                {
                    Edge e = FindEdge(atkPosYInc, prevViewCast, newViewCast);

                    // zero�� �ƴ� ������ �߰���
                    if (e.PointA != Vector3.zero)
                    {
                        viewPoints.Add(e.PointA);
                    }

                    if (e.PointB != Vector3.zero)
                    {
                        viewPoints.Add(e.PointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            prevViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        vertices[0] = atkPosYInc;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = viewPoints[i];
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateBounds();
        viewMesh.RecalculateNormals();
        viewMesh.RecalculateTangents();
    }

    public override void Clear()
    {
        viewMesh.Clear();
        viewMeshRenderer.enabled = false;
    }

    ViewCastInfo ViewCast(Vector3 position, float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;
        if (Physics.Raycast(position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    Edge FindEdge(Vector3 pos, ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = minAngle + (maxAngle - minAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(pos, angle);
            bool edgeDstThresholdExceed = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceed)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new Edge(minPoint, maxPoint);
    }
}