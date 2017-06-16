using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoMove : MonoBehaviour {

    public float speed;
    // Use this for initialization
    void Start () {
		
	}

    public void Move()
    {
        transform.Translate(0, speed, 0);
        if (transform.position.y < -15.0f)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        Move();
	}
}
