using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject player;//プレイヤー
    public GameObject L_joint;//左ジョイント
    public GameObject R_joint;//右ジョイント

    private float vx;//縦スティック入力値
    private float vy;//横スティック入力値
    private float oldVx;//前の横スティック入力
    private float oldVy;//前の縦スティック入力
    private float d_Vx;//横入力の差
    private float d_Vy;//縦入力の差
    private float d_Stick;//スティック入力値の差
    private bool isReverse;//反転判定
    private Player p_Class;//プレイヤークラス
    private Joint lj_Class;//左ジョイントクラス
    private Joint rj_Class;//右ジョイントクラス

    // Use this for initialization
    void Start()
    {
        isReverse = false;
        p_Class = player.GetComponent<Player>();
        lj_Class = L_joint.GetComponent<Joint>();
        rj_Class = R_joint.GetComponent<Joint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0.0f) return;

        vx = Input.GetAxis("Horizontal");
        vy = Input.GetAxis("Vertical");

        Reversal();
        Recession();

        //Debug.Log(d_Stick);
    }

    /// <summary>
    /// 反転判定
    /// </summary>
    /// <param name="vx">横スティック入力</param>
    /// <param name="vy">縦スティック入力</param>
    private void Reversal()
    {
        if (vx < 0.5f && vx > -0.5f
            && vy < 0.5f && vy > -0.5f) return;

        StickReverce();

        oldVx = vx;
        oldVy = vy;
    }

    /// <summary>
    /// スティック入力が反転しているか判断する
    /// </summary>
    private void StickReverce()
    {
        if (isReverse) return;

        if (oldVx >= 0.0f && oldVy <= 0.0f
            && vx <= 0.0f && vy >= 0.0f)
        {
            StickDifference();
            isReverse = true;
        }
        if (oldVx >= 0.0f && oldVy >= 0.0f
            && vx <= 0.0f && vy <= 0.0f)
        {
            StickDifference();
            isReverse = true;
        }
        if (oldVx <= 0.0f && oldVy >= 0.0f
            && vx >= 0.0f && vy <= 0.0f)
        {
            StickDifference();
            isReverse = true;
        }
        if (oldVx <= 0.0f && oldVy <= 0.0f
            && vx >= 0.0f && vy >= 0.0f)
        {
            StickDifference();
            isReverse = true;
        }
    }

    /// <summary>
    /// スティック入力値の差を計算
    /// </summary>
    private void StickDifference()
    {
        float vxAbs = Mathf.Abs(vx);
        float vyAbs = Mathf.Abs(vy);
        float oxAbs = Mathf.Abs(oldVx);
        float oyAbs = Mathf.Abs(oldVy);

        d_Vx = oxAbs - vxAbs;
        d_Vy = oyAbs - vyAbs;

        d_Stick = (Mathf.Abs(d_Vx) + Mathf.Abs(d_Vy));
    }

    /// <summary>
    /// 後退切り替え
    /// </summary>
    public void Recession()
    {
        if (!isReverse) return;

        p_Class.Recession();

        lj_Class.SpeedChange(d_Stick * 10);
        rj_Class.SpeedChange(d_Stick * 10);

        isReverse = false;
    }

    /// <summary>
    /// 反転判定設定
    /// </summary>
    /// <param name="isReverse"></param>
    public void SetIsReverse(bool isReverse)
    {
        this.isReverse = isReverse;
    }

    /// <summary>
    /// 反転判定取得
    /// </summary>
    /// <returns>反転判定</returns>
    public bool GetIsReverse()
    {
        return isReverse;
    }
}
