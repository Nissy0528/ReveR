using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverEffect : MonoBehaviour {

    // Use this for initialization
    public GameObject Image;
    public GameObject Game;
    public GameObject Over;
    

    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}
    void Move()
    {
        if (Main.IsGameOver)
        {
            Image.GetComponent<Image>().enabled = true;
            if (Game.GetComponent<RectTransform>().localPosition.y <= 0)
            {
                Game.GetComponent<RectTransform>().localPosition += new Vector3(0, 10, 0);
            }
            if (Over.GetComponent<RectTransform>().localPosition.y <= 0 && Game.GetComponent<RectTransform>().localPosition.y >= 0)
            {
                Over.GetComponent<RectTransform>().localPosition += new Vector3(0, 10, 0);
            }

        }
    }
}
