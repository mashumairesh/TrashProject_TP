using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Var
    [SerializeField] private float mHealth;
    [SerializeField] public float mSpeed;

    private float mMaxHealth;

    //condition
    private bool isDead;


    //etc
    [SerializeField] private TypeEnemy mTypeEnemy;

    //Mono
    [SerializeField] private EnemyPathMoveModule2D MoveModule2D;
    [SerializeField] private EnemyHealthModule EnemyHealthModule;

    //Func
    public bool IsDead { get; private set; }

    public System.Action<Enemy> OnDeath;




    private void Awake()
    {
        __TEST__INIT();
        Initialize();
    }

    private void Initialize()
    {
        mMaxHealth = mHealth;
    }

    private void __TEST__INIT()
    {
        MoveModule2D.Init(this);
    }

    private void InitCopyTypeEnemy()
    {

    }

    public void IHit(float Damage)
    {
        mHealth -= Damage;
        if (mHealth < 0)
        {
            Die();
        }
        else 
        { 
            EnemyHealthModule.Update_Health(mHealth / mMaxHealth);
            Debug.Log((mHealth / mMaxHealth));
        }

    }

    public void Die()
    {
        if (IsDead) return;
        IsDead = true;

        OnDeath?.Invoke(this);
    }
}
