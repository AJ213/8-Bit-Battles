using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawningArea : MonoBehaviour 
{
	public GameObject[] bases;
	public GameObject redBase;
	public GameObject greenBase;
	public GameObject selectedBase;

	void Start () 
	{

	}

	void Update () 
	{

	}


	public void BasesHaveSpawned()
	{
		FindBases ();
	}



	public void FindBases()
	{
		bases = GameObject.FindGameObjectsWithTag ("Base");

		for (int i = 0; i < bases.Length; i++)
		{
			if (bases[i].GetComponent<BuildingProperties>().isRedBuilding == true)
			{
				redBase = bases [i].gameObject;
			}
			else if (bases[i].GetComponent<BuildingProperties>().isGreenBuilding == true)
			{
				greenBase = bases [i].gameObject;
			}
		}
	}
}
