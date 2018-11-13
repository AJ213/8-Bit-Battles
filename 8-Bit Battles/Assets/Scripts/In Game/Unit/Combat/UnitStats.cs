using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public int attack;
    public int speed;
    public int defense;
    public int resistance;
    public int minAttackRange;
	public int maxAttackRange;
	public int minAssistRange;
	public int maxAssistRange;
    public int movementDistance;

    public bool isPhysicalDamage;

	public bool flier;
	public bool watermovement;
	public bool groundmovement;

	public bool isRed;


    public UnitType UnitIdentity;
    public enum UnitType
    {
        Infantry,
        Gunner,
        Demolitionist,
        FighterPlane,
        Rifleman,
        Gunslinger,
        Medic,
        Juggernaut,
        Officer,
        ATV,
        ChemicalAgent,
        Spy,
        SpecialForces,
        Sniper,
        Artillery,
        Marine,
        Assassin
    }

    public Weapon.WeaponTypes[] WeaponsThisUnitCanUse;
}
