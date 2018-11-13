using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingProperties : MonoBehaviour
{
    public int buildingSizeX;
    public int buildingSizeY;
    public BuildingIdentity[,] buildingSize;

    public bool isCollidable;
    public BuildingIdentity WhatBuilding;
    public enum BuildingIdentity
    {
        None,
        Base,
        Mine,
        Pad
    }
    public bool isRedBuilding;
    public bool isGreenBuilding;

    void Awake()
    {
        SetBuildingSize();
    }

    void SetBuildingSize()
    {
        InstantiateBuildingSize();
        if (WhatBuilding == BuildingIdentity.Base)
        {
            SetBaseSize();
        }
        else if(WhatBuilding == BuildingIdentity.Pad)
        {
            buildingSize[0, 0] = BuildingIdentity.Pad;
        }
        else if (WhatBuilding == BuildingIdentity.Mine)
        {
            buildingSize[0, 0] = BuildingIdentity.Mine;
        }
    }

    void SetBaseSize()
    {
        for (int x = 0; x < buildingSize.GetLength(1); x++)
        {
            for (int y = 0; y < buildingSize.GetLength(0); y++)
            {
                if (isRedBuilding)
                {
                    if (x != 2 || y != 1)
                    {
                        buildingSize[x, y] = BuildingIdentity.Base;
                    }
                }
                else
                {
                    if (x != 0 || y != 1)
                    {
                        buildingSize[x, y] = BuildingIdentity.Base;
                    }
                }
            }
        }
    }

    void InstantiateBuildingSize()
    {
        buildingSize = new BuildingIdentity[buildingSizeX, buildingSizeY];
        for (int x = 0; x < buildingSize.GetLength(1); x++)
        {
            for (int y = 0; y < buildingSize.GetLength(0); y++)
            {
                buildingSize[x, y] = BuildingIdentity.None;
            }
        }
    }
}
