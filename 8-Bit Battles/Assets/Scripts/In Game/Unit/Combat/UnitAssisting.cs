using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAssisting : MonoBehaviour
{
    public void Assist()
    {
        if (ScriptLink.mouseController.SelectedUnit.GetComponent<Unit>().canAttack == false)
            return;

        else if (ScriptLink.mouseController.SelectedUnit.GetComponent<UnitStats>().UnitIdentity == UnitStats.UnitType.Medic)
        {
            if (HealAlly() == false)
            {
                return;
            }
        }
        else if (ScriptLink.mouseController.SelectedUnit.GetComponent<UnitStats>().UnitIdentity == UnitStats.UnitType.Officer)
        {
            if (CommandAlly() == false)
            {
                return;
            }
        }
        else
        {
            return;
        }

        ScriptLink.mouseController.SelectedUnit.GetComponent<Unit>().canAttack = false;
        ScriptLink.mouseController.SelectedUnit.GetComponent<Unit>().canMove = false;
        ScriptLink.mouseController.UnselectUnit();
    }

    bool HealAlly()
    {
        if (ScriptLink.mouseController.EnemyUnit.GetComponent<UnitStats>().health == ScriptLink.mouseController.EnemyUnit.GetComponent<UnitStats>().maxHealth)
        {
            return false;
        }
        ScriptLink.mouseController.EnemyUnit.GetComponent<UnitStats>().health += 10;
        Debug.Log("Healed");
        if (ScriptLink.mouseController.EnemyUnit.GetComponent<UnitStats>().health > ScriptLink.mouseController.EnemyUnit.GetComponent<UnitStats>().maxHealth)
        {
            ScriptLink.mouseController.EnemyUnit.GetComponent<UnitStats>().health = ScriptLink.mouseController.EnemyUnit.GetComponent<UnitStats>().maxHealth;
        }
        return true;
    }
    
    bool CommandAlly()
    {
        if (ScriptLink.mouseController.EnemyUnit.GetComponent<Unit>().canMove == false && ScriptLink.mouseController.EnemyUnit.GetComponent<Unit>().canAttack == false && ScriptLink.mouseController.EnemyUnit.GetComponent<UnitStats>().UnitIdentity != UnitStats.UnitType.Officer)
        {
            ScriptLink.mouseController.EnemyUnit.GetComponent<Unit>().canMove = true;
            ScriptLink.mouseController.EnemyUnit.GetComponent<Unit>().canAttack = true;
			ScriptLink.mouseController.EnemyUnit.GetComponent<Unit> ().LightenUnit ();
            return true;
        }
        else
        {
            return false;
        }
    }
}
