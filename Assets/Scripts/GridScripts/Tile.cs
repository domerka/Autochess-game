using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Single grid element.
/// </summary>
public class Tile : MonoBehaviour
{
    /// <summary>
    /// Sum of G and H.
    /// </summary>
    public int F => g + h;

    /// <summary>
    /// Cost from start tile to this tile.
    /// </summary>
    public int g;

    /// <summary>
    /// Estimated cost from this tile to destination tile.
    /// </summary>
    public int h;

    /// <summary>
    /// Tile's coordinates.
    /// </summary>
    public Vector3Int position;

    /// <summary>
    /// References to all adjacent tiles.
    /// </summary>
    public List<Tile> adjacentTiles = new List<Tile>();

    /// <summary>
    /// If true - Tile is an obstacle impossible to pass.
    /// </summary>
    public bool isObstacle;
}
