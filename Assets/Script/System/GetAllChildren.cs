using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetAllChildren
{
    /// <summary>
    /// 全ての子要素取得
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static List<GameObject> GetAll(this GameObject obj)
    {
        List<GameObject> allChildren = new List<GameObject>();
        GetChildren(obj, ref allChildren);
        return allChildren;
    }

    /// <summary>
    /// 子要素を取得してリストに追加
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="allChildren"></param>
    public static void GetChildren(GameObject obj,ref List<GameObject> allChildren)
    {
        Transform children = obj.GetComponentInChildren<Transform>();
        //子要素がなければ終了
        if(children.childCount==0)
        {
            return;
        }
        foreach(Transform ob in children)
        {
            allChildren.Add(ob.gameObject);
            GetChildren(ob.gameObject, ref allChildren);
        }
    }
}
