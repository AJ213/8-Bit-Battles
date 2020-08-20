using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpreadingManager : MonoBehaviour 
{
	public static UnitStats unitStats;
	int unitX;
	int unitY;

	public static Object prefabMovementTileValid;
	public static Object prefabMovementTileInvalid;
	public static Object prefabAttackTileValid;
	public static Object prefabAttackTileInvalid;
	public static Object prefabAssistTileValid;
	public static Object prefabAssistTileInvalid;
	public Object prefabActionTile;

	public GameObject[,] actionTiles;
	public GameObject[,] tempActionTiles;

	public int actionTileZPosition;
	int mapWidth;
	int mapHeight;

	public bool[,] actionTileSpawningPlan;
		
	/*

	Basic Functions

	*/

	void Start()
	{
		prefabMovementTileValid = Resources.Load ("Movement Tile Valid");
		prefabMovementTileInvalid = Resources.Load ("Movement Tile Invalid");
		prefabAttackTileValid = Resources.Load ("Attack Tile Valid");
		prefabAttackTileInvalid = Resources.Load ("Attack Tile Invalid");
		prefabAssistTileValid = Resources.Load ("Assist Tile Valid");
		prefabAssistTileInvalid = Resources.Load ("Assist Tile Invalid");
	}

	public void InitializeActionTileArray()
	{
		mapWidth = TilesToArray.MapBounds.x;
		mapHeight = TilesToArray.MapBounds.y;
		actionTiles = new GameObject[mapWidth, mapHeight];
		tempActionTiles = new GameObject[mapWidth, mapHeight];
		actionTileSpawningPlan = new bool[mapWidth, mapHeight];
	}

	public void ClearActionTiles()
	{
		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				if (actionTiles [x, y] != null)
				{
					Destroy (actionTiles [x, y].gameObject);
					actionTiles [x, y] = null;
				} 
				if (tempActionTiles [x, y] != null)
				{
					Destroy (tempActionTiles [x, y].gameObject);
					tempActionTiles [x, y] = null;
				}
				actionTileSpawningPlan [x, y] = false;
			}
		}
	}
		
	public void CountActionTiles(out int total, out int redUnits, out int greenUnits)
	{
		total = 0;
		redUnits = 0;
		greenUnits = 0;
		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				if (actionTiles [x, y] != null)
				{
					if (ScriptLink.unitArray.allUnits [x, y] != null)
					{
						total++;
						if (ScriptLink.unitArray.allUnits [x, y].gameObject.GetComponent<UnitStats> ().isRed)
						{
							redUnits++;
						}
						else
						{
							greenUnits++;
						}
					}
				} 
			}
		}
	}
		
	void ClearActionTileSpawningPlan()
	{
		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				actionTileSpawningPlan [x, y] = false;
			}
		}
	}
		
	/*

	Starting Position Setup

	*/

	float buildingX;
	float buildingY;
	float buildingWidth;
	float buildingHeight;
	ActionTileProperties.ActionType replaceTileType;
	StartingPosition startingPosition;
	public enum StartingPosition
	{
		AroundUnit,
		ActionTileType,
		AroundBuilding,
		Unit
	}

	public void StartingPositionParameters 
	(
		StartingPosition inputStartingPosition,
		float inputBuildingX = 0,
		float inputBuildingY = 0,
		float inputBuildingWidth = 0,
		float inputBuildingHeight = 0,
		ActionTileProperties.ActionType inputReplaceTileType = ActionTileProperties.ActionType.Movement_Valid
	)
	{
		startingPosition = inputStartingPosition;
		buildingX = inputBuildingX;
		buildingY = inputBuildingY;
		buildingWidth = inputBuildingWidth;
		buildingHeight = inputBuildingHeight;
		replaceTileType = inputReplaceTileType;
	}

	/*

	Spreading Method Setup

	*/

	int spreadingDistance;
	bool minimumSpreadingExist;
	SpreadingMethod spreadingMethod;
	public enum SpreadingMethod
	{
		TerrainDistance,
		Distance,
		None,
	}
	SpawningRestrictment spawningRestrictment;
	public enum SpawningRestrictment
	{
		None,
		AllUnits,
		AllyUnits,
		EnemyUnits,
		AllExceptAllUnits,
		AllExceptAllyUnits,
		AllExceptEnemyUnits,
	}
	WeaponCompatibilty weaponCompatibility;
	public enum WeaponCompatibilty
	{
		None,
		WeaponCompatible,
		WeaponIncompatible
	}
	string weapon;

	public void SpreadingMethodParameters 
	(
		SpreadingMethod inputSpreadingMethod,
		SpawningRestrictment inputSpawningRestrictment = SpawningRestrictment.None,
		int inputSpreadingDistance = 0,
		bool inputMinimumSpreadingExist = false,
		WeaponCompatibilty inputWeaponCompatibility = WeaponCompatibilty.None,
		string inputWeapon = "none"
	)
	{
		spreadingMethod = inputSpreadingMethod;
		spawningRestrictment = inputSpawningRestrictment;
		spreadingDistance = inputSpreadingDistance;
		minimumSpreadingExist = inputMinimumSpreadingExist;
		weaponCompatibility = inputWeaponCompatibility;
		weapon = inputWeapon;
	}

	/*

	Merging Different ActionTiles Setup

	*/

	ActionTilePriority actionTilePriority;
	public enum ActionTilePriority
	{
		New,
		NewOnly,
		Existing,
	}

	public void ActionTilePriorityParameters
	(
		ActionTilePriority inputActionTilePriority
	)
	{
		actionTilePriority = inputActionTilePriority;
	}

	/*

	The Function that trully starts it all

	*/

	ActionTileProperties.ActionType actionTileType;
	bool updateUnitStats;

	public void MakeActionTiles(ActionTileProperties.ActionType inputActionTileType, bool inputUpdateUnitStats = true)
	{
		updateUnitStats = inputUpdateUnitStats;
		actionTileType = inputActionTileType;
		if (updateUnitStats == true)
		{
			unitStats = GameObject.FindGameObjectWithTag ("GameController").GetComponent<MouseController> ().SelectedUnit.GetComponent<UnitStats> ();
			unitX = (int)(unitStats.transform.position.x - 0.5f);
			unitY = (int)(unitStats.transform.position.y - 0.5f);
		}
		ClearActionTileSpawningPlan ();
		UseActionTileType ();
		CreateFirstActionTiles();
		SpreadActionTiles ();
		Transfer_TempActionTiles_To_ActionTiles ();
	}

	/*

	Setting ActionTile Type to be spawned

	*/

	void UseActionTileType()
	{
		switch (actionTileType)
		{
			case ActionTileProperties.ActionType.Movement_Valid:
				prefabActionTile = prefabMovementTileValid;
				break;
			case ActionTileProperties.ActionType.Movement_Invalid:
				prefabActionTile = prefabMovementTileInvalid;
				break;
			case ActionTileProperties.ActionType.Attack_Valid:
				prefabActionTile = prefabAttackTileValid;
				break;
			case ActionTileProperties.ActionType.Attack_Invalid:
				prefabActionTile = prefabAttackTileInvalid;
				break;
			case ActionTileProperties.ActionType.Assist_Valid:
				prefabActionTile = prefabAssistTileValid;
				break;
			case ActionTileProperties.ActionType.Assist_Invalid:
				prefabActionTile = prefabAssistTileInvalid;
				break;
		}
	
	}
		
	/*

	Creating First ActionTiles

	*/

	void CreateFirstActionTiles()
	{
		switch (startingPosition)
		{
			case StartingPosition.AroundUnit:
				CreateActionTiles_AroundUnit ();
				break;
			case StartingPosition.ActionTileType:
				CreateActionTiles_ActionTileType ();
				break;
			case StartingPosition.AroundBuilding:
				CreateActionTiles_AroundBuilding (buildingX, buildingY, buildingWidth, buildingHeight);
				break;
			case StartingPosition.Unit:
				CreateActionTiles_Unit ();
				break;
		}
	}



	void CreateActionTiles_AroundUnit()
	{
		for (int cardinalDirection = 0; cardinalDirection < 4; cardinalDirection++)
		{
			int yTranslation = (int)(Mathf.Sin ((cardinalDirection * 0.5f) * Mathf.PI));
			int xTranslation = (int)(Mathf.Cos ((cardinalDirection * 0.5f) * Mathf.PI));
			if (ValidSpawning(unitX + xTranslation, unitY + yTranslation) == true)
			{
				actionTileSpawningPlan [unitX + xTranslation, unitY + yTranslation] = true;
			}
		}
	}



	void CreateActionTiles_ActionTileType()
	{
		spreadingDistance += 1;
		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				if (actionTiles [x, y] != null)
				{
					if (actionTiles [x, y].gameObject.GetComponent<ActionTileProperties> ().actionType == replaceTileType)
					{
						actionTileSpawningPlan [x, y] = true;
					}
				}
			}
		}
	}



	void CreateActionTiles_AroundBuilding(float buildingX, float buildingY, float buildingWidth, float buildingHeight)
	{
		for (int corner = 0; corner < 4; corner++)
		{
			int xTranslation = (int)(Mathf.Cos (((corner + 2.0f) * 0.5f) * Mathf.PI));
			int yTranslation = (int)(Mathf.Sin (((corner + 2.0f) * 0.5f) * Mathf.PI));

			float xSpot = (buildingX + ((Mathf.Cos (((corner + 0.5f) * 0.5f) * Mathf.PI) * Mathf.Sqrt(2)) * ((buildingWidth / 2.0f) + 0.5f)));
			float ySpot = (buildingY + ((Mathf.Sin (((corner + 0.5f) * 0.5f) * Mathf.PI) * Mathf.Sqrt(2)) * ((buildingHeight / 2.0f) + 0.5f)));

			bool aboveXMinimum = buildingX - ((buildingWidth / 2.0f) + 0.5f) <= xSpot;
			bool belowXMaximum = buildingX + ((buildingWidth / 2.0f) + 0.5f) >= xSpot;
			bool aboveYMinimum = buildingY - ((buildingHeight / 2.0f) + 0.5f) <= ySpot;
			bool belowYMaximum = buildingY + ((buildingHeight / 2.0f) + 0.5f) >= ySpot;

			while ((aboveXMinimum && belowXMaximum) && (aboveYMinimum && belowYMaximum))
			{
				actionTileSpawningPlan [(int) (xSpot - 0.5f), (int) (ySpot - 0.5f)] = true;
				xSpot += xTranslation;
				ySpot += yTranslation;
				aboveXMinimum = buildingX - ((buildingWidth / 2.0f) + 0.5f) <= xSpot;
				belowXMaximum = buildingX + ((buildingWidth / 2.0f) + 0.5f) >= xSpot;
				aboveYMinimum = buildingY - ((buildingHeight / 2.0f) + 0.5f) <= ySpot;
				belowYMaximum = buildingY + ((buildingHeight / 2.0f) + 0.5f) >= ySpot;
			}
		}
	}



	void CreateActionTiles_Unit()
	{
		actionTileSpawningPlan [unitX, unitY] = true;
	}


	/*

	Spreading ActionTiles

	*/

	void SpreadActionTiles ()
	{
		SpawnPlannedTiles ();

		switch (spreadingMethod)
		{
			case SpreadingMethod.None:
				SpreadActionTiles_Distance (false, 0);
				break;
			case SpreadingMethod.Distance:
				SpreadActionTiles_Distance (false, spreadingDistance);
				break;
			case SpreadingMethod.TerrainDistance:
				SpreadActionTiles_Distance (true, spreadingDistance);
				break;
		}

	}



	void SpreadActionTiles_Distance (bool usingTerrain, int distance)
	{
		for (int i = distance; i > 0; i--)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++) 
				{
					if(tempActionTiles[x,y] != null)
					{
						if (tempActionTiles [x, y].gameObject.GetComponent<ActionTileProperties>().actionTilePhase == ActionTileProperties.ActionTilePhase.Avaliable) 
						{
							SpreadActionTiles_CardinalDirections (x,y);
						}
					}
				}
			}
			MakeSpreadingAvaliability ();
		}
	}



	void SpreadActionTiles_CardinalDirections (int x, int y)
	{
		int passedMovementLeft;
		for (int cardinalDirection = 0; cardinalDirection < 4; cardinalDirection++)
		{
			int yTranslation = (int)(Mathf.Sin ((cardinalDirection * 0.5f) * Mathf.PI));
			int xTranslation = (int)(Mathf.Cos ((cardinalDirection * 0.5f) * Mathf.PI));

			switch (spreadingMethod)
			{
				case SpreadingMethod.None:
					break;
				case SpreadingMethod.Distance:
					passedMovementLeft = tempActionTiles [x, y].gameObject.GetComponent<ActionTileProperties> ().distanceLeft - 1;
					if (tempActionTiles [x + xTranslation, y + yTranslation] == null)
					if (passedMovementLeft > 0)
					{
						if (tempActionTiles [x + xTranslation, y + yTranslation] != null)
						{
							if (tempActionTiles [x + xTranslation, y + yTranslation].gameObject.GetComponent<ActionTileProperties> ().distanceLeft < tempActionTiles [x, y].gameObject.GetComponent<ActionTileProperties> ().distanceLeft)
							{
								Destroy (tempActionTiles [x + xTranslation, y + yTranslation].gameObject);
								SpawnActionTile (x + xTranslation, y + yTranslation, passedMovementLeft);
							}

						}
						else
						{
							SpawnActionTile (x + xTranslation, y + yTranslation, passedMovementLeft);
						}
					}
					tempActionTiles [x, y].gameObject.GetComponent<ActionTileProperties> ().actionTilePhase = ActionTileProperties.ActionTilePhase.Used;
					break;
				case SpreadingMethod.TerrainDistance:
					passedMovementLeft = tempActionTiles [x, y].gameObject.GetComponent<ActionTileProperties> ().distanceLeft - ScriptLink.movementTypes.MovementCostByMovementType (x + xTranslation, y + yTranslation);
					if (passedMovementLeft > 0)
					{
						if (tempActionTiles [x + xTranslation, y + yTranslation] != null)
						{
							if (tempActionTiles [x + xTranslation, y + yTranslation].gameObject.GetComponent<ActionTileProperties> ().distanceLeft < tempActionTiles [x, y].gameObject.GetComponent<ActionTileProperties> ().distanceLeft)
							{
								Destroy (tempActionTiles [x + xTranslation, y + yTranslation].gameObject);
								SpawnActionTile (x + xTranslation, y + yTranslation, passedMovementLeft);
							}

						}
						else
						{
							SpawnActionTile (x + xTranslation, y + yTranslation, passedMovementLeft);
						}
					}
					tempActionTiles [x, y].gameObject.GetComponent<ActionTileProperties> ().actionTilePhase = ActionTileProperties.ActionTilePhase.Used;
					break;
			}
		}
	}


	void DeleteShortDistances()
	{
		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				if (tempActionTiles [x, y] != null)
				{
					switch (actionTileType)
					{
						case ActionTileProperties.ActionType.Attack_Invalid:
							if (tempActionTiles [x, y].gameObject.GetComponent<ActionTileProperties> ().distanceLeft - 1 > unitStats.maxAttackRange - unitStats.minAttackRange)
							{
								Destroy (tempActionTiles [x, y].gameObject);
								tempActionTiles [x, y] = null;
							}
							break;
						case ActionTileProperties.ActionType.Assist_Valid:
							if (tempActionTiles [x, y].gameObject.GetComponent<ActionTileProperties> ().distanceLeft - 1 > unitStats.maxAssistRange - unitStats.minAssistRange)
							{
								Destroy (tempActionTiles [x, y].gameObject);
								tempActionTiles [x, y] = null;
							}
							break;
					}
				}
			}
		}
	}


	void Transfer_TempActionTiles_To_ActionTiles()
	{
		if (minimumSpreadingExist == true) 
		{
			DeleteShortDistances ();
		}


		for (int y = 0; y < mapHeight; y++) 
		{
			for (int x = 0; x < mapWidth; x++) 
			{
				switch (actionTilePriority)
				{
					case ActionTilePriority.NewOnly:
						if (actionTiles [x, y] != null)
						{
							Destroy (actionTiles [x, y].gameObject);
							actionTiles [x, y] = null;
						}
						if (tempActionTiles [x, y] != null)
						{
							actionTiles [x, y] = tempActionTiles [x, y];
							tempActionTiles [x, y] = null;
						}
						break;

					case ActionTilePriority.New:
						if (tempActionTiles [x, y] != null)
						{
							if (actionTiles [x, y] != null)
							{
								Destroy (actionTiles [x, y].gameObject);
								actionTiles [x, y] = null;
							}
							actionTiles [x, y] = tempActionTiles [x, y];
							tempActionTiles [x, y] = null;
						}
						break;

					case ActionTilePriority.Existing:
						if (tempActionTiles [x, y] != null)
						{
							if (actionTiles [x, y] == null)
							{
								actionTiles [x, y] = tempActionTiles [x, y];
							}
							else
							{
								Destroy (tempActionTiles [x, y].gameObject);
							}
							tempActionTiles [x, y] = null;
						}
						break;
				}
			}
		}
	}




	void MakeSpreadingAvaliability()
	{
		for (int y = 0; y < mapHeight; y++) 
		{
			for (int x = 0; x < mapWidth; x++) 
			{
				if (tempActionTiles [x, y] != null)
				{
					if (tempActionTiles [x, y].gameObject.GetComponent<ActionTileProperties>().actionTilePhase == ActionTileProperties.ActionTilePhase.Created)
					{
						tempActionTiles [x, y].GetComponent<ActionTileProperties> ().actionTilePhase = ActionTileProperties.ActionTilePhase.Avaliable;
					}
				}
			}
		}
	}



	bool ValidSpawning(int x, int y)
	{
		bool validSpawning = true;
		bool compatibleWeapon = false;

		ScriptLink.unitArray.UpdateUnitArray ();

		switch (spawningRestrictment)
		{
			case SpawningRestrictment.None:
				break;
			case SpawningRestrictment.AllUnits:
				validSpawning = (ScriptLink.unitArray.allUnits [x, y] == null) ? true : false;
				break;
			case SpawningRestrictment.AllExceptAllUnits:
				validSpawning = (ScriptLink.unitArray.allUnits [x, y] == null) ? false : true;
				break;
			case SpawningRestrictment.AllyUnits:
				if (ScriptLink.unitArray.allUnits [x, y] != null)
				{
					validSpawning = (ScriptLink.unitArray.allUnits [x, y].gameObject.GetComponent<UnitStats> ().isRed == ScriptLink.flowController.IsRedTurn) ? false : true;
				}
				else
				{
					validSpawning = true;
				}
				break;
			case SpawningRestrictment.AllExceptAllyUnits:
				if (ScriptLink.unitArray.allUnits [x, y] != null)
				{
					validSpawning = (ScriptLink.unitArray.allUnits [x, y].gameObject.GetComponent<UnitStats> ().isRed == ScriptLink.flowController.IsRedTurn) ? true : false;
				}
				else
				{
					validSpawning = false;
				}
				break;
			case SpawningRestrictment.EnemyUnits:
				if (ScriptLink.unitArray.allUnits [x, y] != null)
				{
					validSpawning = (ScriptLink.unitArray.allUnits [x, y].gameObject.GetComponent<UnitStats> ().isRed == ScriptLink.flowController.IsRedTurn) ? true : false;
				}
				else
				{
					validSpawning = true;
				}
				break;
			case SpawningRestrictment.AllExceptEnemyUnits:
				if (ScriptLink.unitArray.allUnits [x, y] != null)
				{
					validSpawning = (ScriptLink.unitArray.allUnits [x, y].gameObject.GetComponent<UnitStats> ().isRed == ScriptLink.flowController.IsRedTurn) ? false : true;
				}
				else
				{
					validSpawning = false;
				}
				break;
		}

		if (spreadingMethod != SpreadingMethod.Distance)
		{
			bool isBase = ScriptLink.buildingLocations.BuildingIdentityLocations [x, y] == BuildingProperties.BuildingIdentity.Base;
			bool isMine = ScriptLink.buildingLocations.BuildingIdentityLocations [x, y] == BuildingProperties.BuildingIdentity.Mine;
			if (isBase || isMine)
			{
				validSpawning = false;
			}
		}

		if (spreadingMethod == SpreadingMethod.TerrainDistance && spreadingDistance - ScriptLink.movementTypes.MovementCostByMovementType (x, y) < 0)
		{
			validSpawning = false;
		}

		if (ScriptLink.unitArray.allUnits [x, y] != null)
		{
			switch (weaponCompatibility)
			{
				case WeaponCompatibilty.None:
					break;
				case WeaponCompatibilty.WeaponCompatible:
					for (int i = 0; i < ScriptLink.unitArray.allUnits [x, y].GetComponent<UnitStats> ().WeaponsThisUnitCanUse.Length; i++)
					{
						if (ScriptLink.unitArray.allUnits [x, y].GetComponent<UnitStats> ().WeaponsThisUnitCanUse [i] == ScriptLink.allWeapons.weapons[weapon].WeaponType)
						{
							compatibleWeapon = true;
							for (int j = 0; j < ScriptLink.unitArray.allUnits [x, y].GetComponent<UnitInventory>().Inventory.Length; j++)
							{
								if (ScriptLink.unitArray.allUnits [x, y].GetComponent<UnitInventory> ().Inventory [j] == ScriptLink.allWeapons.weapons[weapon])
								{
									compatibleWeapon = false;
								}
							}
						}
					}
					if (compatibleWeapon == false)
					{
						validSpawning = false;
					}
					break;
				case WeaponCompatibilty.WeaponIncompatible:
					//Copy and modify above case
					break;
			}
		}
		return validSpawning;
	}


	void SpawnActionTile(int x, int y, int distanceLeft)
	{
		if (ValidSpawning(x,y) == true)
		{
			GameObject newActionTile = Instantiate (prefabActionTile, new Vector3 (x + 0.5f, y + 0.5f, actionTileZPosition), Quaternion.identity) as GameObject;
			newActionTile.GetComponent<ActionTileProperties> ().actionTilePhase = ActionTileProperties.ActionTilePhase.Created;

			switch (spreadingMethod)
			{
				case SpreadingMethod.None:
					break;
				case SpreadingMethod.Distance:
					newActionTile.GetComponent<ActionTileProperties> ().distanceLeft = distanceLeft;
					break;
				case SpreadingMethod.TerrainDistance:
					newActionTile.GetComponent<ActionTileProperties> ().distanceLeft = distanceLeft;
					break;
			}
			tempActionTiles [x, y] = newActionTile;
		}
	}




	void SpawnPlannedTiles()
	{
		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				if (actionTileSpawningPlan [x, y] == true && ValidSpawning(x,y) == true)
				{
					GameObject newActionTile = Instantiate (prefabActionTile, new Vector3(x + 0.5f, y + 0.5f, actionTileZPosition), Quaternion.identity) as GameObject;
					SetFirstActionTileProperties (ref newActionTile);
					tempActionTiles [x, y] = newActionTile;
				}
			}
		}
	}



	GameObject SetFirstActionTileProperties(ref GameObject actionTile)
	{
		actionTile.GetComponent<ActionTileProperties> ().actionTilePhase = ActionTileProperties.ActionTilePhase.Avaliable;
		switch (spreadingMethod)
		{
			case SpreadingMethod.None:
				break;
			case SpreadingMethod.Distance:
				actionTile.GetComponent<ActionTileProperties> ().distanceLeft = spreadingDistance;
				break;
			case SpreadingMethod.TerrainDistance:
				actionTile.GetComponent<ActionTileProperties> ().distanceLeft = spreadingDistance;
				break;
		}
		return actionTile;
	}

}
