using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtroial_Move : MonoBehaviour {

    // Use this for initialization
    public float x;
    public float y;
    public float time;
	void Start () {
        var moveHash = new Hashtable();
        moveHash.Add("x", x);
        moveHash.Add("y", y);
        moveHash.Add("time", time);

        iTween.MoveTo(this.gameObject, moveHash);


    }

    // Update is called once per frame
    void Update () {
		
	}
}
