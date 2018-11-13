using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{

    void AttackingUnit(GameObject defendingUnit)
    {
        UnitStats unitStatsAttacking = ScriptLink.mouseController.SelectedUnit.GetComponent<UnitStats>();
        UnitStats unitStatsDefending = defendingUnit.GetComponent<UnitStats>();
        ScriptLink.mouseController.SelectedUnit.GetComponent<Unit>().canAttack = false;

        Attack(ScriptLink.mouseController.SelectedUnit, defendingUnit); //Initial attack
        if(defendingUnit != null)
        {
            Attack(defendingUnit, ScriptLink.mouseController.SelectedUnit); //Counter attack
            if(ScriptLink.mouseController.SelectedUnit != null)
            {
                if (unitStatsAttacking.speed - unitStatsDefending.speed >= 5)
                {
                    Attack(ScriptLink.mouseController.SelectedUnit, defendingUnit); //Second attack from attack
                }
                else if (unitStatsDefending.speed- unitStatsAttacking.speed  >= 5)
                {
                    Attack(defendingUnit, ScriptLink.mouseController.SelectedUnit); //Second counter attack
                }
            }
        }
    }

    void Attack(GameObject attackingUnit, GameObject defendingUnit)
    {
        UnitStats unitStatsAttacking = attackingUnit.GetComponent<UnitStats>();
        UnitStats unitStatsDefending = defendingUnit.GetComponent<UnitStats>();

        if (unitStatsAttacking.isPhysicalDamage)
        {
            unitStatsDefending.health -= unitStatsAttacking.attack + unitStatsDefending.defense;
        }
        else
        {
            unitStatsDefending.health -= unitStatsAttacking.attack + unitStatsDefending.resistance;
        }
        isUnitDead(defendingUnit);
    }

    void isUnitDead(GameObject defendingUnit)
    {
        if (defendingUnit.GetComponent<UnitStats>().health <= 0)
        {
            if (defendingUnit.GetComponent<UnitStats>().isRed)
            {
                ScriptLink.unitSpawner.redCount--;
            }
            else
            {
                ScriptLink.unitSpawner.greenCount--;
            }
        }
    }
}
