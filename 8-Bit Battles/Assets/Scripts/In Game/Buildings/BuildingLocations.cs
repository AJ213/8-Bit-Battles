using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingLocations : MonoBehaviour
{
    public BuildingProperties.BuildingIdentity[,] BuildingIdentityLocations;

    bool instantiatedOnce;

    public void SetBuildingLocations(int startX, int startY, BuildingProperties.BuildingIdentity identity, BuildingProperties.BuildingIdentity[,] buildingSize)
    {
        if(instantiatedOnce == false)
        {
            InstantiateArray();
            instantiatedOnce = true;
        }

        for (int x = 0; x < buildingSize.GetLength(1); x++)
        {
            for (int y = 0; y < buildingSize.GetLength(0); y++)
            {
                BuildingIdentityLocations[startX + x, startY + y] = buildingSize[x, y];
            }
        }
    }
    void InstantiateArray()
    {
        BuildingIdentityLocations = new BuildingProperties.BuildingIdentity[TilesToArray.MapBounds.x, TilesToArray.MapBounds.y];
        for (int x = 0; x < BuildingIdentityLocations.GetLength(1); x++)
        {
            for (int y = 0; y < BuildingIdentityLocations.GetLength(0); y++)
            {
                BuildingIdentityLocations[x, y] = BuildingProperties.BuildingIdentity.None;
            }
        }
    }
}
