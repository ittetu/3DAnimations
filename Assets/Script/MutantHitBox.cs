using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MutantHitBox : MonoBehaviour
{
    float mutantHP = 1000;

    public Image _lifeGuage;

    // Start is called before the first frame update
    void Start()
    {
        //MutantHPをイメージで処理
        mutantHP = _lifeGuage.fillAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Debug.Log("弾当たる");
            //ミュータントを点滅
            //ライフゲージ表示
            //ライフゲージ減少
            _lifeGuage.fillAmount -= 0.01f;
        }
    }
}
