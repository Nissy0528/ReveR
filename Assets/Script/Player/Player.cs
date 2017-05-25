using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject inputManager;//スティック入力
    public LifeScript lifeScript;//LifeScript
    public Slider speedSlider;
    public float speed;//移動速度
    public float addSpeed;//加速速度
    public float subSpeed;//減速速度
    public float speedLimit;//速度上限
    public float rotateSpeed;//回転速度
    public float drag;//移動終了時の慣性
    public float addValue;//加速度加算値
    public float r_Boost;//反転時加速度
    public float bInvalidValue;//反転加速度を減速する値
    public float returnSpeed;//切り替えし加速度
    public float boostTime;//ブースト開始時間
    public int hp;//体力;
    public int damage;//lifeをマイナスする値
    public int maxexp;

    private Rigidbody2D rigid;//リジッドボディ
    private Vector3 size;//オブジェクトのサイズ
    private Vector3 lookPos;//見る座標
    private AudioSource se;//効果音
    private int direc;//方向
    private float vx;//横スティック入力値
    private float vy;//縦スティック入力値
    private float r_Speed;//切り替えし加速度
    private float iniSpeed;//初期速度
    private float b_Time;//ブースト開始カウント
    private bool isRecession;//反転判定
    private bool isRe;//反転判定（慣性）
    private bool isStart;//スタート判定
    private bool isRBoost;//ブースト準備判定
    private bool isStop;//停止判定

    //↓デバッグ用
    private bool isForce;//慣性判定
    private bool isReturn;//反転時の加速判定
    private bool isDeceleration;//反転時の加速判定
    private bool isRForce;//反転時慣性判定

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();//リジッドボディ取得
        se = GetComponent<AudioSource>();

        isRecession = false;
        isStart = false;
        isStop = false;

        //addSpeed = 0.0f;
        iniSpeed = speed;
        direc = 1;
        r_Speed = 1.0f;

        HpBarCtrl.hpbar = hp;//HpBar取得
    }

    // Update is called once per frame
    void Update()
    {
        vx = Input.GetAxis("Horizontal");
        vy = Input.GetAxis("Vertical");

        if (isStop) return;

        //BoostCount();//ブーストカウント

        if (!isForce)
        {
            Move();//移動（慣性なし）
        }
        else
        {
            ForceMove();//移動（慣性あり）
        }
        Rotate();//回転
    }

    /// <summary>
    /// 移動（慣性なし）
    /// </summary>
    private void Move()
    {
        //if (!isReturn && vx == 0.0f && vy == 0.0f)
        //{
        //    addSpeed -= addValue;
        //}

        //スティック入力されてなければ
        if (vx < 0.5f && vx > -0.5f
            && vy < 0.5f && vy > -0.5f)
        {
            rigid.drag = drag;
            //if (isReturn)
            //{
            //    addSpeed = 0;
            //}
            return;//何もしない
        }

        //if (addSpeed <= 1.0f)
        //{
        //    addSpeed += addValue;
        //}
        //else
        //{
        //    addSpeed = 1.0f;
        //}

        //if (isDeceleration)
        //{
        //    if (speed > 0 && speed > currentSpeed)
        //    {
        //        speed -= bInvalidValue;
        //    }
        //    if (speed < 0 && speed < currentSpeed)
        //    {
        //        speed += bInvalidValue;
        //    }
        //}

        if (isRe)
        {
            if (direc > 0.0f)
            {
                r_Speed -= returnSpeed;
                if (r_Speed <= -1.0f)
                {
                    r_Speed = -1.0f;
                    isRe = false;
                }
            }
            if (direc < 0.0f)
            {
                r_Speed += returnSpeed;
                if (r_Speed >= 1.0f)
                {
                    r_Speed = 1.0f;
                    isRe = false;
                }
            }
        }

        if (speed > 0.0f)
        {
            speed -= subSpeed;
            if (speed <= 0.0f)
            {
                speed = 0.0f;
            }
        }

        rigid.drag = 0;
        rigid.velocity = transform.up * (speed * r_Speed);

        speedSlider.value = Mathf.Abs(speed);
    }

    /// <summary>
    /// 移動（慣性あり）
    /// </summary>
    private void ForceMove()
    {
        if (vx < 0.5f && vx > -0.5f
            && vy < 0.5f && vy > -0.5f)
        {
            return;
        }
        rigid.AddForce(transform.up * speed);//前後移動
    }

    /// <summary>
    /// 回転
    /// </summary>
    private void Rotate()
    {
        //スティック入力されていなけば
        if (vx < 0.5f && vx > -0.5f
            && vy < 0.5f && vy > -0.5f
            || Time.timeScale == 0.0f) return;//何もしない

        Vector2 pos = transform.position;//自分の座標
        //スティックの入力によって見る座標を設定
        //反転しなければ
        if (!isRecession)
        {
            lookPos = pos + Vector2.left * vx + Vector2.up * vy;//指定した座標を見る
        }
        //反転していれば
        else
        {
            lookPos = pos + Vector2.left * -vx + Vector2.up * -vy;//指定した座標の反対を見る
        }

        Vector2 vec = (lookPos - transform.position).normalized;//見る座標を計算して正規化
        float angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;//角度計算
        Quaternion newRota = Quaternion.Euler(0.0f, 0.0f, angle);//見る角度設定
        transform.rotation = Quaternion.Slerp(transform.rotation, newRota, rotateSpeed);//設定した方向にゆっくり向く
    }

    /// <summary>
    /// 反転
    /// </summary>
    public void Recession()
    {
        //speed = iniSpeed;

        if (!isRForce)
        {
            speed *= -1;//速度を逆に
        }
        else
        {
            if (isStart)
            {
                direc *= -1;
                isRe = true;
            }
            else
            {
                r_Speed *= -1;
                isStart = true;
            }
        }
        //iniSpeed *= -1;//初期速度を逆に
        isRecession = !isRecession;//反転判定を設定
        Vector2 pos = transform.position;//自分の座標
        //反転しなければ
        if (!isRecession)
        {
            lookPos = pos + Vector2.left * vx + Vector2.up * vy;//指定した座標を見る
        }
        //反転していれば
        else
        {
            lookPos = pos + Vector2.left * -vx + Vector2.up * -vy;//指定した座標の反対を見る
        }

        Vector2 vec = (lookPos - transform.position).normalized;//見る座標を計算して正規化
        float angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;//角度計算
        Quaternion newRota = Quaternion.Euler(0.0f, 0.0f, angle);//見る角度設定
        transform.rotation = newRota;
    }

    /// <summary>
    /// ブーストカウント
    /// </summary>
    private void BoostCount()
    {
        //ブーストしなければ
        if (!isRBoost) return;//何もしない

        b_Time += Time.deltaTime;//ブースト開始カウント
        //ブーストカウントが指定数以上になったら
        if (b_Time >= boostTime)
        {
            //方向に合わせてブースト
            if (speed > 0)
            {
                speed += r_Boost;
            }
            if (speed < 0)
            {
                speed -= r_Boost;
            }
            isDeceleration = true;//ブースト減速判定
            b_Time = 0.0f;//ブーストカウント初期化
            isRBoost = false;//ブースト準備判定false
        }
    }

    /// <summary>
    /// ジョイント停止
    /// </summary>
    public void StopJoint()
    {
        isStop = true;
        transform.FindChild("L_Joint").GetComponent<Joint>().SetIsStop(true);
        transform.FindChild("R_Joint").GetComponent<Joint>().SetIsStop(true);
    }

    /// <summary>
    /// 潰す判定
    /// </summary>
    /// <returns></returns>
    public bool IsCrush()
    {
        return isStop
            && !transform.FindChild("L_Joint").GetComponent<Joint>().IsStop()
            && !transform.FindChild("R_Joint").GetComponent<Joint>().IsStop();
    }

    /// <summary>
    /// 停止判定
    /// </summary>
    /// <returns></returns>
    public bool IsStop()
    {
        return speed == 0.0f;
    }

    /// <summary>
    /// 敵消滅効果音再生
    /// </summary>
    private void EnemyDeadSE()
    {
        se.PlayOneShot(se.clip);
    }

    /// <summary>
    /// 潰し処理
    /// </summary>
    public void Crush()
    {
        EnemyDeadSE();
        if (speed > 0.0f && speed <= speedLimit)
        {
            speed += addSpeed;
        }
        if (speed < 0.0f && speed >= -speedLimit)
        {
            speed -= addSpeed;
        }
        //isRBoost = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")//Enemyとぶつかった時
        {
            HpBarCtrl.hpbar--;//hpをマイナス
            //if (HpBarCtrl.hpbar == 0)//体力が０になった時
            //{
            //    Destroy(gameObject);オブジェクトを破壊
            //}

            lifeScript.LifeDown(damage);//lifeScriptのlifeDownメソッドを実行
        }
    }

    public void ExtWing()
    {
        transform.FindChild("L_Joint").GetComponent<ExtendWing>().extendWing();
        transform.FindChild("R_Joint").GetComponent<ExtendWing>().extendWing();
    }

    public void LevelUp()
    {
        GetComponent<Exp>().LevelUp(maxexp);
    }

    //↓デバッグ用

    /// <summary>
    /// 加速度設定
    /// </summary>
    /// <param name="addSpeed">加速度</param>
    public void SetAddSpeed(float addSpeed)
    {
        this.addSpeed = addSpeed;
    }
    /// <summary>
    /// 慣性判定設定
    /// </summary>
    /// <param name="isForce">慣性判定</param>
    public void SetForce(bool isForce)
    {
        this.isForce = isForce;
    }
    /// <summary>
    /// 反転時の加速判定設定
    /// </summary>
    /// <param name="isReturn">反転時の加速判定</param>
    public void SetReturn(bool isReturn)
    {
        this.isReturn = isReturn;
    }
    /// <summary>
    /// 反転時加速判定設定
    /// </summary>
    /// <param name="isRBoost">反転時加速判定</param>
    public void SetRBoost(bool isRBoost)
    {
        this.isDeceleration = isRBoost;
    }
    /// <summary>
    /// 反転時慣性判定設定
    /// </summary>
    /// <param name="isRForce"></param>
    public void SetRForce(bool isRForce)
    {
        this.isRForce = isRForce;
    }
}
