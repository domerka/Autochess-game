using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLayoutScript : MonoBehaviour
{
    public GameObject hexPrefab;
    public GameObject sqaurePrefab;

    public int gridWidth = 7;
    public int gridHeight = 8;

    float hexWidth = 1.127f;
    float hexHeight = 1.1f;
    public float gap = 0.0f;

    Vector3 startPos;

    void Start()
    {
        AddGap();
        CalcStartPos();
        CreateGrid();
    }

    void AddGap()
    {
        hexWidth += hexWidth * gap;
        hexHeight += hexHeight * gap;
    }

    void CalcStartPos()
    {
        float offset = 0;
        if (gridHeight / 2 % 2 != 0)
            offset = hexWidth / 2;

        float x = -hexWidth * (gridWidth / 2) - offset;
        float z = hexHeight * 0.9f * (gridHeight / 2);

        startPos = new Vector3(x, 0, z);
    }

    Vector3 CalcWorldPos(Vector2 gridPos)
    {
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;

        float x = startPos.x + gridPos.x * hexWidth + offset;
        float z = startPos.z - gridPos.y * hexHeight * 0.9f;

        return new Vector3(x, 0, z);
    }

    void CreateGrid()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                GameObject hex = Instantiate(hexPrefab);
                Vector2 gridPos = new Vector2(x, y);
                hex.transform.position = CalcWorldPos(gridPos);
                hex.transform.parent = this.transform;
                hex.name = x+""+y;


                hex.GetComponent<Renderer>().enabled = false;
                hex.tag = "Free";
                hex.AddComponent<Tile>();

                int tileColumn = hex.name[0] - '0';
                int tileRow = hex.name[1] - '0';

                var xTile = tileColumn - (tileRow - (tileRow & 1)) / 2;
                var zTile = tileRow;
                var yTile = -xTile - zTile;

                Vector3Int tileCoordinates = new Vector3Int(xTile, zTile, yTile);

                hex.GetComponent<Tile>().position = tileCoordinates;
                hex.GetComponent<Tile>().isObstacle = false;
            }
        }
        //create bench

        Vector3 posi = new Vector3(-7.89511395f, 0.0267623849f, -8.21996975f);
        for (int i = 0; i < 9;i++)
        {
            GameObject hex = Instantiate(sqaurePrefab);
            hex.transform.position = posi;
            hex.transform.parent = this.transform;
            hex.name = i.ToString();
            hex.tag = "FreeBench";
            hex.GetComponent<Renderer>().material.color = Color.blue;
            hex.GetComponent<Renderer>().enabled = false;
            posi.x += 2.0f;
        }

    }
}

