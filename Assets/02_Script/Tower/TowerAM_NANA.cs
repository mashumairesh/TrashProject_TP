using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerAM_NANA : TowerAttackModule
{
 
    
    private List<GameObject> Enemy;

    [SerializeField] private GameObject projectile;
    [SerializeField] private List<GameObject> projectilePools;
    [SerializeField] private float Damage;

    [Space(10f)]
    [SerializeField] private float Ability1_CooltimeDownPer;
    [SerializeField] private float Ability1_ApplyTime;
    [SerializeField] private bool Ability1_Apply = false;
    [SerializeField] private IEnumerator cor_Ability1;

    [Space(10f)]
    [SerializeField] private float projectileDistance = 20f;
    [SerializeField] private float projectileDuration = 0.3f;

    [SerializeField] private bool isHasEnemy = false;

    private void Awake()
    {
        Enemy = new List<GameObject>();
        cooltimeAbilityCurrunt = cooltimeAbility;
    }

    private void FixedUpdate()
    {
        //Ability Normal
        if (isHasEnemy)
        {
            cooltimeAbilityCurrunt += Time.deltaTime;
            if (cooltimeAbilityCurrunt > cooltimeAbility)
            {
                Attack();
                cooltimeAbilityCurrunt = 0;
            }
        }
        //Ability 1
        //공격속도 상승
        cooltimeAbility1Currunt += Time.deltaTime;
        if (cooltimeAbility1Currunt > cooltimeAbility1)
        {
            Ability1();
            cooltimeAbility1Currunt = 0;
        }

        //Ability 2


    }

    /// <summary>
    /// 적이 반경에 위치할경우 지속적으로 해당 관련 부분을 업데이트 하고 기록함.
    /// TriggerEnter2D하고 병합되는 부분이 생김.
    /// 다만 0.02f 정도의 오차율이라 감안할 예정.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (Enemy.Count != 0)
            {
                isHasEnemy = true;
            }
        }
    }


    /// <summary>
    /// 적이 사격 반경내에 진입할 경우 풀에 추가.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            Enemy.Add(collision.gameObject);
    }
    /// <summary>
    /// 적이 사격 반경외부로 나갈 경우 풀에서 제거.
    /// 만약 풀이 0일경우 쿨타임 초기화
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy.Remove(collision.gameObject);
            if (Enemy.Count == 0)
            {
                isHasEnemy = false;
                cooltimeAbilityCurrunt = cooltimeAbility;
            }
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

    public override void Ability1()
    {
        if (cor_Ability1 != null)
            StopCoroutine(cor_Ability1);
        cor_Ability1 = corFunc_Ability1();
        StartCoroutine(cor_Ability1);
    }

    public IEnumerator corFunc_Ability1()
    {
        float tmp = cooltimeAbility;
        Ability1_Apply = true;
        cooltimeAbility = cooltimeAbility - cooltimeAbility * Ability1_CooltimeDownPer;
        yield return new WaitForSeconds(Ability1_ApplyTime);
        Ability1_Apply = false;
        cooltimeAbility = tmp;
    }

    public override void Ability2() 
    {
        //Passived
    }

    /// <summary>
    /// 프로젝타일을 발사합니다. DoTween으로 관리합니다.
    /// </summary>
    /// <param name="enemy"></param>
    private void ShootProjectile(GameObject enemy)
    {
        //풀에서 퍼와서 임시 저장.
        GameObject bullet = GetProjectileFromPool();

        bullet.GetComponent<Projectile>().Init(Damage);
        //DoTween 초기화
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
