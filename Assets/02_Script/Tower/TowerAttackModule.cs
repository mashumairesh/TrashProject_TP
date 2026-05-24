using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class TowerAttackModule : MonoBehaviour
{

    public float cooltimeAbility;
    public float cooltimeAbilityCurrunt;
    public float cooltimeAbility1;
    public float cooltimeAbility1Currunt;
    public float cooltimeAbility2;
    public float cooltimeAbility2Currunt;

    public abstract void Attack();

    public abstract void Ability1();
    public abstract void Ability2();

}
