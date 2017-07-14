using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowMove : MonoBehaviour
{

    // Use this for initialization

    public Vector3 ArrowMoveVelocity;
    public GameObject[] select;
    public GameObject se;
    public bool[] isSelect;
    public List<Sprite> text;

    public float time;

    //点滅処理
    private int timeAlpha = 15;
    private int timeSpeed = 1;
    private bool IsAlpha = false;
    void Start()
    {
        time = 20;
        isSelect[2] = false;
        isSelect[1] = false;
        isSelect[0] = true;
    }

    // Update is called once per frame
    void Update()
    {
        float y = Input.GetAxis("Vertical");
        time--;

        //Select();
        textChange();
        if (y <= -0.5f && isSelect[2] == true && time <= 0)
        {
            Instantiate(se);
            isSelect[2] = true;
            isSelect[1] = false;
            isSelect[0] = false;

            time = 20;
        }
        if (y >= 0.5f && isSelect[2] == true && time <= 0)
        {
            Instantiate(se);
            isSelect[2] = false;
            isSelect[1] = true;
            isSelect[0] = false;

            time = 20;
        }

        if (y >= 0.5f && isSelect[1] == true && time <= 0)
        {
            Instantiate(se);
            isSelect[2] = false;
            isSelect[1] = false;
            isSelect[0] = true;
            time = 20;
        }
        if (y <= -0.5f && isSelect[1] == true && time <= 0)
        {
            Instantiate(se);
            isSelect[2] = true;
            isSelect[1] = false;
            isSelect[0] = false;
            time = 20;
        }
        if (y >= 0.5f && isSelect[0] == true && time <= 0)
        {
            Instantiate(se);
            isSelect[2] = false;
            isSelect[1] = false;
            isSelect[0] = true;
            time = 20;
        }
        if (y <= -0.5f && isSelect[0] == true && time <= 0)
        {
            Instantiate(se);
            isSelect[2] = false;
            isSelect[1] = true;
            isSelect[0] = false;
            time = 20;
        }


    }
    void Select()
    {
        for (int i = 0; i < select.Length; i++)
        {
            //if (select[i].GetComponent<RectTransform>().position.y ==
            //GetComponent<RectTransform>().position.y)
            //{

            //}
            SetIsSelect(i);
        }
    }

    private void SetIsSelect(int i)
    {
        isSelect[i] = true;
        for (int j = 0; j < isSelect.Length; j++)
        {
            if (j != i)
            {
                isSelect[j] = false;
            }
        }
    }

    public bool GetIsSelect(int i)
    {
        return isSelect[i];
    }
    public void textChange()
    {
        if (isSelect[0] == true)
        {
            //select[0].GetComponent<Image>().sprite = text[1];
            SelectAlpha(0,0);
            select[1].GetComponent<Image>().sprite = text[2];
            select[2].GetComponent<Image>().sprite = text[4];
        }
        else if (isSelect[1] == true)
        {
            select[0].GetComponent<Image>().sprite = text[0];
            //select[1].GetComponent<Image>().sprite = text[3];
            SelectAlpha(1, 2);
            select[2].GetComponent<Image>().sprite = text[4];
        }
        else if (isSelect[2] == true)
        {
            select[0].GetComponent<Image>().sprite = text[0];
            select[1].GetComponent<Image>().sprite = text[2];
            //select[2].GetComponent<Image>().sprite = text[5];
            SelectAlpha(2, 4);
        }
    }
    private void SelectAlpha(int Snumber,int number)
    {

        if (timeAlpha == 15) IsAlpha = false;
        if (timeAlpha == 0) IsAlpha = true;

        timeAlpha += timeSpeed;

        if (IsAlpha)
        {
            timeSpeed = 1;
            select[Snumber].GetComponent<Image>().sprite = text[number+1];
        }
        else
        {
            timeSpeed = -1;
            select[Snumber].GetComponent<Image>().sprite = text[number];
        }


    }
}
