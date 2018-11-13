using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "shop item")]
public class ShopItem : ScriptableObject
{
    public new string name;
    public string description;

    public Sprite artwork;

    public int cost;
    public int shopOrder;

    public int unitIndex;

    public bool isUnit;
}
