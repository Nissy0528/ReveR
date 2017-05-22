using System.Collections;
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
    public float rotateSpeed;//回転速度
    public float rotateLimit;//回転制限
    public float rotateBoost;//回転加速
    public float mass;//重さ
    public float crushTime;//敵を潰すまでの時間

    private float maxAngle;//最大回転値
    private float minAngle;//最小回転値
    private float rotateZ;//回転角度変化用（-180～180に）
    private float angleZ;//回転角度更新用
    private float coreRotaZ;//コアの回転角度
    private float cuurentSpeed;//初期回転速度
    private float crushCount;//敵を潰すまでのカウント
    private bool isStart;//スタート判定
    private bool isStop;//回転停止判定

    //↓デバッグ用
    private bool isJointMass;//ジョイントの重さ判定

    // Use this for initialization
    void Start()
    {
        maxAngle = rotateLimit;//最大回転値設定
        minAngle = -rotateLimit;//最小回転値設定

        //右の翼なら
        if (j_Type == JointType.RIGHT)
        {
            rotateSpeed *= -1;//逆回転に
        }

        cuurentSpeed = rotateSpeed;//初期速度設定

        isStart = false;
        isStop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0.0f) return;

        float vx = Input.GetAxis("Horizontal");//横入力値
        float vy = Input.GetAxis("Vertical");//縦入力値

        if ((vx != 0.0f || vy != 0.0f) && !isStart)
        {
            isStart = true;
        }

        StopCount();//回転停止カウント

        Rotate(vx, vy);//回転
    }

    /// <summary>
    /// 回転
    /// </summary>
    private void Rotate(float vx, float vy)
    {
        if (!isStart || isStop) return;

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

        float rotate = core.transform.localEulerAngles.z + transform.localEulerAngles.z;//自分の角度にコアの角度を加算

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
            //反時計周りなら
            if (rotateSpeed < 0)
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

        //角度制限内で回転
        if (isJointMass)
        {
            if (rotateSpeed > 0)
            {
                angleZ = Mathf.Clamp(rotateZ + (rotateSpeed - mass), minAngle, maxAngle);
            }
            if (rotateSpeed < 0)
            {
                angleZ = Mathf.Clamp(rotateZ + (rotateSpeed + mass), minAngle, maxAngle);
            }
        }
        else
        {
            angleZ = Mathf.Clamp(rotateZ + rotateSpeed, minAngle, maxAngle);
        }
        //angleZ = rotateZ + rotateSpeed;

        //Debug.Log(angleZ + ":" + minAngle + ":" + maxAngle);

        AttackChange();//攻撃判定切り替え

        //角度がマイナスなら
        if (angleZ < 0)
        {
            angleZ += 360;//もとに戻す
        }
        transform.rotation = Quaternion.Euler(0, 0, angleZ);//角度更新

    }

    /// <summary>
    /// 回転速度変更（切り替えし時）
    /// </summary>
    /// <param name="difference"></param>
    public void SpeedChange(float difference)
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
    /// 回転停止カウント
    /// </summary>
    private void StopCount()
    {
        //回転停止しないなら
        if (!isStop) return;//何もしない

        //回転停止カウント開始
        crushCount += 1.0f;
        //回転停止カウントが指定時間になったら
        if (crushCount >= crushTime)
        {
            isStop = false;//停止解除
            crushCount = 0.0f;//カウント初期化
        }
    }

    /// <summary>
    /// 回転停止判定設定
    /// </summary>
    /// <param name="isStop">回転停止判定</param>
    public void SetIsStop(bool isStop)
    {
        this.isStop = isStop;
    }

    /// <summary>
    /// 回転停止判定取得
    /// </summary>
    /// <returns>回転停止判定</returns>
    public bool IsStop()
    {
        return isStop;
    }

    //↓デバッグ用

    /// <summary>
    /// 重さ判定設定
    /// </summary>
    /// <param name="isJointMass">重さ判定</param>
    public void SetJMass(bool isJointMass)
    {
        this.isJointMass = isJointMass;
    }
}
