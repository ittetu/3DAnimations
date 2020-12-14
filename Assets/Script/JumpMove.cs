using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JumpMove : NetworkBehaviour
{
    bool jumpSwitch = false;
    bool jumping = false;
    float jumpForce = 35f;
    Rigidbody rb;
    //Animator animator;
    float interbal = 0;
    //AnimatorClipInfo nowAnm;

    void DoJump()//ジャンプ処理
    {
        //nowAnm = animator.GetCurrentAnimatorClipInfo(0)[0];
        if(/*nowAnm.clip.name != "BasicMotions@Jump01" &&*/ interbal > 0.5)
        {
            interbal = 0;
            //animator.SetTrigger("jumpTrigger");
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        interbal += Time.deltaTime;

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            DoJump();
        }*/
        //ジャンプ操作
        if (Input.GetKeyDown(KeyCode.Space) && jumpSwitch == false && jumping == false)
        {
            jumpSwitch = true;
        }
        

        if (Physics.Raycast(this.transform.position + new Vector3(0f,0f,0f),
            this.transform.TransformDirection(Vector3.down), 1.4f))
        {
            jumping = false;
            //animator.SetBool("groundBool", true);
        }

    }

    void FixedUpdate()
    {
        //ジャンプ判断
        if(jumpSwitch == true && jumping == false)
        {
            DoJump();
            jumpSwitch = false;
            jumping = true;
            //animator.SetBool("groundBool", false);
        }

        
     }
}
