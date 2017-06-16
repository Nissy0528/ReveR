using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kaitenn : MonoBehaviour {

	// Use this for initialization
    public enum Dec { Left,Right}
    public Dec Way;
    public float speed;
    private  float Ve;

	void Start () {
        Ve = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (Way == Dec.Left) Ve += 1 * speed;
        else if (Way == Dec.Right) Ve -= 1 * speed;
        transform.rotation = Quaternion.Euler(0, 0, Ve);
    }
   
}
