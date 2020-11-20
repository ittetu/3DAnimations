using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnAnimal : MonoBehaviour
{
    Transform _transform;


    public void Init(Vector3 spawnPos)
    {
        _transform = GetComponent<Transform>();
        //アニマルのスポーン場所
        _transform.position = spawnPos;
        //アクティブにする
        this.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            //動物を消す
            this.gameObject.SetActive(false);
            //generatedAnimalの数を減らす

        }
    }
}
