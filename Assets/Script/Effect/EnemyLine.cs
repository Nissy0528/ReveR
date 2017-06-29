using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLine : MonoBehaviour
{
    public GameObject[] target;//ラインの描画先

    private Transform parent;//親オブジェクト
    private Transform changeParent;//切り替える親
    private bool isParentChange;//親変更フラグ

    
    // Use this for initialization
    void Start()
    {
        if (GetComponent<LineRenderer>() != null)
        {
            GetComponent<LineRenderer>().positionCount = target.Length;
        }
        parent = transform.parent;
        changeParent = parent.transform.parent.FindChild("EnemyLine");
        if (parent.GetComponent<Turtroial_Move>() != null&&transform.parent.parent.tag != "Boss")
        {
            isParentChange = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Line();
        ParentChange();
    }

    /// <summary>
    /// ライン描画
    /// </summary>
    private void Line()
    {
        if (target == null || GetComponent<LineRenderer>() == null) return;

        for (int i = 0; i < GetComponent<LineRenderer>().positionCount; i++)
        {
            GetComponent<LineRenderer>().SetPosition(i, target[i].transform.position);
        }
    }

    /// <summary>
    /// 親変更
    /// </summary>
    private void ParentChange()
    {
        if (!isParentChange) return;

        //親オブジェクトの補間移動スクリプトなくなったら
        if (parent.GetComponent<Turtroial_Move>() == null || transform.parent.tag != "Enemy")
        {
            transform.parent = changeParent;//親を変更
            isParentChange = false;
        }
    }
}
