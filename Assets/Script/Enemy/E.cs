using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E : MonoBehaviour {

    public GameObject EnemyPrefab;
    public float interval;
    //public float range = 2f;


    // Use this for initialization
    IEnumerator Start()
    {
        while (true)
        {
            Instantiate(EnemyPrefab,EnemyPrefab.transform.position,EnemyPrefab.transform.rotation);
            yield return new WaitForSeconds(interval);
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
