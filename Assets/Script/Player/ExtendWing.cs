using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendWing : MonoBehaviour
{

    //Transform t;
    private GameObject jointR;
    private GameObject jointL;
    public float ext;
    public float scaleMax;
    // Use this for initialization
    void Start()
    {
        //t = GetComponent<Transform>();
        jointR = GameObject.Find("R_Joint");
        jointL = GameObject.Find("L_Joint");

    }

    public void wingLength()
    {
        //t.transform.localScale = new Vector3(t.localScale.x, t.localScale.y, t.localScale.z);
        jointR.transform.localScale = new Vector3(
            jointR.transform.localScale.x,
            jointR.transform.localScale.y,
            jointR.transform.localScale.z);

        jointL.transform.localScale = new Vector3(
            jointL.transform.localScale.x,
            jointL.transform.localScale.y,
            jointL.transform.localScale.z);
    }

    public void extendWing()
    {
        jointR.transform.localScale += new Vector3(ext, 0, 0);
        jointL.transform.localScale += new Vector3(ext, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        wingLength();

        if (jointR.transform.localScale.x >= scaleMax)
        {
            jointR.transform.localScale = new Vector3(scaleMax, jointR.transform.localScale.y, jointR.transform.localScale.z);
        }
        if (jointL.transform.localScale.x >= scaleMax)
        {
            jointL.transform.localScale = new Vector3(scaleMax, jointL.transform.localScale.y, jointL.transform.localScale.z);
        }
    }
}
