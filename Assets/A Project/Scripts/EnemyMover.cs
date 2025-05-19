using UnityEngine;
using System.Collections.Generic;

public class EnemyMover : MonoBehaviour
{
    List<Node> path;
    int index = 0;
    public float speed = 3f;

    public void SetPath(List<Node> newPath)
    {
        path = newPath;
        index = 0;
    }

    void Update()
    {
        if (path == null || index >= path.Count) return;

        Vector3 targetPos = path[index].worldPosition;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            index++;
        }
    }
}
