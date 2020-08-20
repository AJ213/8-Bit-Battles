using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTypes : MonoBehaviour 
{
	private static UnitStats unitStats;

	public int index;
	public UnitOfChoice unitOfChoice = UnitOfChoice.SelectedUnit;
	public enum UnitOfChoice
	{
		SelectedUnit,
		SpawningUnit
	}

	public int MovementCostByMovementType(int x, int y)
	{
		TileProperties.TileType tileType = TilesToArray.tiles [x, y].tileIdentity;
		switch (unitOfChoice)
		{
			case UnitOfChoice.SelectedUnit:
				unitStats = GameObject.FindGameObjectWithTag ("GameController").GetComponent<MouseController> ().SelectedUnit.GetComponent<UnitStats> ();
				break;
			case UnitOfChoice.SpawningUnit:
				unitStats = ScriptLink.unitSpawner.unitPrefabs [index].GetComponent<UnitStats> ();
				break;
		}
		int movementCost = 1;

		if (unitStats.groundmovement == true) 
		{
			if (tileType == TileProperties.TileType.Grass) 
			{
				movementCost = 1;
			}
			else if (tileType == TileProperties.TileType.Water) 
			{
				movementCost = 999;
			}
			else if (tileType == TileProperties.TileType.Wall) 
			{
				movementCost = 999;
			}
			else if (tileType == TileProperties.TileType.Mountain) 
			{
				movementCost = 999;
			}
			else
			{
				movementCost = 999;
			}
		}

		else if (unitStats.watermovement == true) 
		{
			if (tileType == TileProperties.TileType.Grass) 
			{
				movementCost = 1;
			}
			else if (tileType == TileProperties.TileType.Water) 
			{
				movementCost = 2;
			}
			else if (tileType == TileProperties.TileType.Wall) 
			{
				movementCost = 999;
			}
			else if (tileType == TileProperties.TileType.Mountain) 
			{
				movementCost = 999;
			}
			else
			{
				movementCost = 999;
			}
		}

		else if (unitStats.flier == true) 
		{
			if (tileType == TileProperties.TileType.Grass) 
			{
				movementCost = 1;
			}
			else if (tileType == TileProperties.TileType.Water) 
			{
				movementCost = 1;
			}
			else if (tileType == TileProperties.TileType.Wall) 
			{
				movementCost = 1;
			}
			else if (tileType == TileProperties.TileType.Mountain) 
			{
				movementCost = 999;
			}
			else
			{
				movementCost = 999;
			}
		}

		else
		{
			movementCost = 999;
		}
			
		return movementCost;
	}
}
