using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationTime : MonoBehaviour {

    // Use this for initialization
    public float Time;　//制限時間
    public GameObject ExC;
    public GameObject Nic;
    public GameObject Nor;
    
    private  float LimitTime;　　　//制限時間フレーム化
    private float CurrentTime;　　 //進行中のフレーム
    private float ChildVolume;　　 //子オブジェクトの数
    private bool IsTimeStart;　　　//読秒開始

    private GameObject camera;　　　

	void Start () {
        camera = GameObject.Find("Main Camera");

        LimitTime = Time * 60;
        CurrentTime = LimitTime;
        ChildVolume = transform.childCount;
        IsTimeStart = false;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (IsTimeStart) CurrentTime--;　//読秒
        Evaluation();
        //画面下に出たら
        if (transform.position.y < camera.GetComponent<Camera>().ScreenToWorldPoint(Vector3.zero).y - transform.lossyScale.y / 2)
            Destroy(gameObject);//消滅
    }

    /// <summary>
    /// ランクの分割り
    /// </summary>
    void Evaluation()
    {
        //敵一つを倒すと,読秒開始
        if (ChildVolume > transform.childCount) IsTimeStart = true;

        //敵を全部消すと、残った時間でランクを判断する、リストに入れる
        if (transform.childCount == 0)
        {
            if (CurrentTime >= LimitTime * 2 / 3)
            {
                Main.Evaluation.Add("S");
                Instantiate(ExC, GameObject.Find("Canvas").transform);
                JudgeUI.TargetPos = transform.position;
            }
            if ((CurrentTime >= LimitTime * 1 / 3) && (CurrentTime < LimitTime * 2 / 3))
            {
                Main.Evaluation.Add("A");
                Instantiate(Nic, GameObject.Find("Canvas").transform);
                JudgeUI.TargetPos = transform.position;
            }
            if (CurrentTime < LimitTime * 1 / 3)
            {
                Main.Evaluation.Add("B");
                Instantiate(Nor, GameObject.Find("Canvas").transform);
                JudgeUI.TargetPos = transform.position;
            }
                

            Destroy(this);//判定終了後、このオブジェクトを消す
        }
        
    }
    

}
