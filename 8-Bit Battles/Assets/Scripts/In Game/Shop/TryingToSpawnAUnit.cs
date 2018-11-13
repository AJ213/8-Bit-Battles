using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryingToSpawnAUnit : MonoBehaviour
{
    public bool selectingUnitSpawnLocation;

    public int costOfCurrentUnit;
    public int indexOfCurrentUnit;

    void Update()
    {
        if (selectingUnitSpawnLocation)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (selectingUnitSpawnLocation)
                {
                    foreach (GameObject tile in ScriptLink.tileSpreadingManager.actionTiles)
                    {
                        if (tile != null && ScriptLink.mouseController.MouseLocation == new Vector2(tile.transform.position.x, tile.transform.position.y))
                        {
                            if (ScriptLink.flowController.IsRedTurn)
                            {
                                ScriptLink.economyController.redCash -= costOfCurrentUnit;
                            }
                            else
                            {
                                ScriptLink.economyController.greenCash -= costOfCurrentUnit;
                            }
                            ScriptLink.unitSpawner.SpawnUnit(indexOfCurrentUnit, tile.transform.position.x - 0.5f, tile.transform.position.y - 0.5f);
                            ScriptLink.tileSpreadingManager.ClearActionTiles();
                            ScriptLink.economyController.UpdateMoneyText();
                            selectingUnitSpawnLocation = false;
                            ScriptLink.UIcontroller.ToggleAllUI();
                            ScriptLink.UIcontroller.ToggleShop();
                            ScriptLink.mouseController.SelectionBox.GetComponent<SpriteRenderer>().sprite = ScriptLink.mouseController.SelectionBoxYellow;
                        }
                    }
                }
            }
            if (Input.GetButtonDown("Fire2"))
            {
                ScriptLink.tileSpreadingManager.ClearActionTiles();
                selectingUnitSpawnLocation = false;
                ScriptLink.UIcontroller.ToggleAllUI();
                ScriptLink.UIcontroller.ToggleShop();
                ScriptLink.mouseController.SelectionBox.GetComponent<SpriteRenderer>().sprite = ScriptLink.mouseController.SelectionBoxYellow;
            }
        }
    }
}
