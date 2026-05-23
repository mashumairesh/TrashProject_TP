using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;

public class TowerStatus : MonoBehaviour
{

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //        UpdateEXP(1f);
    //}

    public Sprite sprite;

    //능력치, 능력치 사용 쿨타임,

    #region Properties
    #endregion

    /// <summary>
    /// 능력치 수치입니다.
    /// </summary>
    public float abilityValue;
    /// <summary>
    /// 레벨업당 능력치 수치 증가입니다. 레벨과 합연산입니다.
    /// </summary>
    public float abilityValueUp;
    /// <summary>
    /// 쿨타임입니다.
    /// </summary>
    public float coolTime;
    /// <summary>
    /// 레벨당 쿨타임 하락률입니다. 0.1 이하로 내려가지 않습니다.
    /// </summary>
    public float coolTimeDown;

    /// <summary>
    /// 현제 레벨입니다.
    /// </summary>
    [HideInInspector]public int Level = 1;

    /// <summary>
    /// 달성할 수 있는 최대 래밸입니다.
    /// </summary>
    public int LevelMax;

    /// <summary>
    /// 현제 경험치입니다.
    /// </summary>
    public float EXP;
    /// <summary>
    /// 레벨업이 가능한 경험치 양 입니다.
    /// 레벨업 할때마다 2배씩 증가합니다.
    /// </summary>
    public float EXPLevelUp;

    public UnityEvent EventLevelUp;

    /// <summary>
    /// 상태를 초기화합니다.
    /// </summary>
    public void InitStatus()
    {

    }

    /// <summary>
    /// 스테이터스를 업데이트 합니다.
    /// </summary>
    public void UpdateStatus()
    {
        abilityValue += abilityValueUp;
    }

    /// <summary>
    /// 레벨을 업데이트 합니다.
    /// </summary>
    public void UpdateLevel()
    {
        //레벨업시 이벤트도 나와야함
        EXPLevelUp += EXPLevelUp;
        Level++;
        UpdateStatus();
        EventLevelUp.Invoke();
    }

    /// <summary>
    /// 경험치를 업데이트 합니다.
    /// </summary>
    public void UpdateEXP(float rhs)
    {
        EXP += rhs;

        if (EXP > (EXPLevelUp) && Level <= LevelMax)
        {
            EXP -= EXPLevelUp;
            UpdateLevel();
        }
    }

    



}
