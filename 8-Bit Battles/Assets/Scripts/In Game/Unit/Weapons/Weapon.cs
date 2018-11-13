using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "weapon", menuName = "Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public string weaponDescription;
    public int weaponDamage;
    public int weaponBuffAmount;

    public WeaponEffect WeaponEffectType;
    public enum WeaponEffect
    {
       None,
       EffectiveAgainstJuggernauts,
       EffectiveAgainstArtillery,
       EffectiveAgainstDrones,
       EffectiveAgainstATV,
       Double,
       CloseCounter,
       DistantDefense,
       LeftAndRightAOESplash,
       CardinalSplashDamage,
       FallBack,
       AdjacentHeal,
    }

    public WeaponTypes WeaponType;
    public enum WeaponTypes
    {
        None,
        Shotgun,
        MachineGun,
        Bazooka,
        Magnum,
        Rifle,
        Synthetic,
        Plasma
    }

    public WeaponBuff WeaponBuffType;
    public enum WeaponBuff
    {
        None,
        Health,
        Speed,
        Attack,
        Resistance,
        Defense,
        MovementDistance,
        AttackDistanceMax,
        AttackDistanceMin,
        AttackDistanceMaxAndMin
    }
}
