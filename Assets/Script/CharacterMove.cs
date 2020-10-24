using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    GameObject playerCam;
    float runSpeed = 30f;　//移動速度
    float moveAgility = 500f;
    float horizontalInput = 0f;
    float varticalInput = 0f;
    float anmNormalSpeed = 1.0f;
    Vector3 difference;
    Vector3 differencexz;
    Vector3 latestPos;
    Vector3 horizonVelo;
    Vector3 cameraDir;
    Vector3 cameraLR;
    Animator animator;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = GameObject.Find("PlayerCamera");
        rb = this.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //平行移動入力
        horizontalInput = Input.GetAxis("Horizontal");
        varticalInput = Input.GetAxis("Vertical");

        //カメラの方向を取得
        cameraDir = playerCam.transform.TransformDirection(Vector3.forward);
        cameraLR = playerCam.transform.TransformDirection(Vector3.right);
    }

    void FixedUpdate()
    {
        //update毎にゼロ
        Vector3 horizonMoveVector = Vector3.zero;

        //静止状態遷移
        if (rb.velocity.magnitude < 0.01f)
        {
            this.animator.SetBool("idleBool", true); 
            this.animator.speed = anmNormalSpeed;
        }

        //移動方向キー　カメラの向きに進むようにする
        horizonMoveVector += new Vector3(cameraLR.x,0,cameraLR.z).normalized * runSpeed * horizontalInput;
        horizonMoveVector += new Vector3(cameraDir.x,0,cameraDir.z).normalized * runSpeed * varticalInput;

        //平行移動移動慣性
        horizonVelo = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(moveAgility * (horizonMoveVector - horizonVelo));

        //移動距離
        difference = this.transform.position - latestPos;
        differencexz = new Vector3(difference.x, 0, difference.z);
        latestPos = transform.position;

        //移動距離によって遷移
        if (differencexz.magnitude > 0.01f)
        {
            this.animator.SetBool("idleBool", false);　//走り状態遷移
            transform.rotation = Quaternion.LookRotation(differencexz); //移動方向に向く
            
        }

    }
}