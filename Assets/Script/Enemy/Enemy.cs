using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    // Use this for initialization
    private bool isRWHit;
    private bool isLWHit;
    private bool isDead;
    private int x;
    private int y;
    private Player p_Class;

    public GameObject BoomEffect;
    public GameObject camera;
    public GameObject player;
    public Exp Exp;
    public int exp;

    void Start()
    {
        isRWHit = false;
        isLWHit = false;
        x = 0;
        y = 0;
        p_Class = player.GetComponent<Player>();
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

        if (transform.parent.name != "Enemys" && transform.parent.GetChild(0).tag == "Untagged")
        {
            Exp.GetComponent<Exp>().EXP(exp);
            //player.GetComponent<Player>().ExtWing();
            camera.GetComponent<CameraClamp>().SetShake();
            p_Class.Crush();
            GameObject effect = Instantiate(BoomEffect, transform.position, transform.rotation);
            effect.name = "Boom_effct";
            Destroy(gameObject);
        }

    }
    void OnCollisionExit2D(Collision2D col)
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
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "L_Joint")
        {
            isLWHit = true;
            x = 60;
            transform.parent = col.transform.parent;
            isDead = true;
        }
        if (col.gameObject.tag == "R_Joint")
        {
            isRWHit = true;
            y = 60;
            transform.parent = col.transform.parent;
            isDead = true;
        }

        if (isLWHit == true && isRWHit == true)
        {
            Exp.GetComponent<Exp>().EXP(exp);
            //player.GetComponent<Player>().ExtWing();
            camera.GetComponent<CameraClamp>().SetShake();
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
        return isDead;
    }

}
