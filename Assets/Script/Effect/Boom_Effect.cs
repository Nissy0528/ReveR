using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_Effect : MonoBehaviour {

    // Use this for initialization
    public GameObject boom;
    public GameObject boom1;
    private  float time;

	void Start () {
        boom.SetActive(true);
        boom1.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        if (!boom1.GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(gameObject);
        }
		
	}
}
