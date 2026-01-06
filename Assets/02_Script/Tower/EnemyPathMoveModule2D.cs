using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy(부모)의 mSpeed를 참조하여 노드 경로를 따라 이동시키는 모듈.
/// - 이 모듈은 Child 오브젝트에 붙는 것을 전제로 한다.
/// - 디버프/효과 자체는 구현하지 않음: 외부에서 speedMultiplier/speedAdd를 갱신해주면 즉시 반영된다.
/// </summary>
public class EnemyPathMoveModule2D : MonoBehaviour
{
    [Header("Owner (Parent Enemy)")]
    [SerializeField] private Enemy enemy;             // 부모 Enemy 참조
    [SerializeField] private bool moveParent = true;  // true: 부모(Enemy) 이동, false: 자식(모듈) 이동

    [Header("Path")]
    [SerializeField] private Transform[] nodes;       // A부터 순서대로
    [SerializeField] private float arriveDistance = 0.05f;

    [Header("Runtime Speed Modifiers (set by external debuff system)")]
    [Tooltip("기본 속도에 곱해지는 배율 (1 = 그대로, 0 = 정지)")]
    public float speedMultiplier = 1f;

    [Tooltip("기본 속도에 더해지는 가산값 (음수 가능)")]
    public float speedAdd = 0f;

    [Tooltip("최종 속도 최소값(음수 방지용)")]
    public float minFinalSpeed = 0f;

    private int nodeIndex;
    private Transform owner;   // 실제 이동할 Transform (부모 또는 자신)
    private float arriveSqr;

    private void Awake()
    {
        owner = (moveParent && transform.parent != null) ? transform.parent : transform;
        CacheArriveSqr();

        if (enemy == null)
            enemy = ResolveEnemy();
    }

    private void Start()
    {
        // 기존 동작 유지: 시작 시 경로가 지정돼 있으면 첫 노드로 스냅 후 이동 시작
        ResetToPathStart();
    }

    private void OnValidate()
    {
        if (arriveDistance < 0f) arriveDistance = 0f;
        if (minFinalSpeed < 0f) minFinalSpeed = 0f;
        CacheArriveSqr();
    }

    private void CacheArriveSqr()
    {
        arriveSqr = arriveDistance * arriveDistance;
    }

    private Enemy ResolveEnemy()
    {
        // Child에 붙는 전제: 기본은 부모에서 찾고, 없으면 자기 자신에서 찾습니다.
        if (transform.parent != null)
        {
            var parentEnemy = transform.parent.GetComponent<Enemy>();
            if (parentEnemy != null) return parentEnemy;
        }

        return GetComponent<Enemy>();
    }

    /// <summary>
    /// Enemy 참조/경로를 초기화하고 싶을 때 호출.
    /// (스폰 직후 세팅하거나, 프리팹에서 enemy를 직접 지정해도 됨)
    /// </summary>
    public void Init(Enemy enemyRef, Transform[] pathNodes, bool snapToFirstNode = true)
    {
        enemy = enemyRef;
        nodes = pathNodes;

        if (snapToFirstNode) ResetToPathStart();
        else nodeIndex = 0;
    }
    public void Init(Enemy enemyRef, bool snapToFirstNode = true)
    {
        enemy = enemyRef;
        nodes = PathManager.Instance.Nodes;

        if (snapToFirstNode) ResetToPathStart();
        else nodeIndex = 0;
    }

    private void Update()   //DeltaTime을 사용하여 속도 변화는 문제 없을듯.
    {
        if (nodes == null || nodes.Length == 0) return;
        if (owner == null) return;
        if (nodeIndex >= nodes.Length) return;
        if (enemy == null) return;

        float baseSpeed = GetBaseSpeedFromEnemy(enemy);
        float finalSpeed = Mathf.Max(minFinalSpeed, (baseSpeed * speedMultiplier) + speedAdd);
        if (finalSpeed <= 0f) return;

        Vector3 target = nodes[nodeIndex].position;
        owner.position = Vector3.MoveTowards(owner.position, target, finalSpeed * Time.deltaTime);

        if ((owner.position - target).sqrMagnitude <= arriveSqr)
            nodeIndex++;
    }

    /// <summary>
    /// Enemy에서 기본 속도를 가져옵니다.
    /// ※ Enemy 쪽에서 mSpeed 접근이 가능해야 합니다.
    /// </summary>
    private float GetBaseSpeedFromEnemy(Enemy e)
    {
        // 매니저님 요구대로 MonoBehaviour/리플렉션 없이 Enemy 타입으로 직접 참조
        // 아래 한 줄은 Enemy의 구현에 맞춰 "접근 가능한" 멤버명으로 맞춰주시면 됩니다.
        return e.mSpeed;
        // 예: return e.BaseSpeed;
        // 예: return e.Speed;
    }

    public void SetPath(Transform[] pathNodes, bool snapToFirstNode = true)
    {
        nodes = pathNodes;
        if (snapToFirstNode) ResetToPathStart();
        else nodeIndex = 0;
    }

    public void ResetToPathStart()
    {
        nodeIndex = 0;

        if (nodes == null || nodes.Length == 0 || owner == null) return;

        owner.position = nodes[0].position;
        nodeIndex = (nodes.Length > 1) ? 1 : 0;
    }
}
