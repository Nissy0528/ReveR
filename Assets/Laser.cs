using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public GameObject LeftEnemy;
    public GameObject RightEnemy;
    public float LaserSpeed = 0.1f;
   
    private bool IsStop;
    private int count;
    void Start () {

        IsStop = false;
        count = 0;
       
    }

    // Update is called once per frame
    void Update () {
        Move();

    }
    void Move()
    {
        Stop();
        DestroyGameObject();
        if (!IsStop && LeftEnemy != null && RightEnemy == null)
        {
            transform.localScale += new Vector3(LaserSpeed, 0, 0);
            transform.position += new Vector3(LaserSpeed/2, 0, 0);
        }
        else if(!IsStop && LeftEnemy == null && RightEnemy != null)
        {
            transform.localScale += new Vector3(LaserSpeed, 0, 0);
            transform.position += new Vector3(-LaserSpeed/2, 0, 0);
        }
        Debug.Log(count);
    }
    
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            count++;
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            count--;
        }
    }

    void Stop()
    {
        if (count == 2)
            IsStop = true;
        else
            IsStop = false;
    }
    void DestroyGameObject()
    {
        if (LeftEnemy == null && RightEnemy == null) Destroy(gameObject);
    }
   
}
