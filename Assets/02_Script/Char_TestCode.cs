using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.Feedbacks;

public class Char_TestCode : MonoBehaviour
{
    //Var
    //[SerializeField] private Sprite sprIdle;
    //[SerializeField] private List<Sprite> sprMeleeAttack;
    //[SerializeField] private List<Sprite> sprRangeAttack;
    //[SerializeField] private Sprite sprSpecialAttack;

    [SerializeField] private SpriteRenderer sprIdle;
    [SerializeField] private List<SpriteRenderer> sprMeleeAttack;
    [SerializeField] private List<SpriteRenderer> sprRangeAttack;
    [SerializeField] private SpriteRenderer sprSpecialAttack;
    [SerializeField] private SpriteRenderer sprSpecialBill;

    [SerializeField] private AudioClip aud_AttackSound1;
    [SerializeField] private AudioClip aud_AttackSound2;
    [SerializeField] private AudioClip aud_AttackSound3;
    [SerializeField] private AudioClip aud_AttackSound4;

    private int Test_Sellector = 0;

    [SerializeField] private List<SpriteRenderer> sprCollector;

    [SerializeField] private AudioSource audioSource;

    public float pitchTest = 1f;

    [SerializeField] private MMF_Player mmfUlti;

    //Status Var
    [SerializeField] private bool statusHeal;
    [SerializeField] private bool statusUltimite;

    [SerializeField] private int MAX_StatusHeal;
    [SerializeField] private int MAX_StatusUlti;
    [SerializeField] private int _StatusHeal = 0;
    [SerializeField] private int _StatusUlti = 0;

    //Func

    private IEnumerator cor_CoolDown_Heal;
    private IEnumerator cor_CoolDown_Ulti;

    //ETC

    [SerializeField] private Image iconCooldownHeal;
    [SerializeField] private Image iconCooldownUlti;


    enum Action
    {
        idle,
        MeleeAttack_1,
        MeleeAttack_2,
        MeleeAttack_3,
        MeleeAttack_4,
        RangeAttack_1,
        RangeAttack_2,
        RangeAttack_3,
        RangeAttack_4,
        SpecialAction_1,
        SpecialAction_2,
        SpecialAction_3,
        SpecialAction_4,
    }


    private void Awake()
    {
        Initialize();
        //TestSection
        sprCollector = new List<SpriteRenderer>() { sprIdle, sprMeleeAttack[0], sprMeleeAttack[1], sprRangeAttack[0], sprRangeAttack[1], sprSpecialBill, sprSpecialAttack };
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
    }

    private void Initialize()
    {
        statusHeal = false;
        statusUltimite = false;
        iconCooldownHeal.fillAmount = 0f;
        iconCooldownUlti.fillAmount = 0f;

    }

    private void InputManager()
    {
        if (!ManagerEnemy.Instance.PlayerDead)
        {
            //audioSource.pitch = pitchTest;
            if (Input.GetKeyDown(KeyCode.Z))
                ActionController(Action.MeleeAttack_1);
            if (Input.GetKeyDown(KeyCode.X))
                ActionController(Action.RangeAttack_1);
            if (Input.GetKeyDown(KeyCode.C) && statusHeal)
            {
                statusHeal = false;
                _StatusHeal = 0;
                UpdateCooldownVisual();
                ManagerEnemy.Instance.HealthController(1);
                ActionController(Action.SpecialAction_1);
            }
            if (Input.GetKeyDown(KeyCode.V) && statusUltimite)
            {
                statusUltimite = false;
                _StatusUlti = 0;
                UpdateCooldownVisual();
                ActionController(Action.SpecialAction_2);
                mmfUlti.PlayFeedbacks();
            }
        }
    }



    /// <summary>
    /// 특정 액션을 활성화하는 함수입니다.
    /// 해당 함수 내부에서 추가 호출이 이루어지며
    /// 이 추가 호출을 통해 디스플레이 작업이 이루어집니다.
    /// </summary>
    /// <param name="rhs">엑션 파라미터 입니다.</param>
    private void ActionController(Action rhs)
    {
        Action_CallManager(rhs);
        Action_SpritePopup(rhs);    //스프라이트 변경 처리
        Action_AudioManager(rhs);   //오디오 메니저 (메니저 아님) 호출
        //게임 메니저 호출부
    }

