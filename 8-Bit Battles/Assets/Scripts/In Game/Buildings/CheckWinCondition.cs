using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWinCondition : MonoBehaviour
{
    GameObject[] pads;
    GameObject redPad;
    GameObject greenPad;

    GameObject greenInvader; //For when a unit taking a base dies or moves off during taking over the base.
    GameObject redInvader;

    int timeOnRedPad = 0;
    int timeOnGreenPad = 0;

    void Start()
    {
        FlowControl.OnTurnChange += this.CheckIfOnPad; //Allows for this script to listen for the onTurnChange event.
    }
    void OnDisable()
    {
        FlowControl.OnTurnChange -= this.CheckIfOnPad;
    }

    void CheckIfOnPad()
    {
        FindPads();
        AreEnemyUnitsOnPads();
    }

    
    void AreEnemyUnitsOnPads()
    {
		ScriptLink.unitArray.UpdateUnitArray ();
        for (int x = 0; x < ScriptLink.unitArray.allUnits.GetLength(1); x++)
        {
            for (int y = 0; y < ScriptLink.unitArray.allUnits.GetLength(0); y++)
            {
                if (ScriptLink.unitArray.allUnits[x, y] != null)
                {
                    if (new Vector2((float) x + 0.5f, (float) y + 0.5f) == new Vector2(redPad.transform.position.x, redPad.transform.position.y) && ScriptLink.unitArray.allUnits[x, y].GetComponent<UnitStats>().isRed == false)
                    {
                        greenInvader = ScriptLink.unitArray.allUnits[x, y];
                        UnitOnPadForXTurns(1, false);
                    }
                    else if (new Vector2((float) x + 0.5f, (float) y + 0.5f) == new Vector2(greenPad.transform.position.x, greenPad.transform.position.y) && ScriptLink.unitArray.allUnits[x, y].GetComponent<UnitStats>().isRed == true)
                    {
                        redInvader = ScriptLink.unitArray.allUnits[x, y];
                        UnitOnPadForXTurns(1, true);
                    }
                    else if(greenInvader != null && new Vector2(greenInvader.transform.position.x, greenInvader.transform.position.y) != new Vector2(redPad.transform.position.x, redPad.transform.position.y))
                    {
                        timeOnRedPad = 0;
                    }
                    else if (redInvader != null && new Vector2(redInvader.transform.position.x, redInvader.transform.position.y) != new Vector2(greenPad.transform.position.x, greenPad.transform.position.y))
                    {
                        timeOnGreenPad = 0;
                    }
                }
            }
        }
    }

    void UnitOnPadForXTurns(int turnAmount, bool redOnGreen)
    {
        if (redOnGreen)
        {
            if(timeOnGreenPad > -1 && timeOnGreenPad < turnAmount)
            {
                timeOnGreenPad++;
            }
            else if(timeOnGreenPad == turnAmount)
            {
                ScriptLink.UIcontroller.TurnOnGameOver(true);
            }
        }
        else
        {
            if (timeOnRedPad > -1 && timeOnRedPad < turnAmount)
            {
                timeOnRedPad++;
            }
            else if (timeOnRedPad == turnAmount)
            {
                ScriptLink.UIcontroller.TurnOnGameOver(false);
            }
        }

        
    }

    public void FindPads() 
    {
        pads = GameObject.FindGameObjectsWithTag("Pad");

        for (int i = 0; i < pads.Length; i++)
        {
            if (pads[i].GetComponent<BuildingProperties>().isRedBuilding == true)
            {
                redPad = pads[i].gameObject;
            }
            else if (pads[i].GetComponent<BuildingProperties>().isGreenBuilding == true)
            {
                greenPad = pads[i].gameObject;
            }
        }
    }
}