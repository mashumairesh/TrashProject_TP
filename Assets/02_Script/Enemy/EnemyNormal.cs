using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Feedbacks;

public class EnemyNormal : EnemyBase
{
    [SerializeField] MMFeedbacks mFeedbacks;
    [SerializeField] EnemyHealthModule mHealthModule;

    private void OnEnable()
    {
        Init();
        mFeedbacks = this.gameObject.GetComponentInChildren<MMFeedbacks>();
    }

    public override void Hit(float Damage)
    {
        base.Hit(Damage);
        HPcurrunt -= Damage;
        mFeedbacks.PlayFeedbacks();
        mHealthModule.Update_Health(Mathf.Clamp01(HPcurrunt / HPmax));
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
