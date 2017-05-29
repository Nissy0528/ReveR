using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour { 
    [SerializeField]
    public List<GameObject> Enemy;
    private int i;
	// Use this for initialization
	void Start () {
        i = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (Enemy[i] != null)
        {
            Enemy[i].SetActive(true);
        }
        else
        {
            if (i < 3)
            {
                i++;
            }
            else
            {
                Destroy(GetComponent<Tutorial>());
            }
            
        }
       
	}
}
