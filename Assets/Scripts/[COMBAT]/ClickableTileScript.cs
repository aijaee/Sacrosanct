using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTileScript : MonoBehaviour    
{
    //The x and y co-ordinate of the tile
    public int tileX;
    public int tileY;
    //The unit on the tile
    public GameObject unitOnTile;
    public tileMapScript map;


    public void SetUnitOnTile(GameObject unit)
    {
        unitOnTile = unit;
        if (unit != null)
        {
            Debug.Log($"Unit {unit.name} moved to tile ({tileX}, {tileY})");
        }
        else
        {
            Debug.Log($"Tile ({tileX}, {tileY}) is now empty");
        }
    }


    /*
     * This was used in Quill18Create'sTutorial, I no longer use this portion
    private void OnMouseDown()
    {
       
        Debug.Log("tile has been clicked");
        if (map.selectedUnit != null)
        {
            map.generatePathTo(tileX, tileY);
        }
        
    }*/
}
