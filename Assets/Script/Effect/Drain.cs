using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drain : MonoBehaviour
{
    public float speed;//移動速度
    public GameObject[] drains;//吸収エフェクト
    public GameObject finishDrain;//吸収終わりエフェクト

    private GameObject[] hinges = new GameObject[2];//移動先のオブジェクト
    private List<GameObject> drainsObj = new List<GameObject>();//吸収エフェクトオブジェクト
    private GameObject f_DrainObj;//吸収終わりエフェクトの座標
    private Vector2[] drainsPos = new Vector2[2];//吸収エフェクトの座標
    private Vector2[] hingesPos = new Vector2[2];//移動先の座標
    private string[] parentNames = new string[2] { "L_WingTip", "R_WingTip" };//吸収エフェクトの親オブジェクトの名前
    private string[] hingeNames = new string[2] { "L_Hinge", "R_Hinge" };//移動先のオブジェクトの名前

    // Use this for initialization
    void Start()
    {
        SpawnDrain();
    }

    // Update is called once per frame
    void Update()
    {
        drainsObj.RemoveAll(x => x == null);
        Move();
        DrainFinish();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {

        for (int i = 0; i < drainsObj.Count; i++)
        {
            hinges[i] = GameObject.Find(hingeNames[i]);//移動先オブジェクト
            drainsPos[i] = drainsObj[i].transform.position;//吸収エフェクトの座標
            hingesPos[i] = hinges[i].transform.position;//移動先の座標

            //移動先に向けて移動
            drainsPos[i] = Vector3.Lerp(drainsPos[i], hingesPos[i], speed * Time.timeScale);
            drainsObj[i].transform.position = drainsPos[i];

            //移動し終わったら消滅
            if (Mathf.Round(drainsPos[i].x * 10) / 10 == Mathf.Round(hingesPos[i].x * 10) / 10
                && Mathf.Round(drainsPos[i].y * 10) / 10 == Mathf.Round(hingesPos[i].y * 10) / 10)
            {
                Destroy(drainsObj[i]);
            }
        }
    }

    /// <summary>
    /// 吸収終わりエフェクト
    /// </summary>
    private void DrainFinish()
    {
        if (drainsObj.Count != 0) return;

        if (f_DrainObj == null)
        {
            f_DrainObj = Instantiate(finishDrain, gameObject.transform);
        }
        else
        {
            Animator anim = f_DrainObj.GetComponent<Animator>();
            var animState = anim.GetCurrentAnimatorStateInfo(0);

            if(animState.normalizedTime>=1)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// 吸収エフェクト生成
    /// </summary>
    /// <param name="parentName">設定する親オブジェクトの名前</param>
    private void SpawnDrain()
    {
        for (int i = 0; i < drains.Length; i++)
        {
            GameObject parent = GameObject.Find(parentNames[i]);
            drainsObj.Add(Instantiate(drains[i], parent.transform.position, parent.transform.rotation, parent.transform));
        }
    }
}
