using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	[SerializeField] LayerMask unitLayerMask;

	bool hoveredAEnemyUnit = false;
    public bool artilleryCanMove = true;
	public bool canMove = true;
	public bool canAttack = true;

    public Vector2 oldLocation;

	void Start()
	{
		FlowControl.OnTurnChange += OnTurnChange;
	}
	void OnDisable()
	{
		FlowControl.OnTurnChange -= OnTurnChange;
	}
	void OnTurnChange()
	{
		LightenUnit ();
	}

	public void LightenUnit()
	{
		this.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	}
	public void DarkenUnit()
	{
		this.GetComponent<SpriteRenderer> ().color = new Color (0.3f, 0.3f, 0.3f, 1.0f);
	}

    void OnMouseOver()
    {
        if (ScriptLink.mouseController.SelectedUnit == null)
        {
            ScriptLink.unitMenuControl.DisplayUnitStats(this.gameObject);
        }

		if (hoveredAEnemyUnit == false && ScriptLink.mouseController.busyWithAllyUnit == false)
		{
			ScriptLink.unitArray.UpdateUnitArray ();
			if (ScriptLink.unitArray.allUnits [(int)(Mathf.Round (ScriptLink.mouseController.ExactMouseLocation.x - 0.5f)), (int)(Mathf.Round (ScriptLink.mouseController.ExactMouseLocation.y - 0.5f))] != null)
			{
				if (ScriptLink.unitArray.allUnits [(int)(Mathf.Round (ScriptLink.mouseController.ExactMouseLocation.x - 0.5f)), (int)(Mathf.Round (ScriptLink.mouseController.ExactMouseLocation.y - 0.5f))].GetComponent<UnitStats> ().isRed != ScriptLink.flowController.IsRedTurn)
				{
					hoveredAEnemyUnit = true;
					ScriptLink.mouseController.SelectedUnit = ScriptLink.unitArray.allUnits [(int)(Mathf.Round (ScriptLink.mouseController.ExactMouseLocation.x - 0.5f)), (int)(Mathf.Round (ScriptLink.mouseController.ExactMouseLocation.y - 0.5f))];
					ScriptLink.preDefinedActionTileSpawning.MovementTilesWithAttackRange ();
				}
			}
		}
        
    }
    void OnMouseEnter()
    {
        if (ScriptLink.mouseController.SelectedUnit != null && ScriptLink.mouseController.SelectedUnit.GetComponent<Unit>().canMove == false)
        {
            if (this.gameObject != ScriptLink.mouseController.SelectedUnit)
            {
                ScriptLink.UIcontroller.ToggleUnitComparasion();
                ScriptLink.mouseController.EnemyUnit = this.gameObject;
                ScriptLink.unitBattleResult.ShowBattleResults(ScriptLink.mouseController.SelectedUnit, this.gameObject);
            }
        }

    }
    void OnMouseExit()
    {
		if (hoveredAEnemyUnit == true)
		{
			ScriptLink.mouseController.SelectedUnit = null;
			ScriptLink.tileSpreadingManager.ClearActionTiles ();
			hoveredAEnemyUnit = false;
		}
        ScriptLink.UIcontroller.unitComparasion.SetActive(false);
        if (ScriptLink.mouseController.SelectedUnit == null)
        {
            ScriptLink.unitMenuControl.HideUnitMenus();
        }
    }

    public void LocationSelected ()
	{
		if (canMove && canAttack) 
		{
            ScriptLink.mouseController.SelectionBox.GetComponent<SpriteRenderer>().sprite = ScriptLink.mouseController.SelectionBoxRed;
            oldLocation = new Vector2(transform.position.x, transform.position.y);
			this.transform.position = new Vector3 (ScriptLink.mouseController.UnitSelectionLocation.x + 0.5f, ScriptLink.mouseController.UnitSelectionLocation.y + 0.5f, -5f);
			ScriptLink.unitArray.UpdateUnitArray ();
            canMove = false;
            ScriptLink.unitMenuControl.DisplayUnitMenu();
            
            
            if (canAttack == true)
            {
                ScriptLink.preDefinedActionTileSpawning.Attacking();
                ScriptLink.unitMenuControl.DisplayUnitStats(ScriptLink.mouseController.SelectedUnit);
                SpecialCaseWithArtillery();
            }
		}
	}

    void SpecialCaseWithArtillery()
    {
        if(ScriptLink.mouseController.SelectedUnit.GetComponent<UnitStats>().UnitIdentity == UnitStats.UnitType.Artillery)
        {
            if(oldLocation != new Vector2(transform.position.x, transform.position.y))
            {
                artilleryCanMove = false;
            }
        }
    }

    
}
