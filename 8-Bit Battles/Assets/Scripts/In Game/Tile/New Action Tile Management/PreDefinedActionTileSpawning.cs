using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreDefinedActionTileSpawning : MonoBehaviour 
{
	public static UnitStats unitStats;
	private static TileSpreadingManager tileSpreadingManager;
	private static FlowControl flowControl;
	int unitX;
	int unitY;

	public string weapon;

	void Start()
	{
		tileSpreadingManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TileSpreadingManager> ();
		flowControl = GameObject.FindGameObjectWithTag ("GameController").GetComponent<FlowControl> ();
	}

	void FindUnitStats()
	{
		unitStats = GameObject.FindGameObjectWithTag ("GameController").GetComponent<MouseController> ().SelectedUnit.GetComponent<UnitStats> ();
		unitX = (int) (unitStats.transform.position.x - 0.5f);
		unitY = (int) (unitStats.transform.position.y - 0.5f);
	}


	void OtherUpdating()
	{
		tileSpreadingManager.ClearActionTiles ();
		tileSpreadingManager.InitializeActionTileArray ();
		ScriptLink.unitArray.UpdateUnitArray ();
	}


	public void MovementTilesWithAttackRange()
	{
		FindUnitStats ();
		OtherUpdating ();

		//Spread valid movement
		tileSpreadingManager.StartingPositionParameters (TileSpreadingManager.StartingPosition.AroundUnit);
		tileSpreadingManager.SpreadingMethodParameters (TileSpreadingManager.SpreadingMethod.TerrainDistance, TileSpreadingManager.SpawningRestrictment.EnemyUnits, inputSpreadingDistance: unitStats.movementDistance);
		tileSpreadingManager.ActionTilePriorityParameters (TileSpreadingManager.ActionTilePriority.NewOnly);
		tileSpreadingManager.MakeActionTiles (ActionTileProperties.ActionType.Movement_Valid);

		//Make movement on allies invalid
		tileSpreadingManager.StartingPositionParameters (TileSpreadingManager.StartingPosition.ActionTileType, inputReplaceTileType: ActionTileProperties.ActionType.Movement_Valid);
		tileSpreadingManager.SpreadingMethodParameters (TileSpreadingManager.SpreadingMethod.None, TileSpreadingManager.SpawningRestrictment.AllExceptAllyUnits);
		tileSpreadingManager.ActionTilePriorityParameters (TileSpreadingManager.ActionTilePriority.New);
		tileSpreadingManager.MakeActionTiles (ActionTileProperties.ActionType.Movement_Invalid);

		//Allow valid movement on the moving unit
		tileSpreadingManager.StartingPositionParameters (TileSpreadingManager.StartingPosition.Unit);
		tileSpreadingManager.SpreadingMethodParameters (TileSpreadingManager.SpreadingMethod.None);
		tileSpreadingManager.ActionTilePriorityParameters (TileSpreadingManager.ActionTilePriority.New);
		tileSpreadingManager.MakeActionTiles (ActionTileProperties.ActionType.Movement_Valid);

		//Spread attack tiles
		tileSpreadingManager.StartingPositionParameters (TileSpreadingManager.StartingPosition.ActionTileType, inputReplaceTileType: ActionTileProperties.ActionType.Movement_Valid);
		tileSpreadingManager.SpreadingMethodParameters (TileSpreadingManager.SpreadingMethod.Distance, inputSpreadingDistance: unitStats.maxAttackRange, inputMinimumSpreadingExist: false);
		tileSpreadingManager.ActionTilePriorityParameters (TileSpreadingManager.ActionTilePriority.Existing);
		tileSpreadingManager.MakeActionTiles (ActionTileProperties.ActionType.Attack_Invalid);

		/*
		//Spread assist tiles
		tileSpreadingManager.StartingPositionParameters (TileSpreadingManager.StartingPosition.ActionTileType, inputReplaceTileType: ActionTileProperties.ActionType.Movement_Valid);
		tileSpreadingManager.SpreadingMethodParameters (TileSpreadingManager.SpreadingMethod.Distance, inputSpreadingDistance: unitStats.maxAssistRange, inputMinimumSpreadingExist: true, inputSpawningRestrictment: TileSpreadingManager.SpawningRestrictment.AllExceptAllUnits);
		tileSpreadingManager.ActionTilePriorityParameters (TileSpreadingManager.ActionTilePriority.New);
		tileSpreadingManager.MakeActionTiles (ActionTileProperties.ActionType.Assist_Valid);
		*/
	}

	public void Attacking()
	{
		FindUnitStats ();
		OtherUpdating ();

		//Spread attack tiles
		tileSpreadingManager.StartingPositionParameters (TileSpreadingManager.StartingPosition.AroundUnit);
		tileSpreadingManager.SpreadingMethodParameters (TileSpreadingManager.SpreadingMethod.Distance, TileSpreadingManager.SpawningRestrictment.None, inputSpreadingDistance: unitStats.maxAttackRange, inputMinimumSpreadingExist: true);
		tileSpreadingManager.ActionTilePriorityParameters (TileSpreadingManager.ActionTilePriority.NewOnly);
		tileSpreadingManager.MakeActionTiles (ActionTileProperties.ActionType.Attack_Invalid);

		//Destroy Attack Tile on selected unit
		if (tileSpreadingManager.actionTiles[unitX,unitY] != null)
		{
			Destroy (tileSpreadingManager.actionTiles[unitX,unitY].gameObject);
		}
		tileSpreadingManager.actionTiles [unitX, unitY] = null;

		//Validate attack tiles on enemies
		tileSpreadingManager.StartingPositionParameters (TileSpreadingManager.StartingPosition.ActionTileType, inputReplaceTileType: ActionTileProperties.ActionType.Attack_Invalid);
		tileSpreadingManager.SpreadingMethodParameters (TileSpreadingManager.SpreadingMethod.None, TileSpreadingManager.SpawningRestrictment.AllExceptEnemyUnits);
		tileSpreadingManager.ActionTilePriorityParameters (TileSpreadingManager.ActionTilePriority.New);
		tileSpreadingManager.MakeActionTiles (ActionTileProperties.ActionType.Attack_Valid);

		//Spread Assist Tiles
		tileSpreadingManager.StartingPositionParameters (TileSpreadingManager.StartingPosition.AroundUnit);
		tileSpreadingManager.SpreadingMethodParameters (TileSpreadingManager.SpreadingMethod.Distance, TileSpreadingManager.SpawningRestrictment.AllExceptAllyUnits, inputSpreadingDistance: unitStats.maxAssistRange, inputMinimumSpreadingExist: true);
		tileSpreadingManager.ActionTilePriorityParameters (TileSpreadingManager.ActionTilePriority.New);
		tileSpreadingManager.MakeActionTiles (ActionTileProperties.ActionType.Assist_Valid);
	}

	public void SpawnUnitFromBase()
	{
		OtherUpdating ();

		ScriptLink.movementTypes.unitOfChoice = MovementTypes.UnitOfChoice.SpawningUnit;
		//Grab Building and Turn info
		GameObject redBase = ScriptLink.baseSpawningArea.redBase;
		GameObject greenBase = ScriptLink.baseSpawningArea.greenBase;
		float buildingX = ((flowControl.IsRedTurn) ? redBase.transform.position.x : greenBase.transform.position.x);
		float buildingY = ((flowControl.IsRedTurn) ? redBase.transform.position.y: greenBase.transform.position.y);
		int buildingWidth = (int) ((flowControl.IsRedTurn) ? redBase.GetComponent<BuildingProperties>().buildingSizeX : greenBase.GetComponent<BuildingProperties>().buildingSizeX);
		int buildingHeight = (int) ((flowControl.IsRedTurn) ? redBase.GetComponent<BuildingProperties>().buildingSizeY : greenBase.GetComponent<BuildingProperties>().buildingSizeY);

		tileSpreadingManager.StartingPositionParameters (TileSpreadingManager.StartingPosition.AroundBuilding, inputBuildingX: buildingX, inputBuildingY: buildingY, inputBuildingWidth: buildingWidth, inputBuildingHeight: buildingHeight);
		tileSpreadingManager.SpreadingMethodParameters (TileSpreadingManager.SpreadingMethod.TerrainDistance, TileSpreadingManager.SpawningRestrictment.AllUnits, inputSpreadingDistance: 1);
		tileSpreadingManager.ActionTilePriorityParameters (TileSpreadingManager.ActionTilePriority.NewOnly);
		tileSpreadingManager.MakeActionTiles (ActionTileProperties.ActionType.Movement_Valid, inputUpdateUnitStats: false);

		ScriptLink.movementTypes.unitOfChoice = MovementTypes.UnitOfChoice.SelectedUnit;
	}

	public void WeaponSpawning(string weaponName)
	{
		OtherUpdating ();

		GameObject redBase = ScriptLink.baseSpawningArea.redBase;
		GameObject greenBase = ScriptLink.baseSpawningArea.greenBase;
		float buildingX = (int) ((flowControl.IsRedTurn) ? redBase.transform.position.x + 0.5 : greenBase.transform.position.x + 0.5);
		float buildingY = (int) ((flowControl.IsRedTurn) ? redBase.transform.position.y + 0.5: greenBase.transform.position.y + 0.5);
		int buildingWidth = (int) ((flowControl.IsRedTurn) ? redBase.GetComponent<BuildingProperties>().buildingSizeX : greenBase.GetComponent<BuildingProperties>().buildingSizeX);
		int buildingHeight = (int) ((flowControl.IsRedTurn) ? redBase.GetComponent<BuildingProperties>().buildingSizeY : greenBase.GetComponent<BuildingProperties>().buildingSizeY);

		tileSpreadingManager.StartingPositionParameters (TileSpreadingManager.StartingPosition.AroundBuilding, inputBuildingX: buildingX, inputBuildingY: buildingY, inputBuildingWidth: buildingWidth, inputBuildingHeight: buildingHeight);
		tileSpreadingManager.SpreadingMethodParameters (TileSpreadingManager.SpreadingMethod.None, TileSpreadingManager.SpawningRestrictment.None);
		tileSpreadingManager.ActionTilePriorityParameters (TileSpreadingManager.ActionTilePriority.NewOnly);
		tileSpreadingManager.MakeActionTiles (ActionTileProperties.ActionType.Attack_Invalid, inputUpdateUnitStats: false);

		tileSpreadingManager.StartingPositionParameters (TileSpreadingManager.StartingPosition.AroundBuilding, inputBuildingX: buildingX, inputBuildingY: buildingY, inputBuildingWidth: buildingWidth, inputBuildingHeight: buildingHeight);
		tileSpreadingManager.SpreadingMethodParameters (TileSpreadingManager.SpreadingMethod.Distance, TileSpreadingManager.SpawningRestrictment.AllExceptAllyUnits, inputWeaponCompatibility: TileSpreadingManager.WeaponCompatibilty.WeaponCompatible, inputWeapon: weaponName, inputSpreadingDistance: 1);
		tileSpreadingManager.ActionTilePriorityParameters (TileSpreadingManager.ActionTilePriority.New);
		tileSpreadingManager.MakeActionTiles (ActionTileProperties.ActionType.Assist_Valid, inputUpdateUnitStats: false);
	}

	public void FindUnitsAroundMine(GameObject mine)
	{
		OtherUpdating ();

		float buildingX = mine.transform.position.x;
		float buildingY = mine.transform.position.y;
		int buildingWidth = (int) mine.GetComponent<MineProperties> ().buildingSizeX;
		int buildingHeight = (int) mine.GetComponent<MineProperties> ().buildingSizeY;

		tileSpreadingManager.StartingPositionParameters (TileSpreadingManager.StartingPosition.AroundBuilding, inputBuildingX: buildingX, inputBuildingY: buildingY, inputBuildingWidth: buildingWidth, inputBuildingHeight: buildingHeight);
		tileSpreadingManager.SpreadingMethodParameters (TileSpreadingManager.SpreadingMethod.Distance, TileSpreadingManager.SpawningRestrictment.None, inputSpreadingDistance: 0);
		tileSpreadingManager.ActionTilePriorityParameters (TileSpreadingManager.ActionTilePriority.New);
		tileSpreadingManager.MakeActionTiles (ActionTileProperties.ActionType.Attack_Invalid, inputUpdateUnitStats: false);
	}
}
