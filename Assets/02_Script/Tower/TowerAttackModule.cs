using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class TowerAttackModule : MonoBehaviour
{

    public float cooltimeAbility;
    public float cooltimeAbilityCurrunt;

    public abstract void Attack();


}
