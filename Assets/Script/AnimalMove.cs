using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMove : MonoBehaviour
{
    public GameObject player;

    float moveSpeed = 0.01f;
    float toPlayerDis = 0f;

    float moveTime = 5f;
    float moveLimit = 5f;
    float moveCount = 0f;
    float newAngle = 0f;
    float lastLotate = 0f;
    float angleChange = 0f;

    float serchRange = 10f;

    Vector3 toPlayerVec;
    Vector3 toPlayerHVec;

    // Start is called before the first frame update
    void Start()
    {
        lastLotate = this.transform.eulerAngles.y;
        newAngle = lastLotate;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーに向かうベクトル
        toPlayerVec = player.transform.position - this.transform.position;
        //プレイヤーとの距離
        toPlayerDis = toPlayerVec.magnitude;
        //サーチ範囲にプレイヤーがいれば
        if(toPlayerDis < serchRange)
        {
            ToPlayerMove();
        }
        else
        {
            RandomMove();
        }
        
    }

    void ToPlayerMove()
    {
        //プレイヤーの上下無視位置
        toPlayerHVec = new Vector3(toPlayerVec.x, 0f, toPlayerVec.z).normalized;
        //プレイヤーの水平位置に向いて移動
        this.transform.position += toPlayerHVec * moveSpeed;
        this.transform.rotation = Quaternion.LookRotation(-toPlayerHVec);
    }

    void RandomMove()
    {
        moveCount += Time.deltaTime;
        //少しずつ向きを変える
        angleChange = Mathf.LerpAngle(lastLotate, newAngle, moveCount);
        this.transform.eulerAngles = new Vector3(0f, angleChange, 0f);
        //一定時間同じ方向を向く
        if (moveCount < moveTime)
        {
            //ランダムな時間進む
            if (moveCount < moveLimit)
            {
                this.transform.position -= this.transform.forward * moveSpeed;
            }
        }
        else
        {
            //移動時間リセット
            moveCount = 0f;
            //最後の向きを格納
            lastLotate = this.transform.eulerAngles.y;
            //新しい向き
            newAngle = Random.Range(-180f, 180f);
            //動く距離をランダムに設定
            moveLimit = Random.Range(0f, 6f);
            //動く時間をランダムに設定
            moveTime = Random.Range(1f, 5f);
        }
    }
}
