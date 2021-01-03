using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantMove : MonoBehaviour
{
    Animator animator;

    GameObject player;

    State[] states;

    Vector3 playerPos;
    Vector3 toPlayerVec;
    Vector3 toPlayerHVec;//プレイヤーに向かう水平ベクトル
    Vector3 playerHPos;

    float toPlayerDis;//プレイヤーとの直線距離
    float playerSerchArea = 15f;//プレイヤー検知範囲
    float playerAttackArea = 3f;//プレイヤーに攻撃する距離
    float runSpeed = 0.8f;
    float walkSpeed = 0.5f;
    float changeAngle = 0f;
    float lastAngle = 0f;
    float newAngle = 0f;
    float moveCount = 0f;
    float moveLimit = 5f;

    bool run = false;
    bool walking = false;
    bool attack = false;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        states = animator.GetBehaviours<State>();

        player = GameObject.Find("Assault_Rifle_01_FPSController");
        lastAngle = this.transform.eulerAngles.y;
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

        //１まで走る
        animator.SetFloat("Blend", toPlayerDis);

        if (!run)
        {
            animator.CrossFadeInFixedTime("Move Tree", 1);
            run = true;

            var _states = animator.GetBehaviours(Animator.StringToHash("Base Layer.Move Tree"), 0);
            var s = (State)_states[0];

            s.onStateEnter = () => Debug.Log("hasire");
            s.onStateExit = () => run = false;
        }
        
    }

    void Attack()
    {
        //アタックアニメーション
        if (!attack)
        {
            animator.CrossFadeInFixedTime("Attack Tree", 1);
            attack = true;
            //モーション種類
            var attackN = Random.Range(0, 2);
            animator.SetFloat("attackN", attackN);

            var _states = animator.GetBehaviours(Animator.StringToHash("Base Layer.Attack Tree"), 0);
            var s = (State)_states[0];

            s.onStateEnter = () => Debug.Log("kougeki");
            s.onStateExit = () => attack = false;
        }
    }

    void RandamMove()
    {
        //向きを徐々に変える
        changeAngle = Mathf.LerpAngle(lastAngle, newAngle, moveCount);
        this.transform.eulerAngles = new Vector3(0, changeAngle, 0);
        this.transform.position += this.transform.forward * walkSpeed * Time.deltaTime;

        //歩く
        animator.SetFloat("Blend", 0);

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
            animator.CrossFadeInFixedTime("Move Tree", 1);
            walking = true;

            var _states = animator.GetBehaviours(Animator.StringToHash("Base Layer.Move Tree"), 0);
            var s = (State)_states[0];

            s.onStateEnter = () => Debug.Log("aruke");
            s.onStateExit = () => walking = false;
        }
    }
}
