using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class TowerNana : TowerBase
{
    public MMFeedbacks levelUpFeedback;

    public override void doAbility1()
    {
        
    }

    public override void doAbility2()
    {
        
    }

    public override void doAction()
    {
        
    }

    public override void LevelUp()
    {
        levelUpFeedback.PlayFeedbacks();
    }

    public override void Removed()
    {
        
    }

    public override void Spawn()
    {
        
    }

    public override void Upgrade()
    {
        
    }
}
