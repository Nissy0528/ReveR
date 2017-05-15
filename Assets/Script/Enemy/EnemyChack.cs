using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyChack : MonoBehaviour {

    GameObject[] enemyobj;
    int enemynum;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        enemyobj = GameObject.FindGameObjectsWithTag("Enemy");
        enemynum = enemyobj.Length;

        Debug.Log(enemynum);

        test();
    }

    public void test()
    {
        if (enemynum == 0)
        {
            SceneManager.LoadScene("GameClear");
        }
    }
}
