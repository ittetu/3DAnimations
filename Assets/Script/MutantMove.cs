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

    float toPlayerDis;//プレイヤーとの直線距離
    float playerSerchArea = 15f;//プレイヤー検知範囲
    float playerAttackArea = 0.7f;//プレイヤーに攻撃する距離
    float runSpeed = 0.1f;
    float walkSpeed = 1f;
    float changeAngle = 0f;
    float lastAngle = 0f;
    float newAngle = 0f;
    float moveCount = 0f;
    float moveLimit = 5f;

    bool run = false;
    bool walking = false;


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

        if (!run)
        {
            animator.CrossFadeInFixedTime("mutant run",1);
            run = true;

            var _states = animator.GetBehaviours(Animator.StringToHash("Base Layer.mutatnt run"),0);
            var s = (State)_states(0);

            s.onStateExit = () => Debug.Log(moveCount);
        }
    }

    void Attack()
    {

    }

    void RandamMove()
    {
        //向きを徐々に変える
        changeAngle = Mathf.LerpAngle(lastAngle, newAngle, moveCount);
        this.transform.eulerAngles = new Vector3(0, changeAngle, 0);
        this.transform.position += this.transform.forward * walkSpeed * Time.deltaTime;

        if (moveCount >= moveLimit)
        {
            moveCount = 0;
            lastAngle = this.transform.eulerAngles.y;
            newAngle = Random.Range(180, -180);
        }

        if (!walking)
        {

        }
    }
}
