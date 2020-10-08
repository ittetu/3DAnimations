using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    GameObject player;
    Vector3 playerMove = Vector3.zero;
    Vector3 playerPos = Vector3.zero;
    float rotateSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerMove = player.transform.position - playerPos;
    }

    // Update is called once per frame
    void Update()
    {
        //キー操作でカメラ方向転換
        if (Input.GetKey(KeyCode.I))
        {
            transform.RotateAround(player.transform.position, this.transform.right, -1 * rotateSpeed);
        }
        if (Input.GetKey(KeyCode.L))
        {
            transform.RotateAround(player.transform.position, player.transform.up,rotateSpeed);
        }
        if (Input.GetKey(KeyCode.J))
        {
            transform.RotateAround(player.transform.position, player.transform.up, -1 * rotateSpeed);
        }
        if (Input.GetKey(KeyCode.K))
        {
            transform.RotateAround(player.transform.position, this.transform.right, rotateSpeed);
        }

        //カメラを方向転換なしで追従
        playerMove = player.transform.position - playerPos;
        playerPos = player.transform.position;
        this.transform.position += playerMove;
    }
}
