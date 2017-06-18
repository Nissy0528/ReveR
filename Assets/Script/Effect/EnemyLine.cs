using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLine : MonoBehaviour
{
    public GameObject target;//ラインの描画先

    private Transform parent;
    private bool isParentChange;

    // Use this for initialization
    void Start()
    {
        parent = transform.parent;
        if (parent.GetComponent<Turtroial_Move>() != null)
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
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
    }

    /// <summary>
    /// 親変更
    /// </summary>
    private void ParentChange()
    {
        if (!isParentChange) return;

        //親オブジェクトの補間移動スクリプトなくなったら
        if (parent.GetComponent<Turtroial_Move>() == null)
        {
            transform.parent = parent.transform.parent;//親を変更
            isParentChange = false;
        }
    }
}
