using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    // Fields (Variables)
    #region
    public bool busyWithAllyUnit = false;

    Vector2 unitSelectionLocation;
    Vector2 exactMouseLocation;
    Vector2 mouseLocation;

    [SerializeField] LayerMask activeTileLayerMask;
    [SerializeField] LayerMask unitLayerMask;

    [SerializeField] GameObject selectedUnit;
    GameObject enemyUnit;

    [SerializeField] GameObject selectionBox;

    [SerializeField] Sprite selectionBoxYellow;
    [SerializeField] Sprite selectionBoxBlue;
    [SerializeField] Sprite selectionBoxGreen;
    [SerializeField] Sprite selectionBoxRed;
    #endregion
    // Properties (Get/Set)
    #region
    public Vector2 UnitSelectionLocation { get { return this.unitSelectionLocation; } }
    public Vector2 ExactMouseLocation { get { return this.exactMouseLocation; } }
    public Vector2 MouseLocation { get { return this.mouseLocation; } }

    public GameObject SelectedUnit { get { return this.selectedUnit; } set { this.selectedUnit = value; } }
    public GameObject EnemyUnit { get { return this.enemyUnit; } set { this.enemyUnit = value; } }

    public GameObject SelectionBox { get { return this.selectionBox; } }

    public Sprite SelectionBoxYellow { get { return this.selectionBoxYellow; } }
    public Sprite SelectionBoxBlue { get { return this.selectionBoxBlue; } }
    public Sprite SelectionBoxGreen { get { return this.selectionBoxGreen; } }
    public Sprite SelectionBoxRed { get { return this.selectionBoxRed; } }

    /* This is an example of encapsilation Jacob. 
     * I read the value of mouseLocation by using the get and set part of this function to get/set the value of mouseLocation to something else by using the set function all without needing to make mouseLocation public.
     * In this case I only need to read the value of mouseLocation so I commented out the set function.
     * Normally you would just delete the set part of the function but in this case I wanted to show an example of a get/set function like this.
     * Also, in the set part of the function. You can set stuff inside like this:
     *   set 
     *   { 
     *       num++;
     *       if(num > 1)
     *       {
     *          num = value * 2;
     *       } 
     *   }
     * This is part of the reason why we use get/set.
     * If you understand this then please delete this whole comment. Thanks. (If you don't we can talk about it!)
    */
    #endregion
    // Events
    #region
    void Update()
    {
        UserMouseInput();
    }
    #endregion
    // Methods
    #region
    void UserMouseInput()
    {
        exactMouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);                                        // Gets mouse location
        mouseLocation = new Vector2(Mathf.Floor(exactMouseLocation.x) + 0.5f, Mathf.Floor(exactMouseLocation.y) + 0.5f); // Rounds mouse location
        MoveSelectionBox();
        if (Input.GetButtonDown("Fire1"))
        {
            UnitSelectionOptions();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            busyWithAllyUnit = false;
            if (selectedUnit != null)
            {
                ScriptLink.unitMenuControl.Cancel();
            }
        }
    }
    
    void CanWeAttack()
    {
        RaycastHit UnitHit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out UnitHit, 1000.0f, unitLayerMask))
        {
            enemyUnit = UnitHit.collider.gameObject;
            if (EnemyUnit.GetComponent<UnitStats>().isRed != ScriptLink.flowController.IsRedTurn)
            {
                if (ScriptLink.tileSpreadingManager.actionTiles[(int)mouseLocation.x, (int)mouseLocation.y] != null)
                {
                    if (ScriptLink.tileSpreadingManager.actionTiles[(int)mouseLocation.x, (int)mouseLocation.y].GetComponent<ActionTileProperties>().actionType == ActionTileProperties.ActionType.Attack_Valid)
                    {
                        if (ScriptLink.tileSpreadingManager.actionTiles[(int)mouseLocation.x, (int)mouseLocation.y].transform.position.x == EnemyUnit.transform.position.x)
                        {
                            if (ScriptLink.tileSpreadingManager.actionTiles[(int)mouseLocation.x, (int)mouseLocation.y].transform.position.y == EnemyUnit.transform.position.y)
                            {
                                SelectedUnit.GetComponent<UnitAttacking>().Attack();
                            }
                        }
                    }
                }
            }
            else
            {
                if (ScriptLink.tileSpreadingManager.actionTiles[(int)mouseLocation.x, (int)mouseLocation.y] != null)
                {
                    if (ScriptLink.tileSpreadingManager.actionTiles[(int)mouseLocation.x, (int)mouseLocation.y].GetComponent<ActionTileProperties>().actionType == ActionTileProperties.ActionType.Assist_Valid)
                    {
                        if (ScriptLink.tileSpreadingManager.actionTiles[(int)mouseLocation.x, (int)mouseLocation.y].transform.position.x == EnemyUnit.transform.position.x)
                        {
                            if (ScriptLink.tileSpreadingManager.actionTiles[(int)mouseLocation.x, (int)mouseLocation.y].transform.position.y == EnemyUnit.transform.position.y)
                            {
                                SelectedUnit.GetComponent<UnitAssisting>().Assist();
                            }
                        }
                    }
                }
            }
        }
    }
    void MoveSelectionBox()
    {
        SelectionBox.transform.position = new Vector3(mouseLocation.x, mouseLocation.y, SelectionBox.transform.position.z);
    }
    void LocationSelection()
    {
        TileSelection();
    }

    void TileSelection()
    {
        if ((exactMouseLocation.x > 0 && exactMouseLocation.x < ScriptLink.tilesToArray.mapXLimit) && (exactMouseLocation.y > 0 && exactMouseLocation.y < ScriptLink.tilesToArray.mapYLimit))
        {
            unitSelectionLocation = new Vector2(Mathf.FloorToInt(exactMouseLocation.x), Mathf.FloorToInt(exactMouseLocation.y));
            MovementTileCheck();
        }
    }
    void MovementTileCheck()
    {
        if (ScriptLink.tileSpreadingManager.actionTiles[(int)unitSelectionLocation.x, (int)unitSelectionLocation.y] != null)
        {
            if (ScriptLink.tileSpreadingManager.actionTiles[(int)unitSelectionLocation.x, (int)unitSelectionLocation.y].GetComponent<ActionTileProperties>().actionType == ActionTileProperties.ActionType.Movement_Valid)
            {
                SelectedUnit.GetComponent<Unit>().LocationSelected();
            }
        }
    }

    void UnitSelectionOptions()
    {
        bool unitNull = SelectedUnit == null;

        if (unitNull && ScriptLink.UIcontroller.gameStatsBar.activeSelf == true) //We are selecting
        {
            UnitSelection();
        }
        else if (unitNull == false && SelectedUnit.GetComponent<Unit>().canMove == false && SelectedUnit.GetComponent<Unit>().canAttack == true) //We are attacking
        {
            if (mouseLocation == new Vector2(SelectedUnit.transform.position.x, SelectedUnit.transform.position.y))
            {
                UnselectUnit();
            }
            if (SelectedUnit != null && SelectedUnit.GetComponent<UnitStats>().UnitIdentity == UnitStats.UnitType.Artillery && SelectedUnit.GetComponent<Unit>().artilleryCanMove == false)
            {
                SelectedUnit.GetComponent<Unit>().artilleryCanMove = true;
                UnselectUnit();
            }
            CanWeAttack();
        }
        else if (unitNull == false && SelectedUnit.gameObject.GetComponent<UnitStats>().isRed == ScriptLink.flowController.IsRedTurn) //We are moving
        {
            LocationSelection();
        }
    }
    void UnitSelection()
    {
        RaycastHit unitHit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out unitHit, 1000.0f, unitLayerMask))
        {
            if (selectedUnit == null)
            {
                selectedUnit = unitHit.collider.gameObject;
                if (SelectedUnit.gameObject.GetComponent<UnitStats>().isRed == ScriptLink.flowController.IsRedTurn)
                {
                    ScriptLink.unitMenuControl.HideUnitMenus();

                    ScriptLink.preDefinedActionTileSpawning.MovementTilesWithAttackRange();
                    busyWithAllyUnit = true;

                    SelectionBox.GetComponent<SpriteRenderer>().sprite = selectionBoxBlue;
                }
                else
                {
                    selectedUnit = null;
                }

            }
            else
            {
                GameObject oldSelection = SelectedUnit;
                selectedUnit = unitHit.collider.gameObject;
                if (oldSelection.gameObject.GetComponent<UnitStats>().isRed != ScriptLink.flowController.IsRedTurn) //This is keeping sure that we can't select enemy pieces
                {
                    selectedUnit = oldSelection;
                }
            }
        }
    }
    public void UnselectUnit()
    {
        busyWithAllyUnit = false;
        if (selectedUnit.GetComponent<Unit>().canMove == false && SelectedUnit.GetComponent<Unit>().canAttack == true) //We are attacking
        {
            selectedUnit.GetComponent<Unit>().canAttack = false;
            ScriptLink.unitMenuControl.unitInventory.SetActive(false);
        }
        if (selectedUnit.GetComponent<Unit>().canMove == false && SelectedUnit.GetComponent<Unit>().canAttack == false) //Unit done moving
        {
            ScriptLink.mouseController.SelectedUnit.GetComponent<Unit>().DarkenUnit();
        }
        if (selectedUnit != null) //We are moving
        {
            selectedUnit = null;
            ScriptLink.unitMenuControl.HideUnitMenus();
            ScriptLink.tileSpreadingManager.ClearActionTiles();
            SelectionBox.GetComponent<SpriteRenderer>().sprite = SelectionBoxYellow;
        }
    }
    #endregion
}