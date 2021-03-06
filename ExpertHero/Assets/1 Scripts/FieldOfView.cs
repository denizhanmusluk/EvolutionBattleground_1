using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;
    [Range(0, 360)] public float drawAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();
    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    public enum States { humans, player}
    public States currentBehaviour;
    public GameObject weapon;
    void Start()
    {

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;


        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        gunActive();
    }
    //IEnumerator startUp()
    //{
    //    while (!Globals.startActive) { yield return null; }


    //}
    public void gunActive()
    {
        StartCoroutine("FindTargetsWithDelay", 0.1f);
    }
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return null;
            FindVisibleTargets();
        }
    }

    void LateUpdate()
    {
        //if (!FindObjectOfType<ArtilleryController>().pressed)
        //    transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        //if (FindObjectOfType<ArtilleryController>().pressed)
        //    transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

        DrawFieldOfView();
    }

    void FindVisibleTargets()
    {
        bool follow = false;
        //visibleTargets.Clear();
        //weapon.GetComponent<PlayerAttack>().currentBehaviour = PlayerAttack.States.none;
        //weapon.GetComponent<PlayerAttack>().targetZombie = null;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget;
                dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    if (!target.GetComponent<Zombie>().isSelected && visibleTargets.Count < 1)
                    {
                        //target.GetComponent<Zombie>().isSelected = true;

                        visibleTargets.Add(target);
                        weapon.GetComponent<PlayerAttack>().currentBehaviour = PlayerAttack.States.attack;
                        weapon.GetComponent<PlayerAttack>().targetZombie = visibleTargets[0].gameObject;
                        target.GetComponent<Zombie>().eye = this;
                        target.GetComponent<Zombie>().thisSelected();
                    }
                    if (visibleTargets.Count == 1)
                    {
                        if (visibleTargets[0] == null)
                        {
                            Debug.Log("field of view null");

                            visibleTargets.Clear();
                        }
                    }
                }
            }
        }


     
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(drawAngle * meshResolution);
        float stepAngleSize = drawAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - drawAngle / 2 + stepAngleSize * i;
            // Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);


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
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
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

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(true, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

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

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}