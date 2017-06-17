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


    public GameObject BoomEffect;
    public int exp;

    public bool lineFlag;

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

        if (lineFlag == true)
        {
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, GameObject.Find("Boss").transform.position);
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
    /// 死亡判定取得
    /// </summary>
    /// <returns>死亡判定</returns>
    public bool IsOut()
    {
        return isOut;
    }

}
