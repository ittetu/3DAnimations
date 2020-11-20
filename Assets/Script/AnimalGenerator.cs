using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGenerator : MonoBehaviour
{
    Vector3 playerPos;
    public GameObject playerObj;

    const int poolLimit = 20;
    const int generateLimit = 10;
    int generatedAnimalNum = 0;

    //animalのprefabを登録
    public List<GameObject>animalList = new List<GameObject>();
    //activeになったobjectを追加
    public List<spawnAnimal> generatedList = new List<spawnAnimal>();

    float minDisToplayer = 30f;
    float spawnRange = 20f;
    Vector3 spawnPos;


    // Start is called before the first frame update
    void Start()
    {
        spawnAnimal animal;

        playerPos = playerObj.transform.position;

        for (int i = 0;i < poolLimit; i++)
        {
            //アニマルをプール
            animal = ((GameObject)Instantiate(animalList[Random.Range(0, animalList.Count)])).
                GetComponent<spawnAnimal>();
            animal.GetComponent<AnimalMove>().player = playerObj;
            //生成時は非アクティブ
            animal.gameObject.SetActive(false);
            //生成リストに追加
            generatedList.Add(animal);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(generatedAnimalNum < generateLimit)
        {
            for(int i = 0; i < generatedList.Count; i++)
            {
                if(generatedList[i].gameObject.activeSelf == false)
                {
                    //プレイヤーからminDisToPlayer分離れた距離でspawnRangeの範囲内でスポーン
                    var randomPos = Random.insideUnitCircle * spawnRange;
                    var playerHPos = new Vector3(playerPos.x, 0f, playerPos.z);
                    spawnPos = playerHPos + new Vector3(randomPos.x, 0f, randomPos.y);
                    spawnPos += (spawnPos - playerHPos).normalized * minDisToplayer;


                    //spawn地点にrayしてグラウンドか判定してからspawn
                    //試行回数が増えると微妙かも
                    generatedList[i].Init(spawnPos);
                    generatedAnimalNum++;
                }
            }
        }
    }
}
