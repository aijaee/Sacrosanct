using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;
    Transform selectedUnit;
    bool unitSelected = false;
    bool isMoving = false;
    List<Node> path = new List<Node>();

    GridManager gridManager;
    Pathfinding pathFinder;
    Vector2Int currentTileCords;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<Pathfinding>();

        if (gridManager == null || pathFinder == null)
        {
            Debug.LogError("GridManager or Pathfinding component not found in the scene.");
            return;
        }

        if (selectedUnit != null)
        {
            currentTileCords = gridManager.GetCoordinatesFromPosition(selectedUnit.position);
            gridManager.BlockNode(currentTileCords);
        }
        else
        {
            Debug.LogWarning("Selected unit is not assigned.");
        }
    }

    void Update()
    {
        if (isMoving) return; // Prevent click if unit is moving

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit)
            {
                if (hit.transform.CompareTag("Tile"))
                {
                    Tile clickedTile = hit.transform.GetComponent<Tile>();

                    // Check if the clicked tile is blocked (using IsBlocked instead of walkable)
                    if (clickedTile.IsBlocked) return;

                    if (unitSelected)
                    {
                        Vector2Int targetCords = clickedTile.cords;
                        Vector2Int startCords = currentTileCords;

                        // Set the new destination with the correct coordinates
                        pathFinder.SetNewDestination(startCords, targetCords);

                        // Recalculate the path from the current position
                        RecalculatePath(true);
                    }
                }
                else if (hit.transform.CompareTag("Unit"))
                {
                    selectedUnit = hit.transform;
                    unitSelected = true;
                }
            }
        }
    }

    void RecalculatePath(bool resetPath)
    {
        // Get the starting coordinates and update the path
        Vector2Int coordinates = resetPath ? pathFinder.StartCords : gridManager.GetCoordinatesFromPosition(transform.position);

        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);

        // If no path is found, return and display a warning
        if (path.Count == 0)
        {
            Debug.LogWarning("No valid path found to the target.");
            return;
        }

        // Start following the path once calculated
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        isMoving = true;

        // Iterate through the path and move the unit to each node
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 startPosition = selectedUnit.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].cords);
            endPosition.y = startPosition.y;

            float travelPercent = 0f;
            selectedUnit.LookAt(endPosition);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * movementSpeed;
                selectedUnit.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }

            currentTileCords = path[i].cords;
        }

        isMoving = false; // Enable clicks again once movement is complete
    }
}
