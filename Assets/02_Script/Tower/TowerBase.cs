using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{

    //행동 함수
    //소환, 삭제, 행동, 능력사용1,2
    //업그레이드
    /// <summary>
    /// 스폰시 작동할 행동을 적용합니다.
    /// </summary>
    public abstract void Spawn();

    /// <summary>
    /// 삭제시 효과를 적용합니다.
    /// </summary>
    public abstract void Removed();
    /// <summary>
    /// 매턴마다 사용할 능력입니다.
    /// </summary>
    public abstract void doAction();

    /// <summary>
    /// 특수능력 1입니다.
    /// </summary>
    public abstract void doAbility1();
    /// <summary>
    /// 특수능력 2 입니다.
    /// </summary>
    public abstract void doAbility2();

    /// <summary>
    /// 업그레이드시 어떤 작용을 할지 지정합니다.
    /// </summary>
    public abstract void Upgrade();

    /// <summary>
    /// 레벨업 효과입니다.
    /// </summary>
    public abstract void LevelUp();

}
