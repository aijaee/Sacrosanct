using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool blocked;
    public bool IsBlocked => blocked; // Checks if the tile is blocked

    public Vector2Int cords;
    GridManager gridManager;

    void Start()
    {
        SetCords();

        if (blocked)
        {
            gridManager.BlockNode(cords); // Block the node if this tile is blocked
        }
    }

    private void SetCords()
    {
        gridManager = FindObjectOfType<GridManager>();
        int x = (int)transform.position.x;
        int z = (int)transform.position.z;

        cords = new Vector2Int(x / gridManager.UnityGridSize, z / gridManager.UnityGridSize);
    }
}
