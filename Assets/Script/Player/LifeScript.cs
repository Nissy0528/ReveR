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
    public GameObject needleposition;

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
        needleposition = GameObject.Find("NeedlePosition");
    }

    public void Life()
    {
        //speed = Mathf.Abs(player.speed) / player.speedLimit;
        //rt.sizeDelta = new Vector2(iniSize.x, iniSize.y * speed);

        gauge.GetComponent<Image>().fillAmount = (player.speed / player.speedLimit) * 0.69f;
        //gauge.GetComponent<Image>().fillAmount = Mathf.Clamp((player.speed / player.speedLimit), 0.11f, 0.89f);

    }

    public void LifeColor()
    {
        if (player.speed >= 6)
        {
            gauge.GetComponent<Image>().color = new Color(0, 1, 0);
        }

        else if (player.speed >= 3)
        {
            gauge.GetComponent<Image>().color = new Color(1, 1, 0);
        }

        else if (player.speed >= 0)
        {
            gauge.GetComponent<Image>().color = new Color(1, 0, 0);
        }
    }

    public void needleCtrl()
    {
        needleposition.transform.rotation = Quaternion.Euler(0, 0, 122.0f - (244.0f * (player.speed / player.speedLimit)));
    }
    
    // Update is called once per frame
    void Update()
    {
        Life();
        LifeColor();
        //needleCtrl();
    }
}
