using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runanimation : MonoBehaviour
{
    float runSpeed = 10f;　//移動速度
    float moveAgility = 500f;　//機敏性
    float animationSpeed = 8.0f; //アニメーション調整
    float jumpForce = 600f;
    float horizontalInput = 0f;
    float varticalInput = 0f;
    Vector3 difference;
    Vector3 latestPos;　//前回位置
    Animator animator;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");　//
        varticalInput = Input.GetAxis("Vertical");//

    }

    void FixedUpdate()
    {
        Vector3 moceVector = Vector3.zero;

        //静止状態遷移
        if (rb.velocity.magnitude <= 0.01f)
        {
            this.animator.SetBool("idleBool", true); 
            this.animator.speed = 1.0f;
        }

        //移動方向キー
        moceVector.x = runSpeed * horizontalInput;
        moceVector.z = runSpeed * varticalInput;
        //減速
        rb.AddForce(moveAgility * (moceVector - rb.velocity));

        //アニメーション速度調整
        if (rb.velocity.magnitude > 0.01f)
        {
            this.animator.speed = rb.velocity.magnitude / animationSpeed; 
        }

        //ジャンプ
        if (Input.GetKey(KeyCode.Space) && transform.position.y < 0.5) //ジャンプ
        {
            animator.SetTrigger("jumpTrigger");
            rb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
        }

        //移動距離
        difference = this.transform.position - latestPos;
        latestPos = transform.position;
        //移動距離によって遷移
        if (difference.magnitude > 0.05f)
        {
            this.animator.SetBool("idleBool", false);　//走り状態遷移
            if(rb.velocity.y < 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(difference); //移動方向に向く
            }
        }

    }
}