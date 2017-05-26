using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public GameObject player;//プレイヤー
    public GameObject L_joint;//左ジョイント
    public GameObject R_joint;//右ジョイント
    public Slider slider;//スライダー
    public float delay;

    private Vector2 input;//スティック入力値
    private Vector2 oldInput;//前のスティック入力値
    private Vector2 invertInput;//スティック入力値の逆値
    private Player p_Class;//プレイヤークラス
    private Joint lj_Class;//左ジョイントクラス
    private Joint rj_Class;//右ジョイントクラス
    private float inputDefference;//スティック入力差
    private float dot;//スティックの入力差
    private float d_Cnt;
    private bool isInvert;//反転判定
    private bool isStart;//スタート判定

    //デバッグ用↓
    public GameObject oldPoint;
    public GameObject currentPoint;
    public GameObject coPoint;
    public GameObject invertPoint;
    public GameObject InvertLine;
    public GameObject CurrentLine;

    private Vector3 oldIniPos;
    private Vector3 currentIniPos;
    private Vector3 invertIniPos;

    // Use this for initialization
    void Start()
    {
        isInvert = false;
        p_Class = player.GetComponent<Player>();
        lj_Class = L_joint.GetComponent<Joint>();
        rj_Class = R_joint.GetComponent<Joint>();

        oldIniPos = oldPoint.transform.position;
        currentIniPos = currentPoint.transform.position;
        invertIniPos = invertPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0.0f) return;

        input.x = Input.GetAxis("Horizontal") * -1;
        input.y = Input.GetAxis("Vertical");

        InputPoint();//スティック入力座標取得
        Invert();//反転処理

        //デバッグ用↓
        InvertLine.GetComponent<LineRenderer>().SetPosition(0, oldPoint.transform.position);
        InvertLine.GetComponent<LineRenderer>().SetPosition(1, invertPoint.transform.position);
        CurrentLine.GetComponent<LineRenderer>().SetPosition(0, oldPoint.transform.position);
        CurrentLine.GetComponent<LineRenderer>().SetPosition(1, coPoint.transform.position);
    }

    /// <summary>
    /// 反転処理
    /// </summary>
    private void Invert()
    {
        if (!isInvert || !IsInput()) return;

        if (IsInvert())
        {
            InputDefference();
            Recession();
            coPoint.transform.position = new Vector3(currentIniPos.x + input.x, currentIniPos.y + input.y, currentIniPos.z);
            d_Cnt = delay;
        }
        isInvert = false;
    }

    /// <summary>
    /// 後退切り替え
    /// </summary>
    private void Recession()
    {
        p_Class.Recession();

        lj_Class.SpeedChange();
        rj_Class.SpeedChange();
    }

    /// <summary>
    /// スティック入力座標取得
    /// </summary>
    private void InputPoint()
    {
        //最初の入力がされてなけば
        if (!isStart)
        {
            //スティックが入力されたら
            if (IsInput())
            {
                isStart = true;//開始判定true
            }
            return;//何もしない
        }

        if (d_Cnt > 0.0f)
        {
            d_Cnt -= Time.deltaTime;
        }
        else
        {
            d_Cnt = 0.0f;
        }

        //スティックが入力されなくなったら
        if (IsInput() && !isInvert)
        {
            oldInput = new Vector2(input.x, input.y);//入力されてたスティック座標を取得
        }
        else
        {
            d_Cnt = 0.0f;
            isInvert = true;
        }

        invertInput = new Vector2(oldInput.x * -1, oldInput.y * -1);//入力されたティックの逆値

        //デバッグ用↓
        currentPoint.transform.position = new Vector3(currentIniPos.x + input.x, currentIniPos.y + input.y, currentIniPos.z);
        if (d_Cnt == 0.0f)
        {
            coPoint.transform.position = new Vector3(currentIniPos.x + input.x, currentIniPos.y + input.y, currentIniPos.z);
            oldPoint.transform.position = new Vector3(oldIniPos.x + oldInput.x, oldIniPos.y + oldInput.y, oldIniPos.z);
            invertPoint.transform.position = new Vector3(invertIniPos.x + invertInput.x, invertIniPos.y + invertInput.y, invertIniPos.z);
        }
    }

    /// <summary>
    /// スティック入力差をスライダーに表示
    /// </summary>
    private void InputDefference()
    {
        float dotAbs = Mathf.Abs(dot);

        slider.value = dotAbs;

        //if (dotAbs <= 0.3f)
        //{
        //    slider.value = 0.3f;
        //}
        //if (dotAbs > 0.3f && dotAbs <= 0.6f)
        //{
        //    slider.value = 0.6f;
        //}
        //if (dotAbs > 0.6f)
        //{
        //    slider.value = 1.0f;
        //}
    }

    /// <summary>
    /// 入力判定
    /// </summary>
    /// <returns>スティックが入力されたらtrue</returns>
    private bool IsInput()
    {
        return input.x >= 0.6f || input.x <= -0.6f
            || input.y >= 0.6f || input.y <= -0.6f;
    }

    /// <summary>
    /// 逆入力判定
    /// </summary>
    /// <returns>逆入力の範囲にスティックが入力されたらtrue</returns>
    private bool IsInvert()
    {
        dot = Vector2.Dot(oldInput.normalized, input.normalized);

        return dot < 0.0f;
    }
}
