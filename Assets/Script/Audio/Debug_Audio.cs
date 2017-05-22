using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_Audio : MonoBehaviour
{
    public AudioClip[] enemyDeadSE;
    public int seNum;
    public GameObject player;

    // Use this for initialization
    void Start()
    {
        player.GetComponent<AudioSource>().clip = enemyDeadSE[seNum];
    }

    // Update is called once per frame
    void Update()
    {
        player.GetComponent<AudioSource>().clip = enemyDeadSE[seNum];
    }
}
