using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // Var
    [SerializeField] private float mDamage;
    [SerializeField] private float mSpeed;
    [SerializeField] private float mRange;

    private float TestCooldown;

    [SerializeField] private LayerMask enemyLayer;  //이걸 이걸로 관리해야하나?    
    
    // 적 소유
    [SerializeField] private readonly List<Enemy> enemiesInRange = new List<Enemy>();

    //Mono




    //Func
    public IReadOnlyList<Enemy> Enemies => enemiesInRange;




    private void Awake()
    {

    }

    private void Initialize()
    {

    }

    private void Update()
    {
        //Test Attack
        TestCooldown += Time.deltaTime;
        if (TestCooldown > 1f)
        {
            TestCooldown = 0f;
            AttackEnemy();
        }
    }

    private void AttackEnemy()
    {
        for(int i = 0; i < enemiesInRange.Count; i++) 
        {
            enemiesInRange[i].IHit(mDamage);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & enemyLayer) == 0)
            return;

        if (!col.TryGetComponent(out Enemy enemy))
            return;

        if (enemy.IsDead)
            return;

        if (enemiesInRange.Contains(enemy))
            return;

        enemiesInRange.Add(enemy);
        enemy.OnDeath += HandleEnemyDeath;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.TryGetComponent(out Enemy enemy))
            return;

        RemoveEnemy(enemy);
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        RemoveEnemy(enemy);
    }

    private void RemoveEnemy(Enemy enemy)
    {
        if (enemy == null)
            return;

        if (enemiesInRange.Remove(enemy))
        {
            enemy.OnDeath -= HandleEnemyDeath;
        }
    }

    
    private void OnDisable()
    {
        // 트리거 비활성화 시 정리 (씬 전환/타워 파괴 대비)
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            if (enemiesInRange[i] != null)
                enemiesInRange[i].OnDeath -= HandleEnemyDeath;
        }

        enemiesInRange.Clear();
    }


}
