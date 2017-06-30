using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{

    // Use this for initialization

    public Vector3 ArrowMoveVelocity;
    public GameObject[] select;
    public GameObject se;
    public bool[] isSelect;

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
        if (y <= -0.5f && isSelect[isSelect.Length - 1] == false && time <= 0)
        {
            Instantiate(se);
            GetComponent<RectTransform>().position -= ArrowMoveVelocity;
            time = 20;
        }

        if (y >= 0.5f && isSelect[0] == false && time <= 0)
        {
            Instantiate(se);
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
}
