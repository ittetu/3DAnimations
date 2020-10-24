using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    GameObject player;
    Vector3 playerMove = Vector3.zero;
    Vector3 playerPos = Vector3.zero;
    float horizonRotateSpeed = 20f;
    float varticalRotateSpeed = 5f;
    float mouseMoveX = 0;
    float mouseMoveY = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerMove = player.transform.position - playerPos;
    }

    // Update is called once per frame
    void Update()
    {
        //マウス移動量初期化
        mouseMoveX = 0;
        mouseMoveY = 0;

        //マウスのドラッグ量を変数に代入
        mouseMoveX = Input.GetAxis("Mouse X");
        mouseMoveY = Input.GetAxis("Mouse Y");

        //マウス操作ででカメラ方向転換
        transform.RotateAround(player.transform.position, player.transform.up, mouseMoveX * horizonRotateSpeed);
        transform.RotateAround(player.transform.position, this.transform.right,-1 * mouseMoveY * varticalRotateSpeed);

        //カメラを方向転換なしで追従
        playerMove = player.transform.position - playerPos;
        playerPos = player.transform.position;
        this.transform.position += playerMove;
    }

}
