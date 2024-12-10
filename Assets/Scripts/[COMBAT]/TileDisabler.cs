using UnityEngine;

public class TileDisabler : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab; // Assign your tile prefab here
    [SerializeField] private Color disabledColor = Color.red; // Color to indicate the tile is disabled

    private Renderer tileRenderer;
    private bool isDisabled = false;

    void Start()
    {
    }

    // Method to disable the tile
    public void DisableTile()
    {
        isDisabled = true;
        UpdateTileAppearance();
    }

    // Method to enable the tile
    public void EnableTile()
    {
        isDisabled = false;
        UpdateTileAppearance();
    }

    // Check if the tile is disabled
    public bool IsTileDisabled()
    {
        return isDisabled;
    }

    // Update the tile's appearance based on its state
    private void UpdateTileAppearance()
    {
        if (tileRenderer != null)
        {
            tileRenderer.material.color = isDisabled ? disabledColor : Color.white;
        }
    }

    // Example interaction: Prevent player movement onto this tile
    public bool CanMoveToTile()
    {
        return !isDisabled; // Allow movement only if the tile is not disabled
    }

    // Optional: Visualize the tile's state in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = isDisabled ? disabledColor : Color.green;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.9f);
    }
}
