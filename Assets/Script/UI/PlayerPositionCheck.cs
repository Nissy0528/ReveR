using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionCheck : MonoBehaviour
{
    public float speed;

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (player != null)
        {
            transform.position =
                 new Vector3(player.transform.position.x,
                             player.transform.position.y,
                             0);
        }
        else
        {
            float vx = Input.GetAxis("Horizontal");
            float vy = Input.GetAxis("Vertical");

            if (vx >= 0.5f || vx <= -0.5f
            || vy >= 0.5f || vy <= -0.5f)
            {
                transform.Translate(speed * -vx, speed * vy, 0.0f);
            }
        }
    }
}
