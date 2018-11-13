using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllWeapons : MonoBehaviour 
{
    public Weapon[] theWeapons;
    public Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();
    public void MakeWeapons()
    {
        foreach(Weapon weapon in theWeapons)
        {
            weapons.Add(weapon.weaponName, weapon);
        }
    }
}
