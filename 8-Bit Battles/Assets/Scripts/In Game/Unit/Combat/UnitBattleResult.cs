using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnitBattleResult : MonoBehaviour
{
    public Text redUnitName;
    public Text greenUnitName;

    public Text redHP;
    public Text greenHP;
    public Text redATK;
    public Text greenATK;
    public Text redSPD;
    public Text greenSPD;
    public Text redDEF;
    public Text greenDEF;
    public Text redRES;
    public Text greenRES;

    UnitAttacking unitAttacking;

    public GameObject TempRedUnit;
    public GameObject TempGreenUnit;

    public UnitStats redUnitsStats;
    public UnitStats greenUnitStats;

    public void ShowBattleResults(GameObject attacker, GameObject defender)
    {
        unitAttacking = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitAttacking>();
        SetTempUnitStatsToReal(attacker, defender);
        if (ScriptLink.flowController.IsRedTurn)
        {
            unitAttacking.attackingUnit = TempRedUnit;
            unitAttacking.defendingUnit = TempGreenUnit;
        }
        else
        {
            unitAttacking.attackingUnit = TempGreenUnit;
            unitAttacking.defendingUnit = TempRedUnit;
        }
        unitAttacking.CalculateFightResult();
        UpdateTextElements();
    }

    void SetTempUnitStatsToReal(GameObject attacker, GameObject defender)
    {
        //redUnitsStats = attacker.GetComponent<UnitStats>();
        if (ScriptLink.flowController.IsRedTurn) //attacker is red
        {
            redUnitsStats.UnitIdentity = attacker.GetComponent<UnitStats>().UnitIdentity;
            redUnitsStats.health = attacker.GetComponent<UnitStats>().health;
            redUnitsStats.maxHealth = attacker.GetComponent<UnitStats>().maxHealth;
            redUnitsStats.attack = attacker.GetComponent<UnitStats>().attack;
            redUnitsStats.speed = attacker.GetComponent<UnitStats>().speed;
            redUnitsStats.defense = attacker.GetComponent<UnitStats>().defense;
            redUnitsStats.resistance = attacker.GetComponent<UnitStats>().resistance;
            redUnitsStats.minAttackRange = attacker.GetComponent<UnitStats>().minAttackRange;
            redUnitsStats.maxAttackRange = attacker.GetComponent<UnitStats>().maxAttackRange;
            redUnitsStats.isPhysicalDamage = attacker.GetComponent<UnitStats>().isPhysicalDamage;

            TempRedUnit.GetComponent<UnitInventory>().currentWeapon = attacker.GetComponent<UnitInventory>().currentWeapon;
        }
        else //defender is red
        {
            redUnitsStats.UnitIdentity = defender.GetComponent<UnitStats>().UnitIdentity;
            redUnitsStats.health = defender.GetComponent<UnitStats>().health;
            redUnitsStats.maxHealth = defender.GetComponent<UnitStats>().maxHealth;
            redUnitsStats.attack = defender.GetComponent<UnitStats>().attack;
            redUnitsStats.speed = defender.GetComponent<UnitStats>().speed;
            redUnitsStats.defense = defender.GetComponent<UnitStats>().defense;
            redUnitsStats.resistance = defender.GetComponent<UnitStats>().resistance;
            redUnitsStats.minAttackRange = defender.GetComponent<UnitStats>().minAttackRange;
            redUnitsStats.maxAttackRange = defender.GetComponent<UnitStats>().maxAttackRange;
            redUnitsStats.isPhysicalDamage = defender.GetComponent<UnitStats>().isPhysicalDamage;

            TempRedUnit.GetComponent<UnitInventory>().currentWeapon = defender.GetComponent<UnitInventory>().currentWeapon;
        }

        if (ScriptLink.flowController.IsRedTurn) //defender is green
        {
            greenUnitStats.UnitIdentity = defender.GetComponent<UnitStats>().UnitIdentity;
            greenUnitStats.health = defender.GetComponent<UnitStats>().health;
            greenUnitStats.maxHealth = defender.GetComponent<UnitStats>().maxHealth;
            greenUnitStats.attack = defender.GetComponent<UnitStats>().attack;
            greenUnitStats.speed = defender.GetComponent<UnitStats>().speed;
            greenUnitStats.defense = defender.GetComponent<UnitStats>().defense;
            greenUnitStats.resistance = defender.GetComponent<UnitStats>().resistance;
            greenUnitStats.minAttackRange = defender.GetComponent<UnitStats>().minAttackRange;
            greenUnitStats.maxAttackRange = defender.GetComponent<UnitStats>().maxAttackRange;
            greenUnitStats.isPhysicalDamage = defender.GetComponent<UnitStats>().isPhysicalDamage;

            TempGreenUnit.GetComponent<UnitInventory>().currentWeapon = defender.GetComponent<UnitInventory>().currentWeapon;
        }
        else //attacker is green
        {
            greenUnitStats.UnitIdentity = attacker.GetComponent<UnitStats>().UnitIdentity;
            greenUnitStats.health = attacker.GetComponent<UnitStats>().health;
            greenUnitStats.maxHealth = attacker.GetComponent<UnitStats>().maxHealth;
            greenUnitStats.attack = attacker.GetComponent<UnitStats>().attack;
            greenUnitStats.speed = attacker.GetComponent<UnitStats>().speed;
            greenUnitStats.defense = attacker.GetComponent<UnitStats>().defense;
            greenUnitStats.resistance = attacker.GetComponent<UnitStats>().resistance;
            greenUnitStats.minAttackRange = attacker.GetComponent<UnitStats>().minAttackRange;
            greenUnitStats.maxAttackRange = attacker.GetComponent<UnitStats>().maxAttackRange;
            greenUnitStats.isPhysicalDamage = attacker.GetComponent<UnitStats>().isPhysicalDamage;

            TempGreenUnit.GetComponent<UnitInventory>().currentWeapon = attacker.GetComponent<UnitInventory>().currentWeapon;
        }
    }

    void UpdateTextElements()
    {
        RedUnit();
        GreenUnit();
    }

    void RedUnit()
    {
        if (ScriptLink.flowController.IsRedTurn) //attacker is red
        {
            redUnitName.text = unitAttacking.attackingUnitStats.UnitIdentity.ToString();
            redHP.text = (unitAttacking.attackingUnitStats.health.ToString() + "/" + unitAttacking.attackingUnitStats.maxHealth.ToString());
            redATK.text = unitAttacking.attackingUnitStats.attack.ToString();
            redSPD.text = unitAttacking.attackingUnitStats.speed.ToString();
            redDEF.text = unitAttacking.attackingUnitStats.defense.ToString();
            redRES.text = unitAttacking.attackingUnitStats.resistance.ToString();
        }
        else //defender is red
        {
            redUnitName.text = unitAttacking.defendingUnitStats.UnitIdentity.ToString();
            redHP.text = (unitAttacking.defendingUnitStats.health.ToString() + "/" + unitAttacking.defendingUnitStats.maxHealth.ToString());
            redATK.text = unitAttacking.defendingUnitStats.attack.ToString();
            redSPD.text = unitAttacking.defendingUnitStats.speed.ToString();
            redDEF.text = unitAttacking.defendingUnitStats.defense.ToString();
            redRES.text = unitAttacking.defendingUnitStats.resistance.ToString();
        }
    }
    void GreenUnit()
    {
        if (ScriptLink.flowController.IsRedTurn) //defender is green
        {
            greenUnitName.text = unitAttacking.defendingUnitStats.UnitIdentity.ToString();
            greenHP.text = (unitAttacking.defendingUnitStats.health.ToString() + "/" + unitAttacking.defendingUnitStats.maxHealth.ToString());
            greenATK.text = unitAttacking.defendingUnitStats.attack.ToString();
            greenSPD.text = unitAttacking.defendingUnitStats.speed.ToString();
            greenDEF.text = unitAttacking.defendingUnitStats.defense.ToString();
            greenRES.text = unitAttacking.defendingUnitStats.resistance.ToString();
        }
        else //attacker is green
        {
            greenUnitName.text = unitAttacking.attackingUnitStats.UnitIdentity.ToString();
            greenHP.text = (unitAttacking.attackingUnitStats.health.ToString() + "/" + unitAttacking.attackingUnitStats.maxHealth.ToString());
            greenATK.text = unitAttacking.attackingUnitStats.attack.ToString();
            greenSPD.text = unitAttacking.attackingUnitStats.speed.ToString();
            greenDEF.text = unitAttacking.attackingUnitStats.defense.ToString();
            greenRES.text = unitAttacking.attackingUnitStats.resistance.ToString();
        }
    }
}
