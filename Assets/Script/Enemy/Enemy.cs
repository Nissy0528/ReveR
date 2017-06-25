using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    // Use this for initialization
    private bool isRWHit;
    private bool isLWHit;

    private int x;
    private int y;
    private bool isOut;

    private Player p_Class;
    private GameObject camera;
    private GameObject player;
    private Exp Exp;
    private Vector2 linePos;


    public GameObject BoomEffect;
    public GameObject startupEffect;
    public Sprite normal;
    public int exp;

    //public GameObject core;

    void Start()
    {
        isRWHit = false;
        isLWHit = false;

        player = GameObject.Find("Player");
        camera = GameObject.Find("Main Camera");
        p_Class = player.GetComponent<Player>();
        Exp = player.GetComponent<Exp>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            if (transform.parent.GetChild(0).tag == "Untagged" && (isLWHit || isRWHit))
            {
                DestroyObj(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
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
            transform.parent = col.transform.parent;
        }
        if (col.gameObject.tag == "R_Joint")
        {
            isRWHit = true;
            transform.parent = col.transform.parent;
        }

        if (isLWHit == true && isRWHit == true)
        {
            DestroyObj(true);
        }
        if (col.transform.tag == "Player")
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    /// <summary>
    /// 消滅
    /// </summary>
    /// <param name="isDead">潰されたか</param>
    private void DestroyObj(bool isDead)
    {
        if (isDead)
        {
            player.GetComponent<Player>().ExtWing();
            camera.GetComponent<MainCamera>().SetShake();
            p_Class.Crush();
            GameObject effect = Instantiate(BoomEffect, transform.position, transform.rotation);
            effect.name = "Boom_effct";
        }
        else
        {
            isOut = true;
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// 起動状態にする
    /// </summary>
    public void SetActive()
    {
        GetComponent<SpriteRenderer>().sprite = normal;//色のついた画像にする
                                                       //起動エフェクト生成
        GameObject s_effect = Instantiate(startupEffect, transform.position, transform.rotation, transform);
        s_effect.GetComponent<SpriteRenderer>().sprite = normal;
    }

}
