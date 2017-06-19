using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{

    public float speed;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, speed * Time.timeScale, 0);
        if (transform.position.y < -24f)
        {
            transform.position = new Vector3(0, 24f, 0);
        }
    }
}
