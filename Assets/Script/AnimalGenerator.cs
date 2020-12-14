using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AnimalGenerator : NetworkBehaviour
{
    public GameObject playerObj;
    public Vector3 playerPos;

    const int poolLimit = 20;//溜めておく数
    const int generateLimit = 10;//出現させる数
    public int generatedAnimalNum = 0;//出現している数

    //animalのprefabを登録
    public List<GameObject>animalList = new List<GameObject>();
    //生成されたobjectを追加
    public List<spawnAnimal> generatedList = new List<spawnAnimal>();

    float minDisToplayer = 30f;//プレイヤーとの最短スポーン位置
    float spawnRange = 20f;//スポーン範囲
    Vector3 spawnPos;//スポーン位置

    bool animalPool = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer) return;

        if (!animalPool)
        {
            spawnAnimal animal;
            playerPos = playerObj.transform.position;

            for (int i = 0; i < poolLimit; i++)
            {
                //アニマルをプール
                animal = ((GameObject)Instantiate(animalList[Random.Range(0, animalList.Count)])).
                    GetComponent<spawnAnimal>();
                animal.GetComponent<AnimalMove>().playerObj = this.playerObj;
                //生成時は非アクティブ
                animal.gameObject.SetActive(false);
                //生成リストに追加
                generatedList.Add(animal);
            }

            animalPool = true;
        }

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

                    //スポーンした分をカウント
                    generatedList[i].Init(spawnPos);
                    generatedAnimalNum++;
                }
            }
        }
    }
}
