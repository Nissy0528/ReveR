using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    // Use this for initialization
    private bool isRWHit;
    private bool isLWHit;
    private bool isOut;
    private int x;
    private int y;
    private Player p_Class;
    private GameObject camera;
    private GameObject player;
    private Exp Exp;
    

    public GameObject BoomEffect;
    public int exp;

    public bool lineFlag;

    void Start()
    {
        isRWHit = false;
        isLWHit = false;
        x = 0;
        y = 0;

        player = GameObject.Find("Player");
        camera = GameObject.Find("Main Camera");
        p_Class = player.GetComponent<Player>();
        Exp = player.GetComponent<Exp>();


    }

    // Update is called once per frame
    void Update()
    {
        if (x != 0)
        {
            x--;
        }
        if (y != 0)
        {
            y--;
        }

        if (transform.parent != null)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            if (transform.parent.GetChild(0).tag == "Untagged")
            {
                //Exp.GetComponent<Exp>().EXP(exp);
                player.GetComponent<Player>().ExtWing();
                camera.GetComponent<MainCamera>().SetShake();
                p_Class.Crush();
                GameObject effect = Instantiate(BoomEffect, transform.position, transform.rotation);
                effect.name = "Boom_effct";
                Destroy(gameObject);
            }
        }

        //画面下に出たら
        if (transform.position.y < camera.GetComponent<Camera>().ScreenToWorldPoint(Vector3.zero).y - transform.lossyScale.y / 2)
        {
            isOut = true;
            Destroy(gameObject);//消滅
        }

        if (lineFlag == true)
        {
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, GameObject.Find("Boss").transform.position);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "L_Joint" && x == 0)
        {
            isLWHit = false;

        }
        if (col.gameObject.tag == "R_Joint" && y == 0)
        {
            isRWHit = false;
        }
        if (col.transform.tag == "Player")
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "L_Joint")
        {
            isLWHit = true;
            x = 60;
            transform.parent = col.transform.parent;
        }
        if (col.gameObject.tag == "R_Joint")
        {
            isRWHit = true;
            y = 60;
            transform.parent = col.transform.parent;
        }

        if (isLWHit == true && isRWHit == true)
        {
            //Exp.GetComponent<Exp>().EXP(exp);
            player.GetComponent<Player>().ExtWing();
            camera.GetComponent<MainCamera>().SetShake();
            p_Class.Crush();
            GameObject effect = Instantiate(BoomEffect, transform.position, transform.rotation);
            effect.name = "Boom_effct";
            Destroy(gameObject);
        }
        if (col.transform.tag == "Player")
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    /// <summary>
    /// 死亡判定取得
    /// </summary>
    /// <returns>死亡判定</returns>
    public bool IsDead()
    {
        return isOut;
    }

}
