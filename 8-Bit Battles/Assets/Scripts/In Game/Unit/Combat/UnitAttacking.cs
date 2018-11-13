using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttacking : MonoBehaviour
{
    bool distantDefenseActivatedDefending;

    public GameObject attackingUnit;
    public GameObject defendingUnit;

    public UnitStats attackingUnitStats;
    public UnitStats defendingUnitStats;

    public void Attack()
    {
        if (ScriptLink.mouseController.SelectedUnit.GetComponent<Unit>().canAttack == false)
            return;

        attackingUnit = ScriptLink.mouseController.SelectedUnit;
        defendingUnit = ScriptLink.mouseController.EnemyUnit;
        attackingUnitStats = attackingUnit.GetComponent<UnitStats>();
        defendingUnitStats = defendingUnit.GetComponent<UnitStats>();

        CalculateFightResult();
        RemoveDistantDefenseBuff();

        CleanUpDeadUnits();

        ScriptLink.mouseController.SelectedUnit.GetComponent<Unit>().canAttack = false;
        ScriptLink.mouseController.SelectedUnit.GetComponent<Unit>().canMove = false;
        ScriptLink.mouseController.UnselectUnit();
    }

    public void CalculateFightResult()
    {
        CalculateWeaponTriangle(); //Checks if attacking unit has weapon advantage also Checks if defending unit has weapon disadvantage

        CalculateEffectiveAgainst(); //Calculates effective against x for attacker

        HaveUnitsFight(); //Let the battles begin!

        UnCalculateEffectiveAgainst();
        UnCalculateEffectiveAgainst();
        RemoveDistantDefenseBuff();
    }

    void CalculateWeaponTriangle()
    {
        attackingUnitStats = attackingUnit.GetComponent<UnitStats>();
        defendingUnitStats = defendingUnit.GetComponent<UnitStats>();
        Weapon attackerCurrentEquipedWeapon = attackingUnit.GetComponent<UnitInventory>().currentWeapon;
        Weapon defenderCurrentEquipedWeapon = attackingUnit.GetComponent<UnitInventory>().currentWeapon;

        if (attackerCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Shotgun && defenderCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.MachineGun) //Attack in an advantage
        {
            attackingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) * 1.2f); //multiplies attacker damage by 20%
            defendingUnitStats.attack = Mathf.RoundToInt(((float)defendingUnitStats.attack) * 0.8f); //multiplies defender damage by -20%
        }
        else if (attackerCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.MachineGun && defenderCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Bazooka)
        {
            attackingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) * 1.2f); //multiplies attacker damage by 20%
            defendingUnitStats.attack = Mathf.RoundToInt(((float)defendingUnitStats.attack) * 0.8f); //multiplies defender damage by -20%
        }
        else if (attackerCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Bazooka && defenderCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Shotgun)
        {
            attackingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) * 1.2f); //multiplies attacker damage by 20%
            defendingUnitStats.attack = Mathf.RoundToInt(((float)defendingUnitStats.attack) * 0.8f); //multiplies defender damage by -20%
        }

        if (defenderCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Shotgun && attackerCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.MachineGun) //Attacker in a disadvantage
        {
            defendingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) * 1.2f); //multiplies defender damage by 20%
            attackingUnitStats.attack = Mathf.RoundToInt(((float)defendingUnitStats.attack) * 0.8f); //multiplies attacker damage by -20%
        }
        else if (defenderCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.MachineGun && attackerCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Bazooka)
        {
            defendingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) * 1.2f); //multiplies defender damage by 20%
            attackingUnitStats.attack = Mathf.RoundToInt(((float)defendingUnitStats.attack) * 0.8f); //multiplies attacker damage by -20%
        }
        else if (defenderCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Bazooka && attackerCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Shotgun)
        {
            defendingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) * 1.2f); //multiplies defender damage by 20%
            attackingUnitStats.attack = Mathf.RoundToInt(((float)defendingUnitStats.attack) * 0.8f); //multiplies attacker damage by -20%
        }
    }
    void CalculateEffectiveAgainst()
    {
        Weapon attackerCurrentEquipedWeapon = attackingUnit.GetComponent<UnitInventory>().currentWeapon;
        Weapon defenderCurrentEquipedWeapon = attackingUnit.GetComponent<UnitInventory>().currentWeapon;

        switch (attackerCurrentEquipedWeapon.WeaponEffectType) //Attacker weapon effectiveness check
        {
            case Weapon.WeaponEffect.EffectiveAgainstJuggernauts:
                if (defendingUnitStats.UnitIdentity == UnitStats.UnitType.Juggernaut)
                {
                    attackingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) * 1.5f);
                }
                break;
            case Weapon.WeaponEffect.EffectiveAgainstArtillery:
                if (defendingUnitStats.UnitIdentity == UnitStats.UnitType.Artillery)
                {
                    attackingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) * 1.5f);
                }
                break;
            case Weapon.WeaponEffect.EffectiveAgainstDrones:
                if (defendingUnitStats.UnitIdentity == UnitStats.UnitType.FighterPlane)
                {
                    attackingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) * 1.5f);
                }
                break;
            case Weapon.WeaponEffect.EffectiveAgainstATV:
                if (defendingUnitStats.UnitIdentity == UnitStats.UnitType.ATV)
                {
                    attackingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) * 1.5f);
                }
                break;
        }

        switch (defenderCurrentEquipedWeapon.WeaponEffectType) //Defender weapon effectiveness check
        {
            case Weapon.WeaponEffect.EffectiveAgainstJuggernauts:
                if (attackingUnitStats.UnitIdentity == UnitStats.UnitType.Juggernaut)
                {
                    defendingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) * 1.5f);
                }
                break;
            case Weapon.WeaponEffect.EffectiveAgainstArtillery:
                if (attackingUnitStats.UnitIdentity == UnitStats.UnitType.Artillery)
                {
                    defendingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) * 1.5f);
                }
                break;
            case Weapon.WeaponEffect.EffectiveAgainstDrones:
                if (attackingUnitStats.UnitIdentity == UnitStats.UnitType.FighterPlane)
                {
                    defendingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) * 1.5f);
                }
                break;
            case Weapon.WeaponEffect.EffectiveAgainstATV:
                if (attackingUnitStats.UnitIdentity == UnitStats.UnitType.ATV)
                {
                    defendingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) * 1.5f);
                }
                break;
        }
    }

    void UnCalculateEffectiveAgainst()
    {
        Weapon attackerCurrentEquipedWeapon = attackingUnit.GetComponent<UnitInventory>().currentWeapon;
        Weapon defenderCurrentEquipedWeapon = attackingUnit.GetComponent<UnitInventory>().currentWeapon;

        switch (attackerCurrentEquipedWeapon.WeaponEffectType) //Attacker weapon effectiveness check
        {
            case Weapon.WeaponEffect.EffectiveAgainstJuggernauts:
                if (defendingUnitStats.UnitIdentity == UnitStats.UnitType.Juggernaut)
                {
                    attackingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) / 1.5f);
                }
                break;
            case Weapon.WeaponEffect.EffectiveAgainstArtillery:
                if (defendingUnitStats.UnitIdentity == UnitStats.UnitType.Artillery)
                {
                    attackingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) / 1.5f);
                }
                break;
            case Weapon.WeaponEffect.EffectiveAgainstDrones:
                if (defendingUnitStats.UnitIdentity == UnitStats.UnitType.FighterPlane)
                {
                    attackingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) / 1.5f);
                }
                break;
            case Weapon.WeaponEffect.EffectiveAgainstATV:
                if (defendingUnitStats.UnitIdentity == UnitStats.UnitType.ATV)
                {
                    attackingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) / 1.5f);
                }
                break;
        }

        switch (defenderCurrentEquipedWeapon.WeaponEffectType) //Defender weapon effectiveness check
        {
            case Weapon.WeaponEffect.EffectiveAgainstJuggernauts:
                if (attackingUnitStats.UnitIdentity == UnitStats.UnitType.Juggernaut)
                {
                    defendingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) / 1.5f);
                }
                break;
            case Weapon.WeaponEffect.EffectiveAgainstArtillery:
                if (attackingUnitStats.UnitIdentity == UnitStats.UnitType.Artillery)
                {
                    defendingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) / 1.5f);
                }
                break;
            case Weapon.WeaponEffect.EffectiveAgainstDrones:
                if (attackingUnitStats.UnitIdentity == UnitStats.UnitType.FighterPlane)
                {
                    defendingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) / 1.5f);
                }
                break;
            case Weapon.WeaponEffect.EffectiveAgainstATV:
                if (attackingUnitStats.UnitIdentity == UnitStats.UnitType.ATV)
                {
                    defendingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) / 1.5f);
                }
                break;
        }
    }
    void UnCalculateWeaponTriangle()
    {
        attackingUnitStats = attackingUnit.GetComponent<UnitStats>();
        defendingUnitStats = defendingUnit.GetComponent<UnitStats>();
        Weapon attackerCurrentEquipedWeapon = attackingUnit.GetComponent<UnitInventory>().currentWeapon;
        Weapon defenderCurrentEquipedWeapon = attackingUnit.GetComponent<UnitInventory>().currentWeapon;

        if (attackerCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Shotgun && defenderCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.MachineGun) //Attack in an advantage
        {
            attackingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) / 1.2f); //multiplies attacker damage by 20%
            defendingUnitStats.attack = Mathf.RoundToInt(((float)defendingUnitStats.attack) / 0.8f); //multiplies defender damage by -20%
        }
        else if (attackerCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.MachineGun && defenderCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Bazooka)
        {
            attackingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) / 1.2f); //multiplies attacker damage by 20%
            defendingUnitStats.attack = Mathf.RoundToInt(((float)defendingUnitStats.attack) / 0.8f); //multiplies defender damage by -20%
        }
        else if (attackerCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Bazooka && defenderCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Shotgun)
        {
            attackingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) / 1.2f); //multiplies attacker damage by 20%
            defendingUnitStats.attack = Mathf.RoundToInt(((float)defendingUnitStats.attack) / 0.8f); //multiplies defender damage by -20%
        }

        if (defenderCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Shotgun && attackerCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.MachineGun) //Attacker in a disadvantage
        {
            defendingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) / 1.2f); //multiplies defender damage by 20%
            attackingUnitStats.attack = Mathf.RoundToInt(((float)defendingUnitStats.attack) / 0.8f); //multiplies attacker damage by -20%
        }
        else if (defenderCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.MachineGun && attackerCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Bazooka)
        {
            defendingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) / 1.2f); //multiplies defender damage by 20%
            attackingUnitStats.attack = Mathf.RoundToInt(((float)defendingUnitStats.attack) / 0.8f); //multiplies attacker damage by -20%
        }
        else if (defenderCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Bazooka && attackerCurrentEquipedWeapon.WeaponType == Weapon.WeaponTypes.Shotgun)
        {
            defendingUnitStats.attack = Mathf.RoundToInt(((float)attackingUnitStats.attack) / 1.2f); //multiplies defender damage by 20%
            attackingUnitStats.attack = Mathf.RoundToInt(((float)defendingUnitStats.attack) / 0.8f); //multiplies attacker damage by -20%
        }
    }

    void HaveUnitsFight()
    {
        AttackUnit(attackingUnit, defendingUnit);                                                                             //Initial attack
        if (attackingUnit.GetComponent<UnitInventory>().currentWeapon.WeaponEffectType == Weapon.WeaponEffect.Double)      //Attack again if weapon doubles enemies
            AttackUnit(attackingUnit, defendingUnit);

        if (IfUnitIsStillAlive(defendingUnit) == false)                                                                    //Defender Alive?
            return;

        if (isUnitInRange(attackingUnit, defendingUnit))                                                                    //checks if defender is in range to counter
        {
            AttackUnit(defendingUnit, attackingUnit);                                                                          //Counter attack
            if (defendingUnit.GetComponent<UnitInventory>().currentWeapon.WeaponEffectType == Weapon.WeaponEffect.Double)  //Attack again if weapon doubles enemies
                AttackUnit(defendingUnit, attackingUnit);

            if (IfUnitIsStillAlive(attackingUnit) == false)                                                                    //Attacker alive?
                return;
        }

        if (attackingUnitStats.speed - defendingUnitStats.speed >= 5)
        {
            AttackUnit(attackingUnit, defendingUnit);                                                                         //Second attack from attack
            if (attackingUnit.GetComponent<UnitInventory>().currentWeapon.WeaponEffectType == Weapon.WeaponEffect.Double) //Attack again if weapon doubles enemies
                AttackUnit(attackingUnit, defendingUnit);

            if (IfUnitIsStillAlive(defendingUnit) == false)                                                                    //Defender Alive?
                return;
        }
        else if (attackingUnitStats.speed - defendingUnitStats.speed >= 5)
        {
            if (isUnitInRange(attackingUnit, defendingUnit))                                                                    //checks if defender is in range to counter
            {
                AttackUnit(defendingUnit, attackingUnit);                                                                         //Second counter attack
                if (defendingUnit.GetComponent<UnitInventory>().currentWeapon.WeaponEffectType == Weapon.WeaponEffect.Double) //Attack again if weapon doubles enemies
                    AttackUnit(defendingUnit, attackingUnit);

                if (IfUnitIsStillAlive(attackingUnit) == false)                                                                    //Attacker alive?
                    return;
            }
        }
    }
    void AttackUnit(GameObject attackingUnit, GameObject defendingUnit)
    {
        UnitStats unitStatsAttacking = attackingUnit.GetComponent<UnitStats>();
        UnitStats unitStatsDefending = defendingUnit.GetComponent<UnitStats>();

        if (unitStatsAttacking.isPhysicalDamage)
        {
            if (unitStatsAttacking.attack - unitStatsDefending.defense >= 0)
            {
                unitStatsDefending.health -= unitStatsAttacking.attack - unitStatsDefending.defense;
            }
        }
        else
        {
            if (unitStatsAttacking.attack - unitStatsDefending.resistance >= 0)
            {
                unitStatsDefending.health -= unitStatsAttacking.attack - unitStatsDefending.resistance;
            }
        }
    }

    bool isUnitInRange(GameObject attackingUnit, GameObject defendingUnit)
    {
        Vector2 distanceBetweenUnitsVector2 = new Vector2(ScriptLink.mouseController.EnemyUnit.transform.position.x, ScriptLink.mouseController.EnemyUnit.transform.position.y) - new Vector2(ScriptLink.mouseController.SelectedUnit.transform.position.x, ScriptLink.mouseController.SelectedUnit.transform.position.y);
        int distanceBetweenUnits = Mathf.Abs((int)distanceBetweenUnitsVector2.x + (int)distanceBetweenUnitsVector2.y);
        if (ScriptLink.mouseController.EnemyUnit.GetComponent<UnitStats>().maxAttackRange >= distanceBetweenUnits && distanceBetweenUnits >= ScriptLink.mouseController.EnemyUnit.GetComponent<UnitStats>().minAttackRange) //If attacking unit is in range
        {
            CheckForDistantDefense(distanceBetweenUnitsVector2);
			Debug.Log("1+");
            return true;
        }
        else if (ScriptLink.mouseController.EnemyUnit.GetComponent<UnitInventory>().currentWeapon.WeaponEffectType == Weapon.WeaponEffect.CloseCounter) //If has counter all
        {
            if (distanceBetweenUnitsVector2.x == 1 || distanceBetweenUnitsVector2.y == 1)
            {
				Debug.Log("2+");
                return true;
            }
            else
            {
				Debug.Log("3+");
                return false;
            }
        }
        else
        {
			Debug.Log("4+");
            CheckForDistantDefense(distanceBetweenUnitsVector2);
            return false;
        }
    }

    void CheckForDistantDefense(Vector2 distanceBetweenUnitsVector2)
    {
        if (defendingUnit.GetComponent<UnitInventory>().currentWeapon.WeaponEffectType == Weapon.WeaponEffect.DistantDefense) //Has distant defense defender
        {
            if (distanceBetweenUnitsVector2.x > 1 || distanceBetweenUnitsVector2.y > 1)
            {
                defendingUnit.GetComponent<UnitStats>().defense += 9;
                defendingUnit.GetComponent<UnitStats>().resistance += 9;
                Debug.Log("Activating distant defense for defender");
                distantDefenseActivatedDefending = true;
            }
        }
    }
    void RemoveDistantDefenseBuff()
    {
        if(distantDefenseActivatedDefending)
        {
            defendingUnit.GetComponent<UnitStats>().defense -= 9;
            defendingUnit.GetComponent<UnitStats>().resistance -= 9;
            distantDefenseActivatedDefending = false;
        }
    }

    bool IfUnitIsStillAlive(GameObject unit)
    {
        if(unit.GetComponent<UnitStats>().health <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void CleanUpDeadUnits()
    {
        if (ScriptLink.mouseController.SelectedUnit.GetComponent<UnitStats>().health <= 0) //Checking if attacking unit is alive. If he is not then kill him.
        {
            if (ScriptLink.mouseController.SelectedUnit.GetComponent<UnitStats>().isRed)
            {
                ScriptLink.unitSpawner.redCount--;
                ScriptLink.unitSpawner.redUnits.Remove(ScriptLink.mouseController.SelectedUnit);
            }
            else
            {
                ScriptLink.unitSpawner.greenCount--;
                ScriptLink.unitSpawner.greenUnits.Remove(ScriptLink.mouseController.SelectedUnit);
            }
            Destroy(ScriptLink.mouseController.SelectedUnit);
        }
        if (ScriptLink.mouseController.EnemyUnit.GetComponent<UnitStats>().health <= 0) //Checking if defending unit is alive. If he is not then kill him.
        {
            if (ScriptLink.mouseController.EnemyUnit.GetComponent<UnitStats>().isRed)
            {
                ScriptLink.unitSpawner.redCount--;
                ScriptLink.unitSpawner.redUnits.Remove(ScriptLink.mouseController.EnemyUnit);
            }
            else
            {
                ScriptLink.unitSpawner.greenCount--;
                ScriptLink.unitSpawner.greenUnits.Remove(ScriptLink.mouseController.EnemyUnit);
            }
            Destroy(ScriptLink.mouseController.EnemyUnit);
        }
    }
}
