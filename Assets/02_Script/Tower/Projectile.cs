using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float Damage;

    public void Init(float rhs)
    {
        Damage = rhs;
    }

    /// <summary>
    /// 무언가에게 충돌될 시 실제로 작용할 공격 함수입니다.
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// 해당 프로젝타일이 사라진 뒤 연쇄 작용될 이벤트를 실행할 함수입니다.
    /// </summary>
    public abstract void Decay();
}
