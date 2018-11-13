using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryingToGiveAUnitAWeapon : MonoBehaviour {

    public bool givingAUnitAWeapon;

    public int costOfWeapon;
    public string weaponName;

    void Update()
    {
        if (givingAUnitAWeapon)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (givingAUnitAWeapon)
                {
					int x = (int)(ScriptLink.mouseController.MouseLocation.x - 0.5f);
					int y = (int)(ScriptLink.mouseController.MouseLocation.y - 0.5f);
					if (ScriptLink.tileSpreadingManager.actionTiles [x, y] != null)
					{
						if (ScriptLink.tileSpreadingManager.actionTiles [x, y].GetComponent<ActionTileProperties> ().actionType == ActionTileProperties.ActionType.Assist_Valid)
						{
							if (ScriptLink.flowController.IsRedTurn)
							{
								ScriptLink.economyController.redCash -= costOfWeapon;
							}
							else
							{
								ScriptLink.economyController.greenCash -= costOfWeapon;
							}
							ScriptLink.unitArray.allUnits [x, y].GetComponent<UnitInventory> ().AddInventory (weaponName);
							ScriptLink.tileSpreadingManager.ClearActionTiles ();
							ScriptLink.economyController.UpdateMoneyText ();
							givingAUnitAWeapon = false;
							ScriptLink.UIcontroller.ToggleShop ();
							ScriptLink.mouseController.SelectionBox.GetComponent<SpriteRenderer> ().sprite = ScriptLink.mouseController.SelectionBoxYellow;
						}
					}
                }
            }
            if (Input.GetButtonDown("Fire2"))
            {
                ScriptLink.tileSpreadingManager.ClearActionTiles();
                givingAUnitAWeapon = false;
                ScriptLink.UIcontroller.ToggleAllUI();
                ScriptLink.UIcontroller.ToggleShop();
                ScriptLink.mouseController.SelectionBox.GetComponent<SpriteRenderer>().sprite = ScriptLink.mouseController.SelectionBoxYellow;
            }
        }
    }
}
