using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesToArray : MonoBehaviour
{
    Tilemap ourTilemap;
    string[,] tileNames;
    public static TileProperties[,] tiles;
    public static Vector2Int MapBounds => mapBounds;
    private static Vector2Int mapBounds = new Vector2Int(0, 0);

    public delegate void MapSizeInstantiatedEventHandler();
    public static event MapSizeInstantiatedEventHandler OnMapSizeInstantianted;
    public static void MapSizeInstantiated()
    {
        if (OnMapSizeInstantianted != null)
        {
            OnMapSizeInstantianted();
        }
    }

    public Dictionary<string, TileProperties.TileType> tileNameStorage = new Dictionary<string, TileProperties.TileType>();

    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 1)
        {
            ourTilemap = GameObject.Find("map 1").GetComponent<Tilemap>();
            AddGrassTileSet();
        }
        else if(GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 2)
        {
            ourTilemap = GameObject.Find("map 2").GetComponent<Tilemap>();
            AddSnowTileSet();
        }
        else if(GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 3)
        {
            ourTilemap = GameObject.Find("map 3").GetComponent<Tilemap>();
            AddBeachTileSet();
        }
        else
        {
            Debug.Log("Unknown Map");
        }
        

        

        SetArrayLimits();
        TileNamesToArray();
        TileNamesArrayToTiles();

        mapBounds.x = tiles.GetLength(1);
        mapBounds.y = tiles.GetLength(0);
        MapSizeInstantiated();
        //TestArray();
    }

    void TestArray()
    {
       /* 
       foreach (TileProperties tile in tiles)
        {
            Debug.Log(tile.tileIdentity.ToString());
        }
        */
        
        for (int xaxis = 0; xaxis < tileNames.GetLength(1); xaxis++)
        {
            for (int yaxis = 0; yaxis < tileNames.GetLength(0); yaxis++)
            {
                if (tileNameStorage.ContainsKey(tileNames[xaxis, yaxis])) //Checks if the tile name in the location exists in our dictionary
                {
                    Debug.Log(tiles[xaxis, yaxis].tileIdentity.ToString() + " Location: " + xaxis + ", " + yaxis);
                }
            }
        }
        
    }

    void SetArrayLimits()
    {
        bool hasTileX = true; //Sets array limit on the x
        int x = 0;
        while (hasTileX)
        {
            hasTileX = ourTilemap.HasTile(new Vector3Int(x, 0, 0));
            if(!hasTileX)
            {
                break;
            }
            x++;
        }

        bool hasTileY = true; //Sets array limit on the y
        int y = 0;
        while (hasTileY)
        {
            hasTileY = ourTilemap.HasTile(new Vector3Int(0, y, 0));
            if (!hasTileY)
            {
                break;
            }
            y++;
        }

        tileNames = new string[x, y];
        tiles = new TileProperties[x, y];
    }
    void TileNamesToArray()
    {
        for (int xaxis = 0; xaxis < tileNames.GetLength(1); xaxis++)
        {
            for (int yaxis = 0; yaxis < tileNames.GetLength(0); yaxis++)
            {
                tileNames[xaxis, yaxis] = ourTilemap.GetTile(new Vector3Int(xaxis, yaxis, 0)).name;
            }
        }
    }
    void TileNamesArrayToTiles()
    {
        for (int xaxis = 0; xaxis < tileNames.GetLength(1); xaxis++)
        {
            for (int yaxis = 0; yaxis < tileNames.GetLength(0); yaxis++)
            {
                if(tileNameStorage.ContainsKey(tileNames[xaxis, yaxis])) //Checks if the tile name in the location exists in our dictionary
                {
                    tiles[xaxis, yaxis] = new TileProperties(tileNameStorage[tileNames[xaxis, yaxis]]); //Takes the name of the tile and uses it to set a tileIdentity value for this specific tile
                }
            }
        }
    }

    void AddBeachTileSet()
    { 
        tileNameStorage.Add("Deep Deep Water", TileProperties.TileType.Mountain);
        tileNameStorage.Add("Deep Water", TileProperties.TileType.Wall);
		tileNameStorage.Add("Beach", TileProperties.TileType.Grass);
        tileNameStorage.Add("Beach Edge 1", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Edge 2", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Edge 3", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Edge 4", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Edge 5", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Edge 6", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Edge 7", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Edge 8", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Edge 9", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Edge 10", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Edge 11", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Edge 12", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Edge 13", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Edge 14", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Edge 15", TileProperties.TileType.Water);
        tileNameStorage.Add("Beach Variant", TileProperties.TileType.Grass);
        tileNameStorage.Add("Beach Mountain", TileProperties.TileType.Mountain);
        tileNameStorage.Add("Beach Tree", TileProperties.TileType.Wall);
        tileNameStorage.Add("Water", TileProperties.TileType.Water);
    }
    void AddSnowTileSet()
    {
        tileNameStorage.Add("Deep Deep Water", TileProperties.TileType.Mountain);
        tileNameStorage.Add("Deep Water", TileProperties.TileType.Wall);
		tileNameStorage.Add("Snow", TileProperties.TileType.Grass);
        tileNameStorage.Add("Snow Edge 1", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Edge 2", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Edge 3", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Edge 4", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Edge 5", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Edge 6", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Edge 7", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Edge 8", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Edge 9", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Edge 10", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Edge 11", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Edge 12", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Edge 13", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Edge 14", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Edge 15", TileProperties.TileType.Water);
        tileNameStorage.Add("Snow Variant", TileProperties.TileType.Grass);
        tileNameStorage.Add("Snow Mountain", TileProperties.TileType.Mountain);
        tileNameStorage.Add("Snow Tree", TileProperties.TileType.Wall);
        tileNameStorage.Add("Water", TileProperties.TileType.Water);
    }
    void AddGrassTileSet()
    {
        tileNameStorage.Add("Deep Deep Water", TileProperties.TileType.Mountain);
        tileNameStorage.Add("Deep Water", TileProperties.TileType.Wall);
        tileNameStorage.Add("Grass", TileProperties.TileType.Grass);
        tileNameStorage.Add("Grass Edge 1", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Edge 2", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Edge 3", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Edge 4", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Edge 5", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Edge 6", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Edge 7", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Edge 8", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Edge 9", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Edge 10", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Edge 11", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Edge 12", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Edge 13", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Edge 14", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Edge 15", TileProperties.TileType.Water);
        tileNameStorage.Add("Grass Variant", TileProperties.TileType.Grass);
        tileNameStorage.Add("Mountain", TileProperties.TileType.Mountain);
        tileNameStorage.Add("Tree", TileProperties.TileType.Wall);
        tileNameStorage.Add("Water", TileProperties.TileType.Water);
    } //Adds tile names to the dictionary with their associated tileIdentity
}
