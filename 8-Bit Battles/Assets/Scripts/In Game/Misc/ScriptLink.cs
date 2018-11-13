using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptLink : MonoBehaviour
{
    public static TilesToArray tilesToArray;
    public static MouseController mouseController;
    public static FlowControl flowController;
    public static UnitMenuControl unitMenuControl;
    public static UnitSpawner unitSpawner;
	public static MovementTypes movementTypes;
	public static UnitArray unitArray;
    public static AllWeapons allWeapons;
    public static EconomyController economyController;
    public static BuildingLocations buildingLocations;
    public static UIController UIcontroller;
	public static BaseSpawningArea baseSpawningArea;
	public static TileSpreadingManager tileSpreadingManager;
	public static PreDefinedActionTileSpawning preDefinedActionTileSpawning;
    public static TryingToSpawnAUnit tryingToSpawnAUnit;
    public static TryingToGiveAUnitAWeapon tryingToGiveAUnitAWeapon;
    public static UnitBattleResult unitBattleResult;

    void Start()
    {
        InitializeScriptLinks();
    }

    void InitializeScriptLinks()
    {
        tilesToArray = GameObject.FindGameObjectWithTag("GameController").GetComponent<TilesToArray>();
        mouseController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MouseController>();
        flowController = GameObject.FindGameObjectWithTag("GameController").GetComponent<FlowControl>();
        unitMenuControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitMenuControl>();
        unitSpawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitSpawner>();
		movementTypes = GameObject.FindGameObjectWithTag("GameController").GetComponent<MovementTypes>();
		unitArray = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitArray>();
        allWeapons = GameObject.FindGameObjectWithTag("GameController").GetComponent<AllWeapons>();
        economyController = GameObject.FindGameObjectWithTag("GameController").GetComponent<EconomyController>();
        buildingLocations = GameObject.FindGameObjectWithTag("GameController").GetComponent<BuildingLocations>();
        UIcontroller = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>();
		baseSpawningArea = GameObject.FindGameObjectWithTag("GameController").GetComponent<BaseSpawningArea>();
		tileSpreadingManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TileSpreadingManager>();
		preDefinedActionTileSpawning = GameObject.FindGameObjectWithTag("GameController").GetComponent<PreDefinedActionTileSpawning>();
        tryingToSpawnAUnit = GameObject.FindGameObjectWithTag("GameController").GetComponent<TryingToSpawnAUnit>();
        tryingToGiveAUnitAWeapon = GameObject.FindGameObjectWithTag("GameController").GetComponent<TryingToGiveAUnitAWeapon>();
        unitBattleResult = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitBattleResult>();
    }
}
