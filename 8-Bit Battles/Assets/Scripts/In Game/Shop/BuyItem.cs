using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    public ShopItem item;

    public GameObject itemSprite;

    public Text infoOfItem;

    public new Text name;
    public Text cost;
    string costString;

    void Update()
    {
        if (item != null)
        {
            name.text = item.name;
            costString = ("Cost: " + item.cost);
            cost.text = costString;
            itemSprite.GetComponent<Image>().sprite = item.artwork;
            infoOfItem.text = item.description;
        }
    }

    public void BuyAItem()
    {
        if (item.isUnit)
        {
            BuyUnit(item.unitIndex, item.cost);
        }
        else
        {
            BuyWeapon(item.name, item.cost);
        }
    }
    public void BuyUnit(int index, int cost)
    {
        bool redTeamHasFinance = cost <= ScriptLink.economyController.redCash && ScriptLink.flowController.IsRedTurn == true;
        bool greenTeamHasFinace = cost <= ScriptLink.economyController.greenCash && ScriptLink.flowController.IsRedTurn == false;

        if (greenTeamHasFinace || redTeamHasFinance)
        {
            ScriptLink.tryingToSpawnAUnit.costOfCurrentUnit = cost;
            ScriptLink.tryingToSpawnAUnit.indexOfCurrentUnit = index;
			ScriptLink.movementTypes.index = index;
            ChooseUnitSpawnLocation();
        }
        else
        {
            return;
        }
    }
    public void BuyWeapon(string name, int cost)
    {
        bool redTeamHasFinance = cost <= ScriptLink.economyController.redCash && ScriptLink.flowController.IsRedTurn == true;
        bool greenTeamHasFinace = cost <= ScriptLink.economyController.greenCash && ScriptLink.flowController.IsRedTurn == false;

        if (greenTeamHasFinace || redTeamHasFinance)
        {
            ScriptLink.tryingToGiveAUnitAWeapon.costOfWeapon = cost;
            ScriptLink.tryingToGiveAUnitAWeapon.weaponName = name;
            ChooseWeaponGivingLocation();
        }
        else
        {
            return;
        }
    }

    void ChooseWeaponGivingLocation()
    {
        ScriptLink.tryingToGiveAUnitAWeapon.givingAUnitAWeapon = true;
        ScriptLink.mouseController.SelectionBox.GetComponent<SpriteRenderer>().sprite = ScriptLink.mouseController.SelectionBoxGreen;
		ScriptLink.mouseController.busyWithAllyUnit = true;
		ScriptLink.preDefinedActionTileSpawning.WeaponSpawning(item.name);
        ScriptLink.UIcontroller.ToggleAllUI();
    }
    void ChooseUnitSpawnLocation()
    {
        ScriptLink.tryingToSpawnAUnit.selectingUnitSpawnLocation = true;
        ScriptLink.mouseController.SelectionBox.GetComponent<SpriteRenderer>().sprite = ScriptLink.mouseController.SelectionBoxGreen;
		ScriptLink.mouseController.busyWithAllyUnit = true;
        ScriptLink.preDefinedActionTileSpawning.SpawnUnitFromBase();
        ScriptLink.UIcontroller.ToggleAllUI();
    }


}
