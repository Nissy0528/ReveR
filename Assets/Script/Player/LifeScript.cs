using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeScript : MonoBehaviour
{
    RectTransform rt;//RectTransform
    private GameObject playerGO;//playerオブジェクト
    //public int life;
    
    private float hp;

    private Player player;

    // Use this for initialization
    void Start()
    {
        rt = GetComponent<RectTransform>();//RectTransformを取得
        playerGO = GameObject.Find("Player");//playerオブジェクトを取得
        player = playerGO.GetComponent<Player>();
        hp = player.hp / 100;
    }

    public void Life()
    {
        //rt.sizeDelta = new Vector2(rt.sizeDelta.x, life);//ライフの値を設定
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y * hp);

    }


    public void LifeDown(int life)
    {
        rt.sizeDelta -= new Vector2(0, life);//RectTransformのサイズを取得し、マイナスする

        if (rt.sizeDelta.y <= 0)//取得した値のｙが、０以下になった時
        {
            //Destroy(playerGO);//playerオブジェクトを破壊
            SceneManager.LoadScene("GameOver");
        }
    }
    // Update is called once per frame
    void Update()
    {
        Life();
    }
}
