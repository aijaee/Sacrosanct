using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAI : MonoBehaviour
{
    public float moveSpeed = 5f;
    private List<Node> currentPath = new List<Node>();
    private bool isMoving = false;

    // Reference to UnitScript and tileMapScript
    private UnitScript unitScript;
    private tileMapScript map;

    void Start()
    {
        unitScript = GetComponent<UnitScript>();
        map = unitScript.map;

        if (unitScript == null)
        {
            Debug.LogError("UnitScript is missing on this GameObject.");
        }
        if (map == null)
        {
            Debug.LogError("TileMapScript (map) is not assigned in UnitScript.");
        }
    }

    void Update()
    {
        if (isMoving && currentPath.Count > 0)
        {
            MoveAlongPath();
        }
    }

    public void GeneratePathTo(Node target)
    {
        Node source = GetCurrentNode();
        if (source == null || target == null)
        {
            Debug.LogWarning("Invalid source or target node. Pathfinding aborted.");
            return;
        }

        if (source == target)
        {
            Debug.Log("Unit is already at the target.");
            return;
        }

        // Reset path
        currentPath.Clear();
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
        List<Node> unvisited = new List<Node>();

        dist[source] = 0;
        prev[source] = null;

        foreach (Node n in map.graph)
        {
            if (n != source)
            {
                dist[n] = Mathf.Infinity;
                prev[n] = null;
            }
            unvisited.Add(n);
        }

        while (unvisited.Count > 0)
        {
            Node u = null;
            foreach (Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if (u == target) break;

            unvisited.Remove(u);

            foreach (Node neighbor in u.neighbours)
            {
                float alt = dist[u] + u.DistanceTo(neighbor);
                if (alt < dist[neighbor])
                {
                    dist[neighbor] = alt;
                    prev[neighbor] = u;
                }
            }
        }

        Node current = target;
        while (current != null)
        {
            currentPath.Add(current);
            current = prev[current];
        }

        currentPath.Reverse();
        isMoving = true;
    }

    private void MoveAlongPath()
    {
        if (currentPath.Count == 0)
        {
            isMoving = false;
            return;
        }

        Node nextNode = currentPath[0];
        Vector3 targetPosition = map.tileCoordToWorldCoord(nextNode.x, nextNode.y);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentPath.RemoveAt(0);
        }
    }

    private Node GetCurrentNode()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.z);

        if (x < 0 || y < 0 || x >= map.mapSizeX || y >= map.mapSizeY)
        {
            return null;
        }

        return map.graph[x, y];
    }


    public void StopMoving()
    {
        isMoving = false;
        currentPath.Clear();
    }
}
