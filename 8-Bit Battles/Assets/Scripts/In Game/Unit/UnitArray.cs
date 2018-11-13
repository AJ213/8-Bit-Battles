using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitArray : MonoBehaviour 
{
	public GameObject[,] allUnits;

	public void UpdateUnitArray()
	{
		allUnits = new GameObject[ScriptLink.tilesToArray.mapXLimit, ScriptLink.tilesToArray.mapYLimit];
		ClearUnitArray ();
		foreach (GameObject Unit in ScriptLink.unitSpawner.redUnits) 
		{
			allUnits [(int) (Unit.transform.position.x - 0.5f), (int) (Unit.transform.position.y - 0.5f)] = Unit;
		}
		foreach (GameObject Unit in ScriptLink.unitSpawner.greenUnits) 
		{
			allUnits [(int) (Unit.transform.position.x - 0.5f), (int) (Unit.transform.position.y - 0.5f)] = Unit;
		}
	}

	void ClearUnitArray()
	{
		int x;
		int y;
		for (x = 0; x < ScriptLink.tilesToArray.mapXLimit; x++) 
		{
			for (y = 0; y < ScriptLink.tilesToArray.mapYLimit; y++) 
			{
				allUnits [x, y] = null;
			}
		}
	}
}
