using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    // Use this for initialization

    public float speed;

    private Vector3 Velocity;
    private bool IsHit = false;

    void Start()
    {
        Velocity = (GameObject.FindWithTag("Player").transform.position - transform.position).normalized;
        Physics2D.IgnoreLayerCollision(9, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Velocity * speed * Time.timeScale;

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }




}