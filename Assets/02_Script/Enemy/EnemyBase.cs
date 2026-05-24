using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public float HPmax;
    public float HPcurrunt;
    public float mSpeed;

    public virtual void Init()
    {
        HPcurrunt = HPmax;
    }

    public virtual void Hit(float Damage)
    {

    }

    public abstract void Dead();

}
