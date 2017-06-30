using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public GameObject LeftEnemy;
    public GameObject RightEnemy;
    public float LaserSpeed = 0.1f;

    private GameObject parent;
    private bool IsColLeftEnemy;
    private bool IsColRightEnemy;

    void Start()
    {
        IsColLeftEnemy = true;
        IsColRightEnemy = true;
        parent = LeftEnemy.transform.parent.gameObject;
        Physics2D.IgnoreLayerCollision(13, 13, true);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckLR();
        DestroyGameObject();

    }
    void Move()
    {
        if (!IsColRightEnemy)
        {
            transform.localScale += new Vector3(LaserSpeed, 0, 0);
            transform.localPosition += new Vector3(LaserSpeed / 2, 0, 0);
        }
        else if (!IsColLeftEnemy)
        {
            transform.localScale += new Vector3(LaserSpeed, 0, 0);
            transform.localPosition += new Vector3(-LaserSpeed / 2, 0, 0);
        }
    }

    void DestroyGameObject()
    {
        if (LeftEnemy == null && RightEnemy == null) Destroy(parent);
    }

    /// <summary>
    /// 左右の敵がいるか
    /// </summary>
    private void CheckLR()
    {
        if (LeftEnemy == null)
        {
            IsColLeftEnemy = false;
        }
        if (RightEnemy == null)
        {
            IsColRightEnemy = false;
        }
    }

}
