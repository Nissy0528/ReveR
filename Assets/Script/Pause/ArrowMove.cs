using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{

    // Use this for initialization
    static bool IsSelect_3;
    private static bool IsSelect_2;
    private static bool IsSelect_1;

    private Vector3 ArrowMoveVelocity;

    public GameObject Select_1;
    public GameObject Select_2;
    public GameObject Select_3;

    public float time;
    void Start()
    {
        time = 20;
        IsSelect_1 = false;
        IsSelect_3 = false;
        IsSelect_2 = false;
        ArrowMoveVelocity = new Vector3(0, 56);

    }

    // Update is called once per frame
    void Update()
    {
        float y = Input.GetAxis("Vertical");
        time--;

        Select();
        if (y <= -0.5f && IsSelect_3 == false && time <= 0)
        {
            GetComponent<RectTransform>().position -= ArrowMoveVelocity;
            time = 20;
        }

        if (y >= 0.5f && IsSelect_1 == false&&time<=0)
        {
            GetComponent<RectTransform>().position += ArrowMoveVelocity;
            time = 20;
        }

        Debug.Log(GetComponent<RectTransform>().position.y);

    }
    void Select()
    {
        if (Select_1.GetComponent<RectTransform>().position.y ==
            GetComponent<RectTransform>().position.y)
        {
            IsSelect_1 = true;
            IsSelect_2 = false;
            IsSelect_3 = false;
        }


        if (Select_2.GetComponent<RectTransform>().position.y ==
            GetComponent<RectTransform>().position.y)
        {
            IsSelect_1 = false;
            IsSelect_2 = true;
            IsSelect_3 = false;
        }

        if (Select_3.GetComponent<RectTransform>().position.y ==
            GetComponent<RectTransform>().position.y)
        {
            IsSelect_1 = false;
            IsSelect_2 = false;
            IsSelect_3 = true;
        }
    }
    public bool GetIsSelect_1()
    {
        return IsSelect_1;
    }
    public bool GetIsSelect_2()
    {
        return IsSelect_2;
    }
    public bool GetIsSelect_3()
    {
        return IsSelect_3;
    }
}
