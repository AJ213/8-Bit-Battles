using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineProperties : BuildingProperties 
{
	[SerializeField]
	List<Sprite> mineSprites;
	int imageIndex = 0;
	public GameObject[] mineList;

	public Ownership ownership;
	public enum Ownership
	{
		NoTeam,
		RedTeam,
		GreenTeam
	}
		
	void Start()
	{
		FlowControl.OnTurnChange += OnTurnChange;
		SetStartingImage ();
	}

	void SetStartingImage()
	{
		switch (GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues> ().mapNum)
		{
			case 1:
				SetImage (0);
				break;
			case 2:
				SetImage (3);
				break;
			case 3:
				SetImage (6);
				break;
		}
	}

	void OnDisable()
	{
		FlowControl.OnTurnChange -= OnTurnChange;
	}
		
	void OnTurnChange()
	{
		UpdateOwnership ();
		ScriptLink.economyController.GiveOutMoney (ownership);
	}


	void UpdateOwnership()
	{
		int total, red, green;
		ScriptLink.preDefinedActionTileSpawning.FindUnitsAroundMine (this.gameObject);
		ScriptLink.tileSpreadingManager.CountActionTiles (out total, out red, out green);
		ScriptLink.tileSpreadingManager.ClearActionTiles ();
		if (ScriptLink.flowController.IsRedTurn)
		{
			if (red == 0 && green > 0)
			{
				ownership = Ownership.GreenTeam;
				MineCaptured (ownership);
			}
		}
		else
		{
			if (red > 0 && green == 0)
			{
				ownership = Ownership.RedTeam;
				MineCaptured (ownership);
			}
		}
	}

	void SetImage(int index)
	{
		this.gameObject.GetComponent<Image> ().sprite = mineSprites [index];
	}

	public void MineCaptured(Ownership team)
	{
		switch (team)
		{
			case Ownership.NoTeam:
				imageIndex = 0;
				break;
			case Ownership.RedTeam:
				imageIndex = 1;
				break;
			case Ownership.GreenTeam:
				imageIndex = 2;
				break;
		}

		switch (GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues> ().mapNum)
		{
			case 1:
				imageIndex += 0;
				break;
			case 2:
				imageIndex += 3;
				break;
			case 3:
				imageIndex += 6;
				break;
		}
		SetImage (imageIndex);
	}

}
