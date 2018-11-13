using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    public Weapon currentWeapon;
	public Weapon[] Inventory;

    public void AddInventory(string weaponName)
	{
        for (int i = 0; i < Inventory.Length; i++)
        {
            if(Inventory[i] == null)
            {
                Inventory[i] = ScriptLink.allWeapons.weapons[weaponName];
                break;
            }
        }
	}

    public void EquipWeapon(string weaponName)
    {
        foreach (Weapon weapon in Inventory)
        {
            if(weapon == ScriptLink.allWeapons.weapons[weaponName])
            {
                currentWeapon = ScriptLink.allWeapons.weapons[weaponName];
            }
        }
    }
}
