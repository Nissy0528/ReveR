using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drain : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rigid;
    private GameObject hinge;
    private string hingeName;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        hinge = GameObject.Find(hingeName);
        Move();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        transform.position = Vector3.Lerp(transform.position, hinge.transform.position, speed);

        Vector2 pos = transform.position;
        Vector2 hingePos = hinge.transform.position;

        if (Mathf.Round(pos.x * 10) / 10 == Mathf.Round(hingePos.x * 10) / 10
            && Mathf.Round(pos.y * 10) / 10 == Mathf.Round(hingePos.y * 10) / 10)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 移動先のオブジェクトの名前設定
    /// </summary>
    /// <param name="name">オブジェクトの名前</param>
    public void SetHingeName(string name)
    {
        this.hingeName = name;
    }
}
