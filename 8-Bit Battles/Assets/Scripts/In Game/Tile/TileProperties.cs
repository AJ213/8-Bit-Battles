using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperties
{
    public TileType tileIdentity;
    public enum TileType
    {
        Water,
        Grass,
        Wall,
        Mountain
    }

    public TileProperties(TileType tileProp)
    {
        this.tileIdentity = tileProp;
    }
}
