using UnityEngine;
using System.Collections.Generic;

public class AStarPathfinding : MonoBehaviour
{
    public Transform seeker;
    public Transform target;

    void Update()
    {
        if (seeker == null || target == null) return;
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = GridManager.instance.NodeFromWorldPoint(startPos);
        Node targetNode = GridManager.instance.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in GridManager.instance.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;

                int newCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newCost < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();
        GridManager.instance.path = path;
        seeker.GetComponent<EnemyMover>().SetPath(path);
    }

    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);
        return dstX + dstY;
    }
}
