using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormal : EnemyBase
{


    private void OnEnable()
    {
        Init();
    }

    public override void Hit(float Damage)
    {
        base.Hit(Damage);
        HPcurrunt -= Damage;
        if (HPcurrunt <= 0)
        {
            Dead();
        }

    }

    public override void Dead()
    {
        this.gameObject.SetActive(false);
    }
}
