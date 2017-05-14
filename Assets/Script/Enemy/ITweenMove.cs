using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITweenMove : MonoBehaviour {

    // Use this for initialization
    private float x;
    private float y;
    void Start() {

        iTween.MoveTo(gameObject, iTween.Hash("x", -0.6, "y", 1, "delay", 1));
        //iTween.MoveTo(gameObject, iTween.Hash("x", 0, "y", 1, "easetype", iTween.EaseType.easeInBounce, "delay", 2));
        //iTween.MoveTo(gameObject, iTween.Hash("x", -1, "y", 0, "easetype", iTween.EaseType.easeInCirc, "delay", 3));
        //iTween.MoveTo(gameObject, iTween.Hash("x", 0, "y", -1, "easetype", iTween.EaseType.easeInElastic, "delay", 4));

        //iTween.MoveTo(gameObject, iTween.Hash("x", 1.5, "y", 0, "easetype", iTween.EaseType.easeInExpo, "delay", 5));
        //iTween.MoveTo(gameObject, iTween.Hash("x", 0, "y", 1.5, "easetype", iTween.EaseType.easeInOutBack, "delay", 6));
        //iTween.MoveTo(gameObject, iTween.Hash("x", -1.5, "y", 0, "easetype", iTween.EaseType.easeInOutBounce, "delay", 7));
        //iTween.MoveTo(gameObject, iTween.Hash("x", 0, "y", -1.5, "easetype", iTween.EaseType.easeInOutCirc, "delay", 8));

        //iTween.MoveTo(gameObject, iTween.Hash("x", 2, "y", 0, "easetype", iTween.EaseType.easeInOutCubic, "delay", 9));
        //iTween.MoveTo(gameObject, iTween.Hash("x", 0, "y", 2, "easetype", iTween.EaseType.easeInOutElastic, "delay", 10));
        //iTween.MoveTo(gameObject, iTween.Hash("x", -2, "y", 0, "easetype", iTween.EaseType.easeInOutExpo, "delay", 11));
        //iTween.MoveTo(gameObject, iTween.Hash("x", 0, "y", -2, "easetype", iTween.EaseType.easeInOutQuad, "delay", 12));

    }
    
    // Update is called once per frame
    void Update () {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    //Debug.Log("sssss");

        //}


        //x = Input.mousePosition.x;
        //y = Input.mousePosition.y;
        //iTween.MoveUpdate(this.gameObject, iTween.Hash("x", x, "y", y,"easetype",iTween.EaseType.easeInBack));


        // iTween.MoveTo(this.gameObject, iTween.Hash("x", 1, "y", 1,"time",3f,"easetype",iTween.EaseType.easeInOutBack));

       
        
    }
}
