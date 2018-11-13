using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBuilding : MonoBehaviour
{
    [SerializeField]
    List<GameObject> buildingPrefabs;


    void BuildingSpawn(int index, float positionX, float positionY)
    {
        GameObject building = buildingPrefabs[index];
        Vector3 instantiationLocation = new Vector3(positionX + (building.GetComponent<BuildingProperties>().buildingSizeX / 2) + 0.5f, positionY + (building.GetComponent<BuildingProperties>().buildingSizeY / 2) + 0.5f, -2f); //Makes it so when the building is spawned in it is spawned in from the bottom right of the building and not the center.
        building = Instantiate(buildingPrefabs[index], instantiationLocation, Quaternion.Euler(0, 0.0f, 0.0f)) as GameObject;
        building.transform.position = instantiationLocation;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<BuildingLocations>().SetBuildingLocations((int)positionX, (int)positionY, building.GetComponent<BuildingProperties>().WhatBuilding, building.GetComponent<BuildingProperties>().buildingSize);
        building.transform.SetParent(this.gameObject.transform);
    }
    void SpawnAllBuildings()
    {
        if (GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 1)
        {
            // B A S E S //
            BuildingSpawn(0, 15, 15); //Red Base
            BuildingSpawn(1, 32, 33); //Green Base
            // P A D S //
            BuildingSpawn(2, 17, 16); //Red Pad
            BuildingSpawn(3, 32, 34); //Green pad
			// M I N E S //
			BuildingSpawn(4, 26, 17); //A Mine (Close to red) (Right)
			BuildingSpawn(4, 21, 18); //A Mine (Close to red) (Left)
			BuildingSpawn(4, 15, 30); //A Mine (Half way) (Top left)
			BuildingSpawn(4, 28, 33); //A Mine (Close to green) (Left)
			BuildingSpawn(4, 32, 29); //A Mine (Close to green) (Right)
			BuildingSpawn(4, 26, 25); //A Mine
			BuildingSpawn(4, 36, 25); //A Mine
        }
        else if (GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 2)
        {
            // B A S E S //
            BuildingSpawn(0, 16, 31);
            BuildingSpawn(1, 34, 14);
            // P A D S //
            BuildingSpawn(2, 18, 32);
            BuildingSpawn(3, 34, 15);
			// M I N E S //
			BuildingSpawn(4, 27, 17); //A Mine (Close to green) (Left)
			BuildingSpawn(4, 35, 20); //A Mine (Close to green) (Right)
			BuildingSpawn(4, 28, 25); //A Mine (Close to green) (Up)
			BuildingSpawn(4, 16, 20); //A Mine (Half way) (Bottom Left)
			BuildingSpawn(4, 22, 27); //A Mine (Close to red) (Right/Down)
			BuildingSpawn(4, 24, 36); //A Mine (Close to red) (Right/Up)
			BuildingSpawn(4, 29, 34); //A Mine (Close to red) (Right)

        }
        else if (GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 3)
        {
            // B A S E S //
            BuildingSpawn(0, 14, 16);
            BuildingSpawn(1, 32, 31);
            // P A D S //
            BuildingSpawn(2, 16, 17);
            BuildingSpawn(3, 32, 32);
			// M I N E S //
			BuildingSpawn(4, 20, 18); //A Mine (Close to red) (Right)
			BuildingSpawn(4, 16, 23); //A Mine (Close to red) (Up)
			BuildingSpawn(4, 21, 28); //A Mine (Close to red) (Right/Up)
			BuildingSpawn(4, 15, 35); //A Mine (Half way) (Top/Left)
			BuildingSpawn(4, 29, 35); //A Mine (Close to green) (Up)
			BuildingSpawn(4, 36, 28); //A Mine (Close to green) (Down/Right)
			BuildingSpawn(4, 32, 21); //A Mine (Close to green) (Down)
        }
        else
        {
            Debug.Log("N O W O R K");
        }

        ScriptLink.baseSpawningArea.BasesHaveSpawned ();
    }

    void Start()
    {
        TilesToArray.OnMapSizeInstantianted += MapSizeInstantianted;
    }
    void MapSizeInstantianted() //When the map size is instantiated it uses the values to instantiate the buildingLocations array
    {
        SpawnAllBuildings();
    }
    void OnDisable()
    {
        TilesToArray.OnMapSizeInstantianted -= MapSizeInstantianted;
    }
}
