using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class spawnAnimal : NetworkBehaviour
{
    Transform _transform;

    public GameObject _AnimalGeneratorObj;
    AnimalGenerator _AnimalGeneratorSc;

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
        _AnimalGeneratorObj = GameObject.Find("AnimalGeneratorObj");
        _AnimalGeneratorSc = _AnimalGeneratorObj.GetComponent<AnimalGenerator>();
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
            _AnimalGeneratorSc.generatedAnimalNum--;
        }
    }
}
