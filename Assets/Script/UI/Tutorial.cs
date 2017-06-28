using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject count;
    public GameObject iniBack;
    public GameObject changeBack;

    private GameObject[] tutorialUI;
    private GameObject boss;
    private int currentParsNum;
    private int partsNum;

    // Use this for initialization
    void Start()
    {
        boss = transform.FindChild("Boss").gameObject;
        tutorialUI = GameObject.FindGameObjectsWithTag("Tutorial");
        partsNum = boss.GetComponent<BossEnemy>().childEnemy.Count;
        currentParsNum = partsNum;
    }

    // Update is called once per frame
    void Update()
    {
        Count();
        ChangeBack();
    }

    /// <summary>
    /// チュートリアルカウント
    /// </summary>
    private void Count()
    {
        if (boss == null) return;

        //ボスのパーツが一つ減るごとにカウント
        partsNum = boss.GetComponent<BossEnemy>().childEnemy.Count;
        if (partsNum < currentParsNum)
        {
            GameObject cnt = Instantiate(count, GameObject.Find("Canvas").transform);
            cnt.GetComponent<Text>().text = currentParsNum.ToString();
            currentParsNum = partsNum;
        }
    }

    /// <summary>
    /// 背景変更
    /// </summary>
    private void ChangeBack()
    {
        if (boss != null) return;

        //スクールする背景に変更
        if (iniBack != null)
        {
            Instantiate(changeBack);
            DestroyUI();
            Destroy(iniBack);
        }
    }

    /// <summary>
    /// チュートリルUI削除
    /// </summary>
    private void DestroyUI()
    {
        foreach(var ui in tutorialUI)
        {
            Destroy(ui);
        }
    }
}
