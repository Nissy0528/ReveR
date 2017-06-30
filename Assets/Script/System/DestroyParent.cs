using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0 || IsLineOnly())
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 子オブジェクトがラインしかないか
    /// </summary>
    /// <returns></returns>
    private bool IsLineOnly()
    {
        bool isLine = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag != "Line")
            {
                isLine = false;
            }
        }
        return isLine;
    }
}
