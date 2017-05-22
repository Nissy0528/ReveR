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
    private bool isCrush;
    private int x;
    private int y;
    private AudioSource se;
    private Player p_Class;

    public GameObject BoomEffect;
    public GameObject player;
    public GameObject camera;


    void Start()
    {
        isRWHit = false;
        isLWHit = false;
        isCrush = false;
        x = 0;
        y = 0;
        se = GetComponent<AudioSource>();
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

        if (transform.parent.name != "Enemys" && transform.parent.GetChild(0).tag == "Untagged" && !isCrush)
        {
            camera.GetComponent<CameraClamp>().SetShake();
            p_Class.SetIsStop(false);
            Instantiate(BoomEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            //p_Class.StopJoint();
            //isCrush = true;
        }

        //if (p_Class.IsCrush() && (isDead || isCrush))
        //{
        //    Instantiate(BoomEffect, transform.position, transform.rotation);
        //    p_Class.SetIsStop(false);
        //p_Class.SetIsStop(false);
        //    Destroy(gameObject);
        //}

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
            camera.GetComponent<CameraClamp>().SetShake();
            p_Class.SetIsStop(false);
            Instantiate(BoomEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            //p_Class.StopJoint();
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
