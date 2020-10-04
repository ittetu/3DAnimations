using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runanimation : MonoBehaviour
{
    float runSpeed = 10f;　//移動速度
    float moveAgility = 500f;
    float jumpForce = 20f;
    float horizontalInput = 0f;
    float varticalInput = 0f;
    Vector3 difference;
    Vector3 differencexz;
    Vector3 latestPos;
    Vector3 rbVelo;
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

        _Update();
    }

    void _Update()
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
        rbVelo = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(moveAgility * (moceVector - rbVelo));

        //ジャンプ
        if (Input.GetKey(KeyCode.Space) && transform.position.y < 0.1)
        {
            animator.SetTrigger("jumpTrigger");
            rb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
        }

        //移動距離
        difference = this.transform.position - latestPos;
        
        differencexz = new Vector3(difference.x, 0, difference.z);
        //移動距離によって遷移
        Debug.Log(rb.velocity.magnitude);
        if (rb.velocity.magnitude > 0.001f)
        {
            this.animator.SetBool("idleBool", false);　//走り状態遷移
            transform.rotation = Quaternion.LookRotation(differencexz); //移動方向に向く
            
        }
        latestPos = transform.position;

    }
}