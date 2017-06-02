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

    public GameObject gauge;
    public GameObject frame;

    // Use this for initializatio
    void Start()
    {
        //rt = GetComponent<RectTransform>();//RectTransformを取得
        //iniSize = rt.sizeDelta;

        playerGO = GameObject.Find("Player");//playerオブジェクトを取得
        player = playerGO.GetComponent<Player>();
        //speed = Mathf.Abs(player.speed) / player.speedLimit;

        gauge = GameObject.Find("speedgauge");
        frame = GameObject.Find("speedframe");
    }

    public void Life()
    {
        //speed = Mathf.Abs(player.speed) / player.speedLimit;
        //rt.sizeDelta = new Vector2(iniSize.x, iniSize.y * speed);

        gauge.GetComponent<Image>().fillAmount = (player.speed / player.speedLimit) * 0.69f;
        //gauge.GetComponent<Image>().fillAmount = Mathf.Clamp((player.speed / player.speedLimit), 0.11f, 0.89f);

    }

    // Update is called once per frame
    void Update()
    {
        Life();
    }
}
