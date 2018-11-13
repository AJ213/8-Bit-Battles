using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMenuControl : MonoBehaviour
{
    [SerializeField] Text unitNameText;
    [SerializeField] Text HPText;
    [SerializeField] Text atkText;
    [SerializeField] Text spdText;
    [SerializeField] Text defText;
    [SerializeField] Text resText;
    [SerializeField] Text equipedWeapon;

    public GameObject unitInventory;

    public GameObject[] weaponDisplay;

    public Text[] weaponText;

    void Start()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>().unitStats.SetActive(false);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>().unitMenu.SetActive(false);
    }

    public void DisplayUnitStats(GameObject unit)
    {
        if(ScriptLink.UIcontroller.shop.activeSelf == false)
        {
            ScriptLink.UIcontroller.unitStats.SetActive(true);

            unitNameText.text = unit.GetComponent<UnitStats>().UnitIdentity.ToString();
            HPText.text = unit.GetComponent<UnitStats>().health.ToString() + " / " + unit.GetComponent<UnitStats>().maxHealth.ToString();
            atkText.text = unit.GetComponent<UnitStats>().attack.ToString();
            spdText.text = unit.GetComponent<UnitStats>().speed.ToString();
            defText.text = unit.GetComponent<UnitStats>().defense.ToString();
            resText.text = unit.GetComponent<UnitStats>().resistance.ToString();
            equipedWeapon.text = unit.GetComponent<UnitInventory>().currentWeapon.weaponName;
        }
    }

    public void DisplayUnitMenu()
    {
        ScriptLink.UIcontroller.unitMenu.SetActive(true);
    }

    public void HideUnitMenus()
    {
        ScriptLink.UIcontroller.unitStats.SetActive(false);
        ScriptLink.UIcontroller.unitMenu.SetActive(false);
    } 

    public void Wait()
    {
        ScriptLink.mouseController.SelectedUnit.GetComponent<Unit>().canAttack = false;
        ScriptLink.mouseController.SelectedUnit.GetComponent<Unit>().canMove = false;
        ScriptLink.mouseController.UnselectUnit();
        unitInventory.SetActive(false);
    }

    public void Cancel()
    {
        Unit unitScript = ScriptLink.mouseController.SelectedUnit.GetComponent<Unit>();
        if (unitScript.canMove == false && unitScript.canAttack != false || ScriptLink.mouseController.SelectedUnit.GetComponent<UnitStats>().UnitIdentity == UnitStats.UnitType.Artillery)
        {
            if(ScriptLink.mouseController.SelectedUnit.GetComponent<UnitStats>().UnitIdentity == UnitStats.UnitType.Artillery)
            {
                if (unitScript.canMove == false && unitScript.canAttack == false)
                {
                    ScriptLink.mouseController.UnselectUnit();
                    return;
                }
               
            }
            unitScript.transform.position = new Vector3(unitScript.oldLocation.x, unitScript.oldLocation.y, ScriptLink.mouseController.SelectedUnit.transform.position.z);
            unitScript.canMove = true;
            unitScript.canAttack = true;
            unitInventory.SetActive(false);
            ScriptLink.mouseController.UnselectUnit();
        }
        else
        {
            ScriptLink.mouseController.UnselectUnit();
        }
    }

    public void Items()
    {
        if (unitInventory.activeSelf == false)
        {
            unitInventory.SetActive(true);

            for (int i = 0; i < ScriptLink.mouseController.SelectedUnit.GetComponent<UnitInventory>().Inventory.Length; i++)
            {
                if (ScriptLink.mouseController.SelectedUnit.GetComponent<UnitInventory>().Inventory[i] != null)
                {
                    
                    weaponText[i].text = ScriptLink.mouseController.SelectedUnit.GetComponent<UnitInventory>().Inventory[i].weaponName;
                }
                else
                {
                    weaponText[i].text = "Empty Weapon Slot";
                }

                if (ScriptLink.mouseController.SelectedUnit.GetComponent<UnitInventory>().Inventory[i] != null)
                {
                    if(ScriptLink.mouseController.SelectedUnit.GetComponent<UnitInventory>().Inventory[i] == ScriptLink.mouseController.SelectedUnit.GetComponent<UnitInventory>().currentWeapon)
                    {
                        weaponDisplay[i].GetComponent<Button>().interactable = false;
                    }
                    else
                    {
                        weaponDisplay[i].GetComponent<Button>().interactable = true;
                    }
                }
            }
        }
        else
        {
            unitInventory.SetActive(false);
        }
    }

    public void EquipWeapon(int whichWeapon)
    {
        if(ScriptLink.mouseController.SelectedUnit.GetComponent<UnitInventory>().Inventory[whichWeapon] != null)
        {
            RemoveWeaponBuffs();
            ScriptLink.mouseController.SelectedUnit.GetComponent<UnitInventory>().EquipWeapon(ScriptLink.mouseController.SelectedUnit.GetComponent<UnitInventory>().Inventory[whichWeapon].weaponName);
            AddWeaponBuffs();
            HideUnitMenus();
            unitInventory.SetActive(false);
            ScriptLink.mouseController.SelectedUnit.GetComponent<Unit>().canAttack = false;
            ScriptLink.mouseController.SelectedUnit.GetComponent<Unit>().canMove = false;
            ScriptLink.mouseController.UnselectUnit();
        }
    }

    void AddWeaponBuffs()
    {
        Weapon weapon = ScriptLink.mouseController.SelectedUnit.GetComponent<UnitInventory>().currentWeapon;
        UnitStats unitStats = ScriptLink.mouseController.SelectedUnit.GetComponent<UnitStats>();

        unitStats.attack += weapon.weaponDamage;
        switch (weapon.WeaponBuffType)
        {
            case Weapon.WeaponBuff.Attack:
                unitStats.attack += weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.AttackDistanceMax:
                unitStats.maxAttackRange += weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.AttackDistanceMaxAndMin:
                unitStats.maxAttackRange += weapon.weaponBuffAmount;
                unitStats.minAttackRange += weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.AttackDistanceMin:
                unitStats.minAttackRange += weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.Defense:
                unitStats.defense += weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.Health:
                unitStats.maxHealth += weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.MovementDistance:
                unitStats.movementDistance += weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.Resistance:
                unitStats.resistance += weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.Speed:
                unitStats.speed += weapon.weaponBuffAmount;
                break;
        }
    }
    void RemoveWeaponBuffs()
    {
        Weapon weapon = ScriptLink.mouseController.SelectedUnit.GetComponent<UnitInventory>().currentWeapon;
        UnitStats unitStats = ScriptLink.mouseController.SelectedUnit.GetComponent<UnitStats>();

        unitStats.attack -= weapon.weaponDamage;
        switch (weapon.WeaponBuffType)
        {
            case Weapon.WeaponBuff.Attack:
                unitStats.attack -= weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.AttackDistanceMax:
                unitStats.maxAttackRange -= weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.AttackDistanceMaxAndMin:
                unitStats.maxAttackRange -= weapon.weaponBuffAmount;
                unitStats.minAttackRange -= weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.AttackDistanceMin:
                unitStats.minAttackRange -= weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.Defense:
                unitStats.defense -= weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.Health:
                unitStats.maxHealth -= weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.MovementDistance:
                unitStats.movementDistance -= weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.Resistance:
                unitStats.resistance -= weapon.weaponBuffAmount;
                break;

            case Weapon.WeaponBuff.Speed:
                unitStats.speed -= weapon.weaponBuffAmount;
                break;
        }
    }
}
