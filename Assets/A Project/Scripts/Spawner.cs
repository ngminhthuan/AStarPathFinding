using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject obstacle2Prefab;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public AStarPathfinding AStarPathfinding;
    public int obstacleCount = 30;
    public List<GameObject> obstacles = new List<GameObject>();

    void Start()
    {
        SpawnRandom();
    }

    public void SpawnRandom()
    {
        GridManager grid = GridManager.instance;
        List<Node> walkables = new List<Node>();
        for (int x = 0; x < grid.gridWorldSize.x / (grid.nodeRadius * 2); x++)
        {
            for (int y = 0; y < grid.gridWorldSize.y / (grid.nodeRadius * 2); y++)
            {
                Node node = grid.NodeFromWorldPoint(
                    new Vector3(
                        -grid.gridWorldSize.x / 2 + x * grid.nodeRadius * 2 + grid.nodeRadius,
                        0,
                        -grid.gridWorldSize.y / 2 + y * grid.nodeRadius * 2 + grid.nodeRadius
                    )
                );

                if (node.walkable)
                    walkables.Add(node);
            }
        }
        Shuffle(walkables);

        int used = 0;

        if (obstacles.Count > 0) { 
            for (int i = 0; i < obstacles.Count; i++)
            {
                Destroy(obstacles[i]);
            }

            obstacles.Clear();  
        }

        for (int i = 0; i < obstacleCount && used < walkables.Count; i++)
        {
            int random = Random.Range(0, 100);
            GameObject obstacle = null;
            if(random > 50)
            {
                obstacle = Instantiate(obstaclePrefab, walkables[used].worldPosition, Quaternion.identity);
            }
            else
            {
                obstacle = Instantiate(obstacle2Prefab, walkables[used].worldPosition, Quaternion.identity);
            }
            obstacles.Add(obstacle);
            used++;
        }

        if(this.AStarPathfinding.target == null)
        {
            this.AStarPathfinding.target = Instantiate(playerPrefab, walkables[used].worldPosition, Quaternion.identity).transform;
        }
        else
        {
            this.AStarPathfinding.target.position = walkables[used].worldPosition;
        }

        if (this.AStarPathfinding.seeker == null)
        {
            this.AStarPathfinding.seeker = Instantiate(enemyPrefab, walkables[used].worldPosition, Quaternion.identity).transform;
        }
        else
        {
            this.AStarPathfinding.seeker.position = walkables[used].worldPosition;
        }

        GridManager.instance.CreateGrid();
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randIndex = Random.Range(i, list.Count);
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }
}
