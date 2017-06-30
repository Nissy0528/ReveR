using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowMove : MonoBehaviour
{

    // Use this for initialization

    public Vector3 ArrowMoveVelocity;
    public GameObject[] select;
    public bool[] isSelect;
    public List<Sprite> text;

    public float time;
    void Start()
    {
        time = 20;
    }

    // Update is called once per frame
    void Update()
    {
        float y = Input.GetAxis("Vertical");
        time--;

        Select();
        textChange();
        if (y <= -0.5f && isSelect[2] == false && time <= 0)
        {
            GetComponent<RectTransform>().position -= ArrowMoveVelocity;
            time = 20;
        }

        if (y >= 0.5f && isSelect[0] == false && time <= 0)
        {
            GetComponent<RectTransform>().position += ArrowMoveVelocity;
            time = 20;
        }
        

    }
    void Select()
    {
        for (int i = 0; i < select.Length; i++)
        {
            if (select[i].GetComponent<RectTransform>().position.y ==
            GetComponent<RectTransform>().position.y)
            {
                SetIsSelect(i);
            }
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
            select[0].GetComponent<Image>().sprite = text[1];
            select[1].GetComponent<Image>().sprite = text[2];
            select[2].GetComponent<Image>().sprite = text[4];
        }
        else if (isSelect[1] == true)
        {
            select[0].GetComponent<Image>().sprite = text[0];
            select[1].GetComponent<Image>().sprite = text[3];
            select[2].GetComponent<Image>().sprite = text[4];
        }
        else if (isSelect[2] == true)
        {
            select[0].GetComponent<Image>().sprite = text[0];
            select[1].GetComponent<Image>().sprite = text[2];
            select[2].GetComponent<Image>().sprite = text[5];
        }
    }
}
