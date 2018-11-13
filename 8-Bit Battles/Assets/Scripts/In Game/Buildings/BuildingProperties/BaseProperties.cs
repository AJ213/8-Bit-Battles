using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseProperties : BuildingProperties
{
    public void RunOpenOrCloseShop()
    {
        ScriptLink.UIcontroller.ToggleShop();
    }

    void Update()
    {
        if(ScriptLink.tryingToSpawnAUnit.selectingUnitSpawnLocation == true || ScriptLink.tryingToGiveAUnitAWeapon.givingAUnitAWeapon == true)
        {
            this.gameObject.GetComponent<Button>().interactable = false;
        }
        else if (ScriptLink.UIcontroller.shop.activeSelf == true)
        {
            this.gameObject.GetComponent<Button>().interactable = false;
        }
        else if(ScriptLink.mouseController.SelectedUnit != null)
        {
            this.gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            this.gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
