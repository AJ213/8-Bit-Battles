using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public ShopItem[] buyUnits;
    public ShopItem[] buyWeapons;

    public GameObject BuyUnitButton;
    public GameObject BuyWeaponButton;

    [SerializeField] int currentUnit = 0;
    [SerializeField] int currentWeapon = 0;

    int ChangeInt(int num, int upOrDown)
    {
        if(Mathf.Abs(upOrDown) != 1)
        {
            throw new System.ArgumentException("Input for the ChangeInt fuction can only be a value of 1 or -1");
        }
        return num+= (1 * upOrDown);
    }

    public void ChangeUnit(int upOrDown)
    {
        currentUnit = ChangeInt(currentUnit, upOrDown);
    }
    public void ChangeWeapon(int upOrDown)
    {
        currentWeapon = ChangeInt(currentWeapon, upOrDown);
        
    }
    
    void SetShopItemToNum()
    {
        if(currentUnit > -1 && currentWeapon > -1 && currentUnit < buyUnits.Length && currentWeapon < buyWeapons.Length)
        {
            BuyUnitButton.GetComponent<BuyItem>().item = buyUnits[currentUnit];
            BuyWeaponButton.GetComponent<BuyItem>().item = buyWeapons[currentWeapon];
        }
    }

    void Update()
    {
        currentUnit = SetNumberLimit(currentUnit, 0, 16, 17, 33);
        currentWeapon = SetNumberLimit(currentWeapon, 0, 22);
        SetShopItemToNum();
    }

    int SetNumberLimit(int num, int minRedTeam, int maxRedTeam, int minGreenTeam, int maxGreenTeam) //Isn't this cool? Both functions have the same name yet this still works.
    {
        if (ScriptLink.flowController.IsRedTurn == true)
        {
            num = num > maxRedTeam ? num = minRedTeam : num;
            num = num < minRedTeam ? num = maxRedTeam : num;
        }
        else
        {
            num = num > maxGreenTeam ? num = minGreenTeam : num;
            num = num < minGreenTeam ? num = maxGreenTeam : num;
        }
        return num;
    }
    int SetNumberLimit(int num, int min, int max)
    {
        num = num > max ? num = min : num;
        num = num < min ? num = max : num;
        return num;
    }
}
