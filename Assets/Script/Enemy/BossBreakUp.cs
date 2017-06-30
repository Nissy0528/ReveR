using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBreakUp : MonoBehaviour
{
    public GameObject core2;
    public GameObject boss;
    public GameObject BoomEffect;
    public Main main;
    

    public float upspeed;
    public float time;

    private Vector3 savePosition;
    private float minRangeX;
    private float maxRangeX;
    private float minRangeY;
    private float maxRangeY;
    public float range;

    private bool shake;
    private GameObject core2Obj;
    private Camera camera;

    public Sprite newBG;
    // Use this for initialization
    void Start()
    {
        savePosition = transform.position;
        minRangeX = savePosition.x - range;
        maxRangeX = savePosition.x + range;
        minRangeY = savePosition.y - range;
        maxRangeY = savePosition.y + range;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //main = GetComponent<Main>();
    }

    // Update is called once per frame
    void Update()
    {
        bossFrom();
        Shake();

        Vector3 screenPosMax = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));//画面左上の座標
        if (transform.position.y >= screenPosMax.y + transform.localScale.y)
        {
            Destroy(gameObject);

            if (GameObject.Find("MainManager").GetComponent<Main>().GetWave() > 0)
            {
                GameObject.Find("back3").GetComponent<SpriteRenderer>().sprite = newBG;
                GameObject.Find("back3.3").GetComponent<SpriteRenderer>().sprite = newBG;
            }
        }
    }

    void bossFrom()
    {
        if (core2Obj == null&&boss.transform.childCount == 1)
        {
            shake = true;
            
            core2Obj = Instantiate(core2,transform);
            //core2Obj.GetComponent<BossBreakUp>().enabled = false;
        }

        if (core2Obj != null)
        {
            
            if (main.LastWave() == false)
            {
                bossBreakUp();
            }
            else
            {
                BossDead();
                
            }
        }

    }

    private void Shake()
    {
        if (shake == true)
        {
            if (time <= 0.0f)
            {

                return;
            }

            time -= Time.deltaTime;
            if (Time.timeScale > 0.0f)
            {
                float x_val = Random.Range(minRangeX, maxRangeX);
                float y_val = Random.Range(minRangeY, maxRangeY);
                transform.position = new Vector3(x_val, y_val, transform.position.z);

                if(main.LastWave() == true)
                {
                    GameObject effect = Instantiate(BoomEffect, transform.position, transform.rotation);
                    effect.name = "Boom_effct";
                }
            }
        }
    }

    void bossBreakUp()
    {
        Shake();
        if (time <= 0)
        {
            transform.position = new Vector3(core2Obj.transform.position.x,
                                                      core2Obj.transform.position.y + upspeed,
                                                      core2Obj.transform.position.z);
        }
    }
    
    void BossDead()
    {
        Shake();
        if (time <= 0)
        {
            GameObject effect = Instantiate(BoomEffect, transform.position, transform.rotation);
            effect.name = "Boom_effct";
            Destroy(gameObject);
        }
        
    } 
}
