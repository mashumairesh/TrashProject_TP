using DG.Tweening;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] private MMF_Player mmfCamera;
    [SerializeField] private MMF_Player mmfPlayer;

    [SerializeField] private List<Sprite> sprMeleeEnemy; 
    [SerializeField] private List<Sprite> sprRangeEnemy;
    [SerializeField] private SpriteRenderer spriteRenderer;
    //최대 피통 5
    [SerializeField] private int varHealth; //우와 피통보소~
    [SerializeField] private Vector3 varScale;

    [SerializeField] private int mType;

    private IEnumerator corFunc_DelayedDisable;

    private void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// 초기화
    /// </summary>
    private void Initialize()
    {
        //변수정도만 할당

        //모든 초기화가 종료되면 스스로 비활성화
        //this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 부활하자 이기야
    /// </summary>
    /// <param name="Type">1z2x</param>
    /// <param name="health">그거.</param>
    public void InitCall(int Type, int health = 1)
    {
        InitHealth(health);//피통 정상화
        if (Type == 0) { spriteRenderer.sprite = sprMeleeEnemy[0]; }
        else { spriteRenderer.sprite = sprRangeEnemy[0]; }
        mType = Type;
    }

    private void InitHealth(int rhs)
    {

    }

    public bool isSame(int rhs)
    {
        if(mType == rhs) return true;
        else return false;
    }

    public void Hit(int Damage = 1)
    {
        varHealth -= Damage;
        if (varHealth <= 0)
            Dead();
        else
            ManagerEnemy.Instance.Call_HitEnemey();
        mmfCamera.PlayFeedbacks();
        //디스플레이 수정
    }

    public void Hit(int rhs, int lsh)
    {
        mmfCamera.PlayFeedbacks();
        varHealth = 0;
        Dead();
    }
    

    private void Dead()
    {
        ManagerEnemy.Instance.Call_DeadEnemy(this);
        //해당 사망 디스플레이 처리


        //this.gameObject.SetActive(false);
        DisplayDead();
    }

    private void DisplayDead()
    {
        this.gameObject.transform.DOMove(new Vector3(Random.Range(10f, 20f), Random.Range(10f, 20f), Random.Range(10f, 20f)),0.2f);
        mmfPlayer.PlayFeedbacks();
        if (corFunc_DelayedDisable != null)
            StopCoroutine(corFunc_DelayedDisable);
        corFunc_DelayedDisable = Func_DelayedDisable();
        StartCoroutine(corFunc_DelayedDisable);
    }

    private IEnumerator Func_DelayedDisable()
    {
        yield return new WaitForSeconds(0.2f);
        this.gameObject.SetActive(false);
    }

}

