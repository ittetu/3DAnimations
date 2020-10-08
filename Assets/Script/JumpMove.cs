using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMove : MonoBehaviour
{
    bool jumpSwitch = false;
    bool jumping = false;
    float jumpForce = 350f;
    Rigidbody rb;
    Animator animator;

    void DoJump()//ジャンプ処理
    {
        animator.SetTrigger("jumpTrigger");
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //ジャンプ操作
        if (Input.GetKeyDown(KeyCode.Space) && jumpSwitch == false && jumping == false)
        {
            jumpSwitch = true;
        }
        //Debug.Log(jumpSwitch);
        //Debug.Log(jumping);

        if (Physics.Raycast(this.transform.position + new Vector3(0f,0.01f,0f), this.transform.TransformDirection(Vector3.down), 0.05f))
        {
            jumping = false;
            animator.SetBool("groundBool", true);
            Debug.Log("着地");
        }

    }

    void FixedUpdate()
    {
        //ジャンプ判断
        if(jumpSwitch == true && jumping == false)
        {
            DoJump();
            Debug.Log("離陸");
            jumpSwitch = false;
            jumping = true;
            animator.SetBool("groundBool", false);
        }

        //接地判定
        /*if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.down), 0.1f))
        {
            jumping = false;
            animator.SetBool("groundBool", true);
            Debug.Log("着地");
        }*/
    }
}
