using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerEnemy : MonoBehaviour
{

    //적들 웨이브는 자동으로 생성하던지 특정 프리셋을 사용하던지 하샘.

    /*
     * 적들은 자동으로 스폰됨.
     * -> 렌덤
     * 
     * 대충 적들별로 타입이 지정되어 있음. ZX
     * 
     * 스페셜 공격 / 적을 몇번 잡으면 강한 공격을 사용 가능.
     * C = 영수증으로 일정 시간동안 적들을 잡아줌
     * V = 대충 길게 쭉 찔러서 꼬챙이 만듬.
     * 
     * 필요한 시스템.
     * 
     * 적 스폰 -> 풀링
     * 적 이동 -> 전면의 적이 잡히면 이동.
     * -> 하나의 적이 잡힐 경우 한칸씩 이동
     * -> 다수의 적이 잡힐 경우 비어있는 구간을 전부 체워야함.
     * 
     * 링리스트 사용 -> 적들의 논리 배치는 원형으로 이루어져있음.
     * 적들이 앞으로 달려오게끔 만듬.
     * -> 단 이때 적들이 모두 같은 움직임을 하면 어색할 수 있음.
     * Move X축으로, Squaz 사용으로 어색해보이지 않도록.
     * Tween 사용은 최대한 절제하여 램 누수 억제.
     * 모든 엑션은 사망시에 자동으로 Kill 할 수 있도록 함.
     * 
     * 적 사망
     * 적이 올바른 공격을 맞을 시 피격 이펙트 출력 및 사망 처리 -> 빠르게 불투명화
     * 
     * 
     * 필요 설계
     * 
     * 1. 적들의 스폰
     * 1.1 오브젝트 풀링
     * -> 템플릿 본체를 만든 뒤 그곳에서 모든것을 해결 할 수 있게 개발.
     *      -> Manager에서 세부 변수를 제어하는것은 절제.
     * -> 구현 방법 제안.
     *      -> 이미 모든 정보를 가지고 있으며 해당 오브젝트를 사용할 때 모든것이 초기화 된 상태로 내어놓을 수 있게 Manager에서 기초 정보만 전달한다.
     * 
     * 2. 적들의 이동
     * -> Enemy... Script에서 자동으로 이루어지도록 구현. 
     * 
     * 3. 적들의 피격 구현
     * -> Hit() 함수를 만들어 피격을 구현. 이때 체력이 많은 적들은 여러번 맞을 수 있게 한다.
     * 
     * 4. 적들의 사망 처리
     * -> 적들의 모든 체력이 감소하여 사망할 경우 Dead함수를 호출한 뒤 이곳에서 ManagerEnemy를 참조하여 사망되었음을 알려
     * 오브젝트풀에 다시 종속될 수 있도록 한다. 또한 해당 적의 초기화는 내부에서 자동으로 이루어지도록 한다.
     * 
     * Playerble Script와 송신하여야 함.
     * -> 참조로 해결
     * 
     */

    //Var

    public static ManagerEnemy Instance { get; private set; }
    public bool PlayerDead;

    [SerializeField] private Enemy orgEnemy;
    [SerializeField] private ComboSystem comboSystem;

    [SerializeField] private Char_TestCode player;

    //모든 적의 리스트
    [SerializeField] private List<Enemy> enemies;
    //[SerializeField] private Queue<Enemy> enemieQ;
    [SerializeField] private List<Enemy> enemiL;
    [SerializeField] private int EnemeyHendle = 0;

    [SerializeField] private List<GameObject> ePos; //적 포지션
    [SerializeField] private GameObject eSpawnPos;  //적 스폰 포인트

    [SerializeField] private int Player_MaxHealth;
    [SerializeField] private int Player_Health;

    [SerializeField] private List<GameObject> BGCollection;
    //[SerializeField] private int BG_Scroll;

    [SerializeField] private GameObject CavasHolder;
    [SerializeField] private GameObject HealthIcon1;
    [SerializeField] private GameObject HealthIcon2;
    [SerializeField] private List<GameObject> HealthIconsON;
    [SerializeField] private List<GameObject> HealthIconsOFF;

    private void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// 모든 준비 시작.
    /// </summary>
    private void Initialize()
    {
        if (Instance == null)
            Instance = this;

        PlayerDead = false;

        enemies = new List<Enemy>();
        for(int i = 0; i < 40; i++)
        {
            enemies.Add(Instantiate(orgEnemy));//좆까 씨발새끼야
            
        }
        //체력 초기화
        Player_Health = Player_MaxHealth;
        HealthIconsON = new List<GameObject>();
        HealthIconsOFF = new List<GameObject>();
        for (int i = 0; i < Player_MaxHealth; i++)
        {
            HealthIconsON.Add(Instantiate(HealthIcon1, CavasHolder.transform));
            HealthIconsON[i].SetActive(true);
            HealthIconsON[i].transform.DOMoveX(HealthIconsON[i].transform.position.x + (i * 200f), 0f);
        }
        for (int i = 0; i < Player_MaxHealth; i++)
        {
            HealthIconsOFF.Add(Instantiate(HealthIcon2, CavasHolder.transform));
            HealthIconsOFF[i].SetActive(false);
            HealthIconsOFF[i].transform.DOMoveX(HealthIconsOFF[i].transform.position.x + (i * 200f), 0f);
        }

        //enemieQ = new Queue<Enemy>();
        enemiL = new List<Enemy>(); 

        //대충 쳐넣기? ㄴ
        for (int i = 0; i < ePos.Count; i++)
            SpawnEnemey();
        EnemyScroll();
    }

    private void SpawnEnemey()
    {
        //소환 시 큐에 추가
        //enemieQ.Enqueue(enemies[EnemeyHendle++]);
        enemiL.Add(enemies[EnemeyHendle++]);

        //스폰과 함께 초기화 일딴 피통 1
        //enemieQ.Peek().InitCall(Random.Range(0, 1), 1);
        //enemieQ.Peek().gameObject.transform.position = eSpawnPos.transform.position;
        enemiL[enemiL.Count - 1].gameObject.SetActive(true);
        enemiL[enemiL.Count-1].InitCall(Random.Range(0, 2), 1);
        enemiL[enemiL.Count-1].gameObject.transform.position = eSpawnPos.transform.position;


        //핸들 오버할 시 초기화
        if (EnemeyHendle > enemies.Count - 1)
            EnemeyHendle = 0;
    }

    /// <summary>
    /// 단순히 적이 맞기만 했을 경우
    /// 사망 X
    /// </summary>
    public void Call_HitEnemey()
    {

    }

    public void PlayerAttack(int rhs)
    {
        switch (rhs)
        {
            case 0:
            case 1:
                player.CallCooldown();
                if (enemiL[0].isSame(rhs))
                {
                    comboSystem.GetUpdate();
                    enemiL[0].Hit();
                }
                else
                { //플레이어 병신 ㅋㅋ
                    comboSystem.RESTCOMBO();
                    HealthController(0);
                }
                break;
            case 2: //HEAL
                break;
            case 3: //SP
                for (int i = 0; i < enemiL.Count; i++)
                {
                    enemiL[0].Hit(0, 0);}
                break;

            default:
                break;
        }
    }

    public void HealthController(int rhs)
    {

        switch (rhs)
        {
            case 0:
                //Missed
                if (--Player_Health <= 0)
                {
                    //PlayerDead
                    PlayerDead = true;
                }
                HealthIconsON[Player_Health].gameObject.SetActive(false);
                HealthIconsOFF[Player_Health].gameObject.SetActive(true);


                break;
            case 1:
                if (Player_Health >= 3)
                    break;
                else
                {
                    HealthIconsON[Player_Health].gameObject.SetActive(true);
                    HealthIconsOFF[Player_Health].gameObject.SetActive(false);
                    Player_Health++;
                }
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// 적이 뒈ㅔㅔㅔㅔㅔㅔㅔㅔ진 경우 ㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋ
    /// </summary>
    public void Call_DeadEnemy(Enemy dead)
    {
        //스크롤링
        //오브젝트 정리
        //신규 적 생성 및 삽입

        int idx = enemiL.IndexOf(dead);
        if (idx < 0) return;
        enemiL.RemoveAt(idx);

        SpawnEnemey();  //스폰
        EnemyScroll();
        BG_Scroll();
    }

    private void EnemyScroll()
    {
        for(int i = 0; i < ePos.Count; i++)
        {   //모든 적들 해당 리스트로 이? 동?????????????????????????
            enemiL[i].DOKill(false);
            enemiL[i].transform.DOMove(ePos[i].transform.position, 0.25f);
        }
    }

    private void BG_Scroll()
    {
        for (int i = 0; i < BGCollection.Count; i++)
        {
            if (BGCollection[i].transform.position.x < -20f)
            {
                BGCollection[i].transform.position += new Vector3(40f, 0, 0);
            }
            BGCollection[i].transform.DOKill(false);
            BGCollection[i].transform.DOLocalMoveX(BGCollection[i].transform.localPosition.x - 1f, 0.25f);
        }
    }


}
