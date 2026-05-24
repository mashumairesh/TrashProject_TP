using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains;

public class EnemyHealthModule : MonoBehaviour
{

    [SerializeField] private Image imgHealth;

    public void Update_Health(float Amount)
    {
        imgHealth.fillAmount = Amount;
    }

}