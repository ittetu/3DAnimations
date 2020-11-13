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

    float serchRange = 10f;

    Vector3 toPlayerVec;
    Vector3 toPlayerHVec;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        toPlayerVec = player.transform.position - this.transform.position;
        toPlayerDis = toPlayerVec.magnitude;

        if(toPlayerDis < serchRange)
        {
            toPlayerHVec = new Vector3(toPlayerVec.x, 0f, toPlayerVec.z).normalized;
            this.transform.position += toPlayerHVec * moveSpeed;
            this.transform.rotation = Quaternion.LookRotation(-toPlayerHVec);
        }
        else
        {
            moveCount += Time.deltaTime;
            if(moveCount < moveTime)
            {
                if (moveCount < moveLimit)
                {
                    this.transform.position -= this.transform.forward * moveSpeed;
                }
            }
            else
            {
                moveCount = 0f;
                //向きを変える
                var rotationY = Random.Range(-180f, 180);
                transform.Rotate(0f,rotationY,0f);
                //動く距離をランダムに設定
                moveLimit = Random.Range(0f, 7f);
            }
        }
        
    }
}
