using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour {

    public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(0, speed, 0);
        if(transform.position.y < -8.5f)
        {
            transform.position = new Vector3(0, 8.5f, 0);
        }
	}
}
