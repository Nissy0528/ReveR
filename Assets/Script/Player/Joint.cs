﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : MonoBehaviour
{
    //どちら側のジョイントか
    public enum JointType
    {
        LEFT,
        RIGHT
    }

    public JointType j_Type;//どちら側のジョイントか
    public GameObject core;//コアオブジェクト
    public GameObject inputManager;
    public float rotateSpeed;//回転速度
    public float rotateLimit;//回転制限
    public float rotateBoost;//回転加速
    public float mass;//重さ

    private float vx;
    private float vy;
    private float maxAngle;//最大回転値
    private float minAngle;//最小回転値
    private float rotateZ;//回転角度変化用（-180～180に）
    private float angleZ;//回転角度更新用
    private float coreRotaZ;//コアの回転角度
    private float cuurentSpeed;//初期回転速度
    private float rotate;
    private float iniLimit;//初期の回転制限
    private bool isStart;//スタート判定
    private bool isChangeLimit;//回転制限変更フラグ

    // Use this for initialization
    void Start()
    {
        maxAngle = rotateLimit;//最大回転値設定
        minAngle = -rotateLimit;//最小回転値設定
        iniLimit = rotateLimit;//初期制限設定

        //右の翼なら
        if (j_Type == JointType.RIGHT)
        {
            rotateSpeed *= -1;//逆回転に
        }

        cuurentSpeed = rotateSpeed;//初期速度設定

        isStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0.0f) return;

        if (!inputManager.GetComponent<InputManager>().isAI)
        {
            vx = Input.GetAxis("Horizontal");//横入力値
            vy = Input.GetAxis("Vertical");//縦入力値
        }

        if ((vx != 0.0f || vy != 0.0f) && !isStart)
        {
            isStart = true;
        }

        Rotate(vx, vy);//回転
    }

    /// <summary>
    /// 回転
    /// </summary>
    private void Rotate(float vx, float vy)
    {
        if (!isStart || (core.GetComponent<Player>().IsDamage() && transform.GetChild(0).tag == "Untagged")) return;

        //コアの角度が180度超えていたら
        if (core.transform.localEulerAngles.z > 180)
        {
            coreRotaZ = core.transform.localEulerAngles.z - 360;//マイナスに変換
        }
        //それ以外は
        else
        {
            coreRotaZ = core.transform.localEulerAngles.z;//現在の角度を代入
        }

        //コアの角度から最小と最大の角度制限を更新
        minAngle = coreRotaZ + (-rotateLimit);
        maxAngle = coreRotaZ + rotateLimit;

        rotate = core.transform.localEulerAngles.z + transform.localEulerAngles.z;//自分の角度にコアの角度を加算

        //180度超えていたら
        if (rotate > 180 && maxAngle <= 180 && minAngle >= -rotateLimit)
        {
            rotateZ = rotate - 360;//マイナスに変換
        }
        //それ以外は
        else
        {
            rotateZ = rotate;//現在の角度を代入
        }

        //右向きの場合の制限更新
        //最大角度が180度より大きくかつ自分の角度が360度を超えていたら
        if (maxAngle > 180 && rotate > 360)
        {
            rotateZ = rotate - 360;//360度以内に変換
        }
        //最小角度が‐75度より小さかったら
        if (minAngle < -rotateLimit)
        {
            //時計回りなら
            if (rotateSpeed > 0)
            {
                if (core.transform.localEulerAngles.z >= 180 && core.transform.localEulerAngles.z <= 210)
                {
                    if (rotate <= 565 && rotate >= 453)
                    {
                        rotateZ = rotate - 360 * 2;
                    }
                    else
                    {
                        rotateZ = rotate - 360;
                    }
                }
                else
                {
                    //自分の角度が480度超えていたら
                    if (rotate > 480)
                    {
                        //マイナスに変換
                        rotateZ = rotate - 360 * 2;
                    }
                    //超えていなければ
                    else
                    {
                        //360度以内に変換
                        rotateZ = rotate - 360;
                    }
                }
            }
            //反時計周りなら
            if (rotateSpeed < 0)
            {
                if (core.transform.localEulerAngles.z > 270)
                {
                    if (rotate > 543)
                    {
                        rotateZ = rotate - 360 * 2;
                    }
                    else
                    {
                        rotateZ = rotate - 360;
                    }
                }
                else
                {
                    //自分の角度が360度超えていたら
                    if (rotate > 360)
                    {
                        //マイナスに変換
                        rotateZ = rotate - 360 * 2;
                    }
                    //超えていなければ
                    else
                    {
                        //360度以内に変換
                        rotateZ = rotate - 360;
                    }
                }
            }

        }

        //角度制限内で回転
        angleZ = Mathf.Clamp(rotateZ + rotateSpeed, minAngle, maxAngle);

        //angleZ = rotateZ + rotateSpeed;
        //Debug.Log(rotate + ":" + minAngle + ":" + maxAngle);

        AttackChange();//攻撃判定切り替え

        //角度がマイナスなら
        if (angleZ < 0)
        {
            angleZ += 360;//もとに戻す
        }
        transform.rotation = Quaternion.Euler(0, 0, angleZ);//角度更新

        if ((angleZ <= minAngle || angleZ >= maxAngle) && isChangeLimit)
        {
            isChangeLimit = false;
        }

    }

    /// <summary>
    /// 回転速度変更（切り替えし時）
    /// </summary>
    public void SpeedChange()
    {
        //回転速度初期化
        rotateSpeed = cuurentSpeed;

        rotateSpeed *= -1;//逆回転に
        cuurentSpeed *= -1;//初期速度も逆に
    }

    /// <summary>
    ///　攻撃判定切り替え
    /// </summary>
    private void AttackChange()
    {
        if (angleZ <= minAngle || angleZ >= maxAngle)
        {
            transform.GetChild(0).tag = "Untagged";
            return;
        }

        if (j_Type == JointType.LEFT)
        {
            transform.FindChild("L_Wing").tag = "L_Joint";
        }
        if (j_Type == JointType.RIGHT)
        {
            transform.FindChild("R_Wing").tag = "R_Joint";
        }
    }

    /// <summary>
    /// 通常状態判定
    /// </summary>
    /// <returns></returns>
    public bool IsUntagged()
    {
        return transform.GetChild(0).tag == "Untagged";
    }
}
