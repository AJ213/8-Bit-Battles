using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowControl : MonoBehaviour
{
    // Fields
    #region
    float ignoreTimer = 0;

    public delegate void TurnChangedEventHandler();
    public static event TurnChangedEventHandler OnTurnChange;

    bool isRedTurn;

    public int turnNum = 1;

    [SerializeField] Text whatTurnText;

    public Button endTurnButton;

    #endregion
    // Properties 
    #region
    public bool IsRedTurn { get { return this.isRedTurn; } set { this.isRedTurn = value;  } }


    #endregion
    public static void TurnChanged()
    {
        if(OnTurnChange != null)
        {
            OnTurnChange();
        }
    }

	void Update()
	{
		ignoreTimer += Time.deltaTime;
	}

    public void ChangeTurn()
    {
		if (ignoreTimer > 1.0f)
		{
			ignoreTimer = 0;
            ScriptLink.mouseController.SelectedUnit = null;
            if (isRedTurn == true)
	        {
	            isRedTurn = false;
	            whatTurnText.text = "Green Turn";
	        }
	        else
	        {
	            isRedTurn = true;
	            whatTurnText.text = "Red Turn";
	        }
	        turnNum++;
	        TurnChanged();
		}
    }
}
