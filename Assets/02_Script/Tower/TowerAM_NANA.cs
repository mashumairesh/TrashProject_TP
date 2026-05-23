using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerAM_NANA : TowerAttackModule
{
 
    
    private List<GameObject> Enemy;

    [SerializeField] private GameObject projectile;
    [SerializeField] private List<GameObject> projectilePools;


    [SerializeField] private float projectileDistance = 20f;
    [SerializeField] private float projectileDuration = 1f;

    private void Awake()
    {
        Enemy = new List<GameObject>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
        if (collision.gameObject.CompareTag("Enemy"))
            Enemy.Add(collision.gameObject);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }


    /// <summary>
    /// 공격 함수입니다. 호출 딸깍.
    /// </summary>
    public override void Attack()
    {
        GameObject target = GetClosestEnemy();

        if (target == null)
            return;

        ShootProjectile(target);
    }

    /// <summary>
    /// 프로젝타일을 발사합니다. DoTween으로 관리합니다.
    /// </summary>
    /// <param name="enemy"></param>
    private void ShootProjectile(GameObject enemy)
    {
        GameObject bullet = GetProjectileFromPool();

        bullet.transform.DOKill();

        bullet.SetActive(true);

        bullet.transform.position = transform.position;

        Vector2 dir =
            (enemy.transform.position - transform.position).normalized;

        Vector2 targetPos =
            (Vector2)transform.position + dir * projectileDistance;

        bullet.transform.DOMove(targetPos, projectileDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                bullet.SetActive(false);
            });
    }


    /// <summary>
    /// 프로젝타일 오브젝트 풀링입니다.
    /// </summary>
    /// <returns></returns>
    private GameObject GetProjectileFromPool()
    {
        for (int i = 0; i < projectilePools.Count; i++)
        {
            if (!projectilePools[i].activeSelf)
            {
                return projectilePools[i];
            }
        }

        GameObject newProjectile = Instantiate(projectile);

        newProjectile.SetActive(false);

        projectilePools.Add(newProjectile);

        return newProjectile;
    }

    /// <summary>
    /// 가장 근접한 적의 위치를 도출합니다.
    /// </summary>
    /// <returns></returns>
    private GameObject GetClosestEnemy()
    {
        GameObject target = null;
        float closestDistance = Mathf.Infinity;
        Vector2 currentPos = transform.position;

        for (int i = 0; i < Enemy.Count; i++)
        {
            GameObject enemy = Enemy[i];

            if (enemy == null || enemy == gameObject)
                continue;

            float sqrDistance = ((Vector2)enemy.transform.position - currentPos).sqrMagnitude;

            if (sqrDistance < closestDistance)
            {
                closestDistance = sqrDistance;
                target = enemy;
            }
        }

        return target;
    }

}
