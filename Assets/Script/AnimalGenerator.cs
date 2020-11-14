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

    public List<GameObject>animalList = new List<GameObject>();
    public List<spawnAnimal> generatedList = new List<spawnAnimal>();

    // Start is called before the first frame update
    void Start()
    {
        spawnAnimal animal;

        for (int i = 0;i < poolLimit; i++)
        {
            //アニマルをプール
            animal = ((GameObject)Instantiate(animalList[Random.Range(0, 2)])).GetComponent<spawnAnimal>();
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
                if(generatedList[i].activeSelf == false)
                {
                    generatedList[i].Init(spawnPos);
                }
            }
        }
    }
}
