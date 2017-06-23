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
    public int exp;

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
        if (!InCamera()) return;

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
    /// 画面内にいるか
    /// </summary>
    /// <returns></returns>
    private bool InCamera()
    {
        Camera mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();//カメラ
        Vector3 scale = new Vector3(transform.localScale.x / 10, transform.localScale.y / 10, 0);//サイズの半分
        Vector3 screenPosMin = mainCamera.ScreenToWorldPoint(Vector3.zero) + scale;//画面右下の座標
        Vector3 screenPosMax = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f)) - scale;//画面左上の座標

        if (transform.position.x >= screenPosMin.x && transform.position.x <= screenPosMax.x
            && transform.position.y >= screenPosMin.y && transform.position.y <= screenPosMax.y)
        {
            return true;
        }
        return false;
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
