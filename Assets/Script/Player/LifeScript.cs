using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeScript : MonoBehaviour
{
    RectTransform rt;//RectTransform
    private GameObject playerGO;//playerオブジェクト

    private float speed;
    private Vector2 iniSize;
    private Player player;

    // Use this for initialization
    void Start()
    {
        rt = GetComponent<RectTransform>();//RectTransformを取得
        iniSize = rt.sizeDelta;

        playerGO = GameObject.Find("Player");//playerオブジェクトを取得
        player = playerGO.GetComponent<Player>();
        speed = Mathf.Abs(player.speed) / player.speedLimit;
    }

    public void Life()
    {
        speed = Mathf.Abs(player.speed) / player.speedLimit;
        rt.sizeDelta = new Vector2(iniSize.x, iniSize.y * speed);
    }

    // Update is called once per frame
    void Update()
    {
        Life();
    }
}
