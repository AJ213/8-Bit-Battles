using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviour
{
    public int redCount;
    public int greenCount;

    [SerializeField] Text redCountText;
    [SerializeField] Text greenCountText;

    public List<GameObject> unitPrefabs;

    public List<GameObject> redUnits = new List<GameObject>();
    public List<GameObject> greenUnits = new List<GameObject>();

    void Start()
    {
        FlowControl.OnTurnChange += this.TurnChanged; //Allows for this script to listen for the onTurnChange event.
        GameObject.FindGameObjectWithTag("GameController").GetComponent<AllWeapons>().MakeWeapons();
        SpawnAllUnits();
    }
    void SpawnAllUnits()
    {
        if (GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 1)
        {
            SpawnUnit(7, 18, 16);
            SpawnUnit(26, 31, 34);
        }
        else if (GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 2)
        {
            SpawnUnit(7, 19, 32);
            SpawnUnit(26, 33, 15);
        }
        else if (GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 3)
        {
            SpawnUnit(7, 17, 17);
            SpawnUnit(26, 31, 32);
        }

        //TestUnits();
    }
    void TestUnits()
    {
        // R E D T E A M //
        SpawnUnit(0, 25, 29);
        SpawnUnit(1, 25, 28);
        SpawnUnit(2, 25, 27);
        SpawnUnit(3, 25, 26);
        SpawnUnit(4, 25, 25);
        SpawnUnit(5, 25, 24);
        SpawnUnit(6, 25, 23);
        SpawnUnit(7, 25, 22);
        SpawnUnit(8, 25, 21);
        SpawnUnit(9, 25, 20);
        SpawnUnit(10, 25, 19);
        SpawnUnit(11, 25, 18);
        SpawnUnit(12, 25, 17);
        SpawnUnit(13, 25, 16);
        SpawnUnit(14, 25, 15);
        SpawnUnit(15, 25, 13);
        SpawnUnit(16, 25, 12);

        // G R E E N T E A M //
        SpawnUnit(19, 26, 29);
        SpawnUnit(20, 26, 28);
        SpawnUnit(21, 26, 27);
        SpawnUnit(22, 26, 26);
        SpawnUnit(23, 26, 25);
        SpawnUnit(24, 26, 24);
        SpawnUnit(25, 26, 23);
        SpawnUnit(26, 26, 22);
        SpawnUnit(27, 26, 21);
        SpawnUnit(28, 26, 20);
        SpawnUnit(29, 26, 19);
        SpawnUnit(30, 26, 18);
        SpawnUnit(31, 26, 16);
        SpawnUnit(32, 26, 15);
        SpawnUnit(33, 26, 14);
        SpawnUnit(34, 26, 12);
        SpawnUnit(35, 26, 11);
    }

    void TurnChanged() //When turn is changed it updates all unit info
    {
        UpdateAllUnits();
    }
    void OnDisable()
    {
        FlowControl.OnTurnChange -= this.TurnChanged;
    }

    public void SpawnUnit(int index, float positionX, float positionY)
    {
        GameObject unit = unitPrefabs[index];
        unit = Instantiate(unitPrefabs[index], new Vector3(positionX + 0.5f, positionY + 0.5f, -5f), Quaternion.Euler(0, 0.0f, 0.0f)) as GameObject;
        unit.transform.SetParent(transform);

        SetAllUnitIVs(unit);
        StoreUnitInfo(unit);

        unit.GetComponent<Unit>().oldLocation = new Vector2(positionX + 0.5f, positionY + 0.5f);

        if(GameObject.FindGameObjectWithTag("GameController").GetComponent<FlowControl>().turnNum != 1)
        {
            unit.GetComponent<Unit>().canAttack = false;
            unit.GetComponent<Unit>().canMove = false;
			unit.GetComponent<Unit> ().DarkenUnit ();
        }
    }

    void StoreUnitInfo(GameObject unit)
    {
        if (unit.GetComponent<UnitStats>().isRed)
        {
            redCount++;
            redCountText.text = redCount.ToString("Red Count: " + redCount);
            redUnits.Add(unit);
        }
        else
        {
            greenCount++;
            greenCountText.text = greenCount.ToString("Green Count: " + greenCount);
            greenUnits.Add(unit);
        }
    }
    
    public void UpdateAllUnits()
    {
        UpdateRedUnits();
        UpdateGreenUnits();
    }
    void UpdateRedUnits()
    {
        foreach (GameObject Unit in redUnits)
        {
            Unit.GetComponent<Unit>().canMove = true;
            Unit.GetComponent<Unit>().canAttack = true;
        }
    }
    void UpdateGreenUnits()
    {
        foreach (GameObject Unit in greenUnits)
        {
            Unit.GetComponent<Unit>().canMove = true;
            Unit.GetComponent<Unit>().canAttack = true;
        }
    }

    void SetAllUnitIVs(GameObject unit)
    {
        unit.GetComponent<UnitStats>().maxHealth += SetUnitIVs();
        unit.GetComponent<UnitStats>().health = unit.GetComponent<UnitStats>().maxHealth;
        unit.GetComponent<UnitStats>().attack += SetUnitIVs();
        unit.GetComponent<UnitStats>().speed += SetUnitIVs();
        unit.GetComponent<UnitStats>().defense += SetUnitIVs();
        unit.GetComponent<UnitStats>().resistance += SetUnitIVs();
    }
    int SetUnitIVs()
    {
        var boonBaneChances = new Dictionary<int, int>();
        boonBaneChances.Add(0, 60);    // 60% chance to return a buff of 0
        boonBaneChances.Add(3, 16);    // 16% chance to return a buff of positive 3
        boonBaneChances.Add(-3, 16);   // 16% chance to return a buff of negative 3
        boonBaneChances.Add(4, 3);     // 3% chance etc
        boonBaneChances.Add(-4, 3);    // 3% chance etc
        boonBaneChances.Add(5, 1);     // 1% chance etc
        boonBaneChances.Add(-5, 1);    // 1% chance etc

        // This will go through the boonBanes and pick one. The percentages mean that something 50% is 50% more like to be picked than something with 4%
        return WeightedRandomizer.From(boonBaneChances).TakeOne();
    }
}