using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAnim : MonoBehaviour {

    private Animator animator;
    private int rnd;
    private int count;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        rnd = Random.Range(1, 30);
	}
	
	// Update is called once per frame
	void Update () {
        count++;
        if (count >= rnd)
        {
            animator.SetBool("Star", true);
        }
	}
}