    private void Action_CallManager(Action rhs)
    {
        switch (rhs)
        {
            case Action.idle:
                break;
            case Action.MeleeAttack_1:
            case Action.MeleeAttack_2:
            case Action.MeleeAttack_3:
            case Action.MeleeAttack_4:
                ManagerEnemy.Instance.PlayerAttack(0);
                break;
            case Action.RangeAttack_1:
            case Action.RangeAttack_2:
            case Action.RangeAttack_3:
            case Action.RangeAttack_4:
                ManagerEnemy.Instance.PlayerAttack(1);

                break;
            case Action.SpecialAction_1:
                ManagerEnemy.Instance.PlayerAttack(2);

                break;
            case Action.SpecialAction_2:
                ManagerEnemy.Instance.PlayerAttack(3);

                break;
            case Action.SpecialAction_3:
                break;
            case Action.SpecialAction_4:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 스프라이트 그래픽 처리입니다.
    /// </summary>
    /// <param name="rhs">알지?</param>
    private void Action_SpritePopup(Action rhs)
    {
        int tmp = 0;

        //현제 디스플레이 되어있는 스프라이트 오브젝트를 전부 비활성화 합니다.
        for (int i = 0; i < sprCollector.Count; i++) //Disable
            sprCollector[i].gameObject.SetActive(false);

        //Selected Obeject Able
        //스프라이트를 활성화하지만 리스트 일시 내부에 있는 스프라이트를 랜덤으로 활성화 합니다.
        switch (rhs)
        {
            case Action.idle:
                break;
            case Action.MeleeAttack_1:
            case Action.MeleeAttack_2:
            case Action.MeleeAttack_3:
            case Action.MeleeAttack_4:
                tmp = Random.Range(0, sprMeleeAttack.Count);
                sprMeleeAttack[tmp].gameObject.SetActive(true);
                Action_SpriteTween(rhs,tmp);    //스프라이트 트윈 처리
                break;
            case Action.RangeAttack_1:
            case Action.RangeAttack_2:
            case Action.RangeAttack_3:
            case Action.RangeAttack_4:
                tmp = Random.Range(0, sprRangeAttack.Count);
                sprRangeAttack[tmp].gameObject.SetActive(true);
                Action_SpriteTween(rhs, tmp);    //스프라이트 트윈 처리
                break;
            case Action.SpecialAction_1:    //특수 엑션 C
                tmp = Random.Range(0, sprRangeAttack.Count);
                sprSpecialBill.gameObject.SetActive(true);
                Action_SpriteTween(rhs, tmp);    //스프라이트 트윈 처리
                break;
            case Action.SpecialAction_2:    //특수 엑션 V
                sprSpecialAttack.gameObject.SetActive(true);
                Action_SpriteTween(rhs);    //스프라이트 트윈 처리
                break;
            case Action.SpecialAction_3:
                break;
            case Action.SpecialAction_4:
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 스프라이트 트윈 처리입니다.
    /// </summary>
    /// <param name="rhs">알지?</param>
    private void Action_SpriteTween(Action rhs, int lsh = 0)
    {
        //커스텀 트윈 작동부 입니다.
        switch (rhs)
        {
            case Action.idle:
                break;
            case Action.MeleeAttack_1:
            case Action.MeleeAttack_2:
            case Action.MeleeAttack_3:
            case Action.MeleeAttack_4:
                //sprMeleeAttack[lsh].gameObject.transform.DOPunchScale(new Vector3(0.25f, 0.05f, 0f), 0.15f, 1).SetEase(Ease.InOutFlash);
                sprMeleeAttack[lsh].gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
                //흔드르라 이기야
                sprMeleeAttack[lsh].gameObject.transform.DOShakeScale(0.1f, 0.3f);
                //광주는 폭동이야 진압하지 않을 수 없잖아/?
                sprMeleeAttack[lsh].gameObject.transform.DOLocalMoveX(0.6f, 0.1f).SetEase(Ease.InOutFlash);//?
                break;
            case Action.RangeAttack_1:
            case Action.RangeAttack_2:
            case Action.RangeAttack_3:
            case Action.RangeAttack_4:
                sprRangeAttack[lsh].gameObject.transform.DOPunchScale(new Vector3(0.25f, 0.05f, 0f), 0.15f, 2).SetEase(Ease.InOutFlash);
                sprRangeAttack[lsh].gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
                sprRangeAttack[lsh].gameObject.transform.DOLocalMoveX(0.5f, 0.1f).SetEase(Ease.InOutFlash);
                break;
            case Action.SpecialAction_1:    //특수 엑션 C
                sprSpecialBill.gameObject.transform.DOPunchScale(new Vector3(0.25f, 0.05f, 0f), 0.15f, 2).SetEase(Ease.InOutFlash);
                sprSpecialBill.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
                sprSpecialBill.gameObject.transform.DOLocalMoveX(0.5f, 0.1f).SetEase(Ease.InOutFlash);
                break;
            case Action.SpecialAction_2:    //특수 엑션 V
                sprSpecialAttack.gameObject.transform.DOPunchScale(new Vector3(0.25f, 0.05f, 0f), 0.15f, 2).SetEase(Ease.InOutFlash);
                sprSpecialAttack.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
                sprSpecialAttack.gameObject.transform.DOLocalMoveX(0.5f, 0.1f).SetEase(Ease.InOutFlash);
                break;
            case Action.SpecialAction_3:
                break;
            case Action.SpecialAction_4:
                break;
            default:
                break;
        }
    }

    public void CallCooldown()
    {
        Cooldown_Heal();
        Cooldown_Ulti();
        UpdateCooldownVisual();
    }

    private void Cooldown_Heal()
    {
        _StatusHeal++;
        if (_StatusHeal >= MAX_StatusHeal)
            statusHeal = true;
    }
    private void Cooldown_Ulti()
    {
        _StatusUlti++;
        if (_StatusUlti >= MAX_StatusUlti)
            statusUltimite = true;
    }

    private void UpdateCooldownVisual()
    {

        iconCooldownHeal.fillAmount = Mathf.Clamp01((float)_StatusHeal / MAX_StatusHeal);
        iconCooldownUlti.fillAmount = Mathf.Clamp01((float)_StatusUlti / MAX_StatusUlti);
    }

    private IEnumerator corFunc_CoolDown_Heal()
    {
        yield return null;
    }

    private IEnumerator corFunc_CoolDown_Ulti()
    {
        yield return null;
    }

    /// <summary>
    /// 해당 엑션에 맞는 오디오 클립을 재생합니다.
    /// </summary>
    /// <param name="rhs"></param>
    private void Action_AudioManager(Action rhs)
    {
        //커스텀 트윈 작동부 입니다.
        switch (rhs)
        {
            case Action.idle:
                break;
            case Action.MeleeAttack_1:
            case Action.MeleeAttack_2:
            case Action.MeleeAttack_3:
            case Action.MeleeAttack_4:
                audioSource.pitch = 3f;
                audioSource.PlayOneShot(aud_AttackSound1);
                break;
            case Action.RangeAttack_1:
            case Action.RangeAttack_2:
            case Action.RangeAttack_3:
            case Action.RangeAttack_4:
                audioSource.pitch = 1f;
                audioSource.PlayOneShot(aud_AttackSound2);
                break;
            case Action.SpecialAction_1:    //특수 엑션 C
                audioSource.pitch = 0.85f;
                audioSource.PlayOneShot(aud_AttackSound3);
                break;
            case Action.SpecialAction_2:    //특수 엑션 V
                audioSource.pitch = 2f;
                audioSource.PlayOneShot(aud_AttackSound4);
                break;
            case Action.SpecialAction_3:
                break;
            case Action.SpecialAction_4:
                break;
            default:
                break;
        }
    }


}
