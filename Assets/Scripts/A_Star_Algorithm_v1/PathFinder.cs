/// ksPark
///
/// A* Algorithm을 이용한 길 찾기 시스템

using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

[RequireComponent(typeof(LineRenderer))]

class Spot
{
    public Vector3 position;
    public float distanceStart;
    public float distanceEnd;

    public Spot(Vector3 position, float distanceStart, float distanceEnd)
    {
        this.position = position;
        this.distanceStart = distanceStart;
        this.distanceEnd = distanceEnd;
    }
}

public class PathFinder : MonoBehaviour
{
    [Range(0.001f, 10.0f)]
    public float spotDistance = 1.0f;

    public Vector3 start, end;

    private LineRenderer lineRenderer;
    private List<Vector3> path;
    private SimplePriorityQueue<Spot> pq;
    Dictionary<Vector3, Vector3> visit;

    public float speed = 5.0f;
    public bool isMove = false;

    private Vector3[] moveList =
    {
        Vector3.left + Vector3.forward, Vector3.left + Vector3.back,
        Vector3.right + Vector3.forward, Vector3.right + Vector3.back,
        Vector3.left, Vector3.right, Vector3.forward, Vector3.back
    };

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        path    = new List<Vector3>();
        pq      = new SimplePriorityQueue<Spot>();
        visit   = new Dictionary<Vector3, Vector3>();
    }

    private void Update()
    {
        if (isMove)
            MovePath(path);
    }

    public void GetPath(Vector3 targetPos)
    {
        if (isMove && path.Count > 0)
            return;

        start = transform.position;
        end = targetPos;
        end.y = start.y;

        path.Clear();
        pq.Clear();
        visit.Clear();

        pq.Enqueue(
            new Spot(start, 0, Vector3.Distance(start, end)),
            Vector3.Distance(start, end)
        );
        visit.Add(start, start);

        Collider[] collList = new Collider[0];
        while (pq.Count != 0)
        {
            Spot nowSpot = pq.Dequeue();

            if (nowSpot.distanceEnd < spotDistance)
            {
                if (!visit.ContainsKey(end))
                    visit.Add(end, nowSpot.position);
                break;
            }

            for (int i=0; i<moveList.Length; i++)
            {
                Vector3 newPosition = nowSpot.position + moveList[i] * spotDistance;
                if (visit.ContainsKey(newPosition)) continue;
                visit.Add(newPosition, nowSpot.position);

                if (Physics.Raycast(nowSpot.position, moveList[i], spotDistance))
                    continue;

                Debug.DrawRay(nowSpot.position, moveList[i], Color.red);

                Spot newSpot = new Spot(newPosition, nowSpot.distanceStart + spotDistance * (i<4 ? 1.4f : 1f), Vector3.Distance(newPosition, end));
                pq.Enqueue(newSpot, newSpot.distanceStart + newSpot.distanceEnd);
            }
        }

        while (start != end)
        {
            path.Add(end);
            end = visit[end];
        }

        path.Add(start);

        lineRenderer.positionCount = path.Count;

        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(i, path[i]);
        }
    }
    
    void MovePath(List<Vector3> path)
    {
        if (path.Count == 0)
        {
            isMove = false;
            return;
        }

        transform.position = path[path.Count - 1];

        if (Vector3.Distance(transform.position, path[path.Count-1]) < spotDistance * .01f)
            path.Remove(path[path.Count-1]);
    }
}
