using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEnemy : MonoBehaviour {

    // Use this for initialization
    //public GameObject GroupEnemy;
    public GameObject Top;
    public GameObject RightUp;
    public GameObject RightDown;
    public GameObject LeftUp;
    public GameObject LefDown;

    public GameObject Keyenemy;

    public float PentagonRadius = 1;
    public float CircleRadius = 1.9f;
    public float speed = 9f;

    private Vector3 Center;
    private float radian;

    private BoxCollider2D[] BoxC; 
    private Turtroial_Move[] TMove;
	void Start () {
        Center = transform.position;
        radian = 0;

        BoxC = GetComponentsInChildren<BoxCollider2D>();
        TMove = GetComponentsInChildren<Turtroial_Move>();

        //var r = 0.8f;
        //var cosr = Mathf.Sqrt(3) * r;
        //Top.transform.position = new Vector3(Center.x, Center.y + r);
        //RightDown.transform.position = new Vector3(Center.x + r / 2, Center.y - cosr);
        //RightUp.transform.position = new Vector3(Center.x + cosr, Center.y + r / 2);
        //LefDown.transform.position = new Vector3(Center.x - r / 2, Center.y - cosr);
        //LeftUp.transform.position = new Vector3(Center.x - cosr, Center.y + r / 2);

        GetExPosition();
    }
	// Update is called once per frame
	void Update () {
        KeyEnemyMove();
        Expansion();
	}
    void Expansion()
    {
        if (IsKeyEnemyDead()){
            foreach (var x in BoxC) if (x != null) x.isTrigger = false;
            foreach (var x in TMove) if (x != null) x.enabled = true;
        }
    }
    void GetExPosition()
    {
        Top.GetComponent<Turtroial_Move>().TargetPosition = new Vector3(
           Top.transform.position.x,
           Top.transform.position.y + PentagonRadius);

        RightDown.GetComponent<Turtroial_Move>().TargetPosition = new Vector3(
            RightDown.transform.position.x + PentagonRadius / 2,
            RightDown.transform.position.y - Mathf.Sqrt(3) * PentagonRadius);

        RightUp.GetComponent<Turtroial_Move>().TargetPosition = new Vector3(
            RightUp.transform.position.x + Mathf.Sqrt(3) * PentagonRadius,
            RightUp.transform.position.y + PentagonRadius / 2);

        LefDown.GetComponent<Turtroial_Move>().TargetPosition = new Vector3(
            LefDown.transform.position.x - PentagonRadius / 2,
            LefDown.transform.position.y - Mathf.Sqrt(3) * PentagonRadius);

        LeftUp.GetComponent<Turtroial_Move>().TargetPosition = new Vector3(
            LeftUp.transform.position.x - Mathf.Sqrt(3) * PentagonRadius,
            LeftUp.transform.position.y + PentagonRadius / 2);
    }
    void KeyEnemyMove()
    {
        if (!IsKeyEnemyDead()){
            Center = transform.position;
            radian += 0.01f * speed;
            var x = Mathf.Sin(radian) * CircleRadius;
            var y = Mathf.Cos(radian) * CircleRadius;
            Keyenemy.transform.position = new Vector3(x + Center.x, y + Center.y);
        }
    }
    bool IsKeyEnemyDead()
    {
        if (Keyenemy == null) return true;
        else return false;
    }
   
}
