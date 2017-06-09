using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public GameObject LeftEnemy;
    public GameObject RightEnemy;
    public float LaserSpeed = 0.1f;
   
    private bool IsStop;
    private bool IsColLeftEnemy;
    private bool IsColRightEnemy;

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
        if (!IsColRightEnemy)
        {
            transform.localScale += new Vector3(LaserSpeed, 0, 0);
            transform.position += new Vector3(LaserSpeed/2, 0, 0);
        }
        else if(!IsColLeftEnemy)
        {
            transform.localScale += new Vector3(LaserSpeed, 0, 0);
            transform.position += new Vector3(-LaserSpeed/2, 0, 0);
        }
        Debug.Log(count);
    }
    
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if(LeftEnemy != null){
            if(col.gameObject.name == LeftEnemy.transform.name) IsColLeftEnemy = true; }


        if (RightEnemy != null){
            if (col.gameObject.name == RightEnemy.transform.name) IsColRightEnemy = true; }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (LeftEnemy != null){
            if (col.gameObject.name == LeftEnemy.transform.name) IsColLeftEnemy = false; }


        if (RightEnemy != null){
            if (col.gameObject.name == RightEnemy.transform.name) IsColRightEnemy = false; }
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
