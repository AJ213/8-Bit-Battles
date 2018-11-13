using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTileProperties : MonoBehaviour {

	public Sprite Sprite_Movement_Valid;
	public Sprite Sprite_Movement_Invalid;
	public Sprite Sprite_Assist_Valid;
	public Sprite Sprite_Assist_Invalid;
	public Sprite Sprite_Attack_Valid;
	public Sprite Sprite_Attack_Invalid;

	public int distanceLeft;
	public ActionTilePhase actionTilePhase;
	public ActionType actionType;

	public enum ActionTilePhase
	{
		Created,
		Avaliable,
		Used
	}
		
	public enum ActionType
	{
		Movement_Valid,
		Movement_Invalid,
		Attack_Valid,
		Attack_Invalid,
		Assist_Valid,
		Assist_Invalid
	}
}
