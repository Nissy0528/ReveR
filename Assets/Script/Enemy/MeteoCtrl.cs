using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoCtrl : MonoBehaviour {

    public float spinspeed;
    private Random rnd;
    private int x;
    

	// Use this for initialization
	void Start () {
        x = Random.Range(0, 2);

    }


    public void Spin()
    {
        spinspeed++;
        if (x == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0 - spinspeed);
        }
        else  if(x == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0 + spinspeed);
        }
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        Spin();
	}
}
