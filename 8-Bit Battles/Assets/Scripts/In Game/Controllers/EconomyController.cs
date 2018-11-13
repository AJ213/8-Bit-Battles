using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EconomyController : MonoBehaviour
{
	private static FlowControl flowControl;
    public int redCash;
    public int greenCash;
	public Text redCashDisplay;
	public Text greenCashDisplay;
	public Text shopCashDisplay;

	int mineProfit = 50;

	void Start()
	{
		redCash = 50;
		greenCash = 50;
        UpdateMoneyText();
        FlowControl.OnTurnChange += this.TurnChanged;

	}
	void OnDisable()
	{
		FlowControl.OnTurnChange -= this.TurnChanged;
	}
    void TurnChanged()
    {
        if (ScriptLink.flowController.IsRedTurn)
        {
            redCash += 20;
        }
        else
        {
            greenCash += 25;
        }
        UpdateMoneyText();
    }
	public void UpdateMoneyText()
	{
		redCashDisplay.text = redCash.ToString();
		greenCashDisplay.text = greenCash.ToString();
		flowControl = GameObject.FindGameObjectWithTag ("GameController").GetComponent<FlowControl> ();
		shopCashDisplay.text = ((flowControl.IsRedTurn) ? redCash : greenCash).ToString();
	}

	public void GiveOutMoney(MineProperties.Ownership ownership)
	{
		if (ScriptLink.flowController.IsRedTurn == true && ownership == MineProperties.Ownership.RedTeam)
		{
			ScriptLink.economyController.redCash += mineProfit;
		}
		else if (ScriptLink.flowController.IsRedTurn == false && ownership == MineProperties.Ownership.GreenTeam)
		{
			ScriptLink.economyController.greenCash += mineProfit;
		}
		UpdateMoneyText ();
	}
}
