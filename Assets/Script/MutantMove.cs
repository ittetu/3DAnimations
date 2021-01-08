using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantMove : MonoBehaviour
{
    public List<string> animNameList = new List<string>();

    Animator animator;

    GameObject player;

    Collider leftHandCol;
    Collider rightHandCol;

    State[] states;

    Vector3 playerPos;
    Vector3 toPlayerVec;
    Vector3 toPlayerHVec;//プレイヤーに向かう水平ベクトル
    Vector3 playerHPos;

    int attackNum = 0; //敵の攻撃種類
    int MutantIdleHash = Animator.StringToHash("Base Layer.Mutant Idle");

    float toPlayerDis;//プレイヤーとの直線距離
    float playerSerchArea = 15f;//プレイヤー検知範囲
    float playerAttackArea = 7f;//プレイヤーに攻撃する距離
    float runSpeed = 1f;
    float walkSpeed = 0.5f;
    float changeAngle = 0f;//方向転換時の変数
    float lastAngle = 0f;
    float newAngle = 0f;
    float moveCount = 0f;//移動時間管理
    float moveLimit = 5f;//移動時間制限

    bool run = false;
    bool walking = false;
    bool attack = false;
    bool idle = false;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        states = animator.GetBehaviours<State>();

        leftHandCol = GameObject.Find("LeftHand").GetComponent<Collider>();
        rightHandCol = GameObject.Find("RightHand").GetComponent<Collider>();

        player = GameObject.Find("Assault_Rifle_01_FPSController");
        lastAngle = this.transform.eulerAngles.y;
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveCount += Time.deltaTime; //モーション時間計測

        //プレイヤーとの相関
        playerPos = player.transform.position;
        playerHPos = new Vector3(playerPos.x, 0, playerPos.z);
        toPlayerVec = playerPos -this.transform.position;
        toPlayerHVec = new Vector3(toPlayerVec.x, 0f, toPlayerVec.z);
        toPlayerDis = toPlayerVec.magnitude;

        //プレイヤー検知範囲
        if(toPlayerDis <= playerSerchArea && toPlayerDis > playerAttackArea)
        {
            ToPlayerMove();
        }
        else if(toPlayerDis <= playerAttackArea)//攻撃アニメーション
        {
            Attack();
        }
        else//ランダムに歩く
        {
            RandamMove();
        }
    }

    void ToPlayerMove()
    {
        this.transform.position += toPlayerHVec.normalized * runSpeed * Time.deltaTime;
        this.transform.LookAt(playerHPos);

        if (!run)
        {
            animator.CrossFadeInFixedTime("Mutant Run", 1);
            run = true;

            var _states = animator.GetBehaviours(Animator.StringToHash("Base Layer.Mutant Run"), 0);
            var s = (State)_states[0];

            s.onStateEnter = () => Debug.Log("hasire");
            s.onStateExit = () =>
            {
                run = false;
                Debug.Log("走り終了");
            };
        }
        
    }

    void Attack()
    {
        //アタックアニメーション
        if (!attack)
        {
            attack = true;
            //モーション種類
            attackNum = Random.Range(0, 2);
            //モーション再生
            animator.CrossFadeInFixedTime(animNameList[attackNum], 0.2f);
            //コライダーを有効
            AttackColOn();

            var _states = animator.GetBehaviours(Animator.StringToHash("Base Layer." + animNameList[attackNum]), 0);
            var s = (State)_states[0];

            attackNum = Random.Range(0, 2);

            s.onStateEnter = () => Debug.Log("kougeki");
            s.onStateExit = () =>
            {
                attack = false;
                Debug.Log("アタック終了");
                AttackColOff();
            };
        }
    }

    void RandamMove()
    {
        //向きを徐々に変える
        changeAngle = Mathf.LerpAngle(lastAngle, newAngle, moveCount);
        this.transform.eulerAngles = new Vector3(0, changeAngle, 0);
        this.transform.position += this.transform.forward * walkSpeed * Time.deltaTime;

        //ランダム移動取得
        if (moveCount >= moveLimit)
        {
            moveCount = 0;
            lastAngle = this.transform.eulerAngles.y;
            newAngle = Random.Range(180, -180);
        }
        //歩きアニメーション
        if (!walking)
        {
            animator.CrossFadeInFixedTime("Mutant Walking", 1);
            walking = true;

            var _states = animator.GetBehaviours(Animator.StringToHash("Base Layer.Mutant Walking"), 0);
            var s = (State)_states[0];

            s.onStateEnter = () => Debug.Log("aruke");
            s.onStateExit = () => walking = false;
        }
    }

    void AttackColOn()
    {
        leftHandCol.enabled = true;
        rightHandCol.enabled = true;
    }

    void AttackColOff()
    {
        leftHandCol.enabled = false;
        rightHandCol.enabled = false;
    }
}
