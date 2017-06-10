using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{

    // Use this for initialization
    public float time;
    public GameObject Bullet;

    private float flame;
    void Start()
    {
        flame = time * 60;
    }

    // Update is called once per frame
    void Update()
    {
        flame--;
        if (flame <= 0)
        {
            Instantiate(Bullet, transform.position, transform.rotation);
            flame = time * 60;
        }


    }
}
