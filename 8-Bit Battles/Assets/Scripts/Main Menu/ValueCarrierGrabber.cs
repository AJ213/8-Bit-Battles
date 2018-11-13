using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueCarrierGrabber : MonoBehaviour
{
    ExtraValues extraValues;
    public Button button;
    public int mapNum;

	void Start ()
    {
        button = this.gameObject.GetComponent<Button>();
        extraValues = GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>();
        button.onClick.AddListener(CallMapNum);
	}

    void CallMapNum()
    {
        extraValues.SetMapNum(mapNum);
    }
}
