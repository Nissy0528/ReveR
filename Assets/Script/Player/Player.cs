using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject inputManager;//スティック入力
    public GameObject damageEffect;//ダメージエフェクト
    public GameObject drain;//エネルギー吸収エフェクト
    public GameObject[] spriteObjs;//スプライトレンダラーが入ってるオブジェクト
    public GameObject[] SE;//効果音
    public Sprite[] sprites;//プレイヤーの画像
    public float speed;//移動速度
    public float subSpeed;//減速速度
    public float subMin;//減速速度の最小値
    public float speedLimit;//速度上限
    public float speedMin;//速度最小値
    public float rotateSpeed;//回転速度
    public float returnSpeed;//切り替えし加速度
    public float blowSpeed;//ダメージ時に吹き飛ぶ速度
    public int damageTime;//ダメージ表現の長さ
    public int flashInterval;//点滅間隔
    public int damage;//lifeをマイナスする値
    public int shakeCnt;//コントローラーの振動時間

    public List<TrailRenderer> WingTrail;//両翼のオブジェクトを取る
    public List<Sprite> CoreSprite;//コアの画像
    public List<Material> WingSprite;//翼のエフェクトの画像
    private GameObject[] trails;
    private Rigidbody2D rigid;//リジッドボディ
    private Vector3 lookPos;//見る座標
    private Vector2 vec;//見る方向
    private Color iniTrailColor;
    private GameObject moveSE;
    private int direc;//方向
    private int s_Cnt;//コントローラー振動時間カウント
    private int damageCnt;//ダメージ表現カウント
    private float vx;//横スティック入力値
    private float vy;//縦スティック入力値
    private float r_Speed;//切り替えし加速度
    private float iniSpeed;//初期速度
    private bool isRecession;//反転判定
    private bool isStart;//スタート判定
    private bool isStop;//停止判定
    private bool isJudge;//スティック入力差計算判定
    private bool isReturn;//切り替えし判定
    public static bool isDamage;//ダメージ判定


    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();//リジッドボディ取得
        trails = GameObject.FindGameObjectsWithTag("Trail");
        iniTrailColor = trails[0].GetComponent<TrailRenderer>().material.color;

        isRecession = false;
        isStart = false;
        isStop = false;
        isJudge = false;
        isDamage = false;

        iniSpeed = speed;
        direc = 1;
        r_Speed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputManager.GetComponent<InputManager>().isAI)
        {
            vx = Input.GetAxis("Horizontal");
            vy = Input.GetAxis("Vertical");
        }

        if (Time.timeScale == 0.0f) return;

        //ダメージを受けてないか
        if (!isDamage || blowSpeed == 0)
        {
            Move();//移動
            ReturnForce(returnSpeed);//切り替えし慣性
            Rotate();//回転
        }
        DamageEffect();//ダメージ表現（点滅）
        MoveSE();//移動効果音
        ChangeColor();//色変更

        if (s_Cnt > 0)
        {
            s_Cnt -= 1;
        }
        else
        {
            ControllerShake.Shake(0.0f, 0.0f);
        }

        if (speed > speedLimit)
        {
            speed = speedLimit;
        }

    }

    /// <summary>
    /// 移動（慣性なし）
    /// </summary>
    private void Move()
    {
        //スティック入力されてなければ
        if (vx < 0.5f && vx > -0.5f
            && vy < 0.5f && vy > -0.5f)
        {
            rigid.drag = 1.5f;
            return;//何もしない
        }

        Boost();

        float subRatio = (speed / iniSpeed);

        if (subRatio <= subMin)
        {
            subRatio = subMin;
        }

        if (speed > 0.0f)
        {
            speed -= subSpeed * subRatio;
            if (speed <= speedMin)
            {
                speed = speedMin;
            }
        }

        rigid.drag = 0;
        rigid.velocity = transform.up * (speed * r_Speed);
    }

    /// <summary>
    /// ダッシュ
    /// </summary>
    private void Boost()
    {
        if (!inputManager.GetComponent<InputManager>().isAI)
        {
            //Aボタンが押されていたら
            if (Input.GetKey(KeyCode.JoystickButton0))
            {
                speed = speedLimit;//加速

            }
            else
            {
                speed = iniSpeed;
            }
        }
    }

    /// <summary>
    /// 切り替えし時の慣性
    /// </summary>
    private void ReturnForce(float speed)
    {
        if (!isReturn) return;

        if (direc > 0.0f)
        {
            r_Speed -= speed;
            if (r_Speed <= -1.0f)
            {
                r_Speed = -1.0f;
                isReturn = false;
            }
        }
        if (direc < 0.0f)
        {
            r_Speed += speed;
            if (r_Speed >= 1.0f)
            {
                r_Speed = 1.0f;
                isReturn = false;
            }
        }
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

        vec = (lookPos - transform.position).normalized;//見る座標を計算して正規化
        float angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;//角度計算
        Quaternion newRota = Quaternion.Euler(0.0f, 0.0f, angle);//見る角度設定
        transform.rotation = Quaternion.Slerp(transform.rotation, newRota, rotateSpeed);//設定した方向にゆっくり向く
    }

    /// <summary>
    /// 反転
    /// </summary>
    public void Recession()
    {
        if (isStart)
        {
            direc *= -1;
            isReturn = true;
        }
        else
        {
            r_Speed *= -1;
            isStart = true;
        }
        isRecession = !isRecession;//反転判定を設定
        Vector2 pos = transform.position;//自分の座標
        //反転しなければ
        if (!isRecession)
        {
            lookPos = pos + Vector2.left * vx + Vector2.up * vy;//指定した座標を見る
            if (blowSpeed > 0)
            {
                blowSpeed *= -1;
            }
        }
        //反転していれば
        else
        {
            lookPos = pos + Vector2.left * -vx + Vector2.up * -vy;//指定した座標の反対を見る
            if (blowSpeed < 0)
            {
                blowSpeed *= -1;
            }
        }

        Vector2 vec = (lookPos - transform.position).normalized;//見る座標を計算して正規化
        float angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;//角度計算
        Quaternion newRota = Quaternion.Euler(0.0f, 0.0f, angle);//見る角度設定
        transform.rotation = newRota;
    }

    /// <summary>
    /// 停止判定
    /// </summary>
    /// <returns></returns>
    public bool IsStop()
    {
        return speed <= 0.0f;
    }

    /// <summary>
    /// 反転判定取得
    /// </summary>
    /// <returns></returns>
    public bool IsRecession()
    {
        return isRecession;
    }

    /// <summary>
    /// 敵消滅効果音再生
    /// </summary>
    private void EnemyDeadSE(int seNum)
    {
        Instantiate(SE[seNum]);
    }

    /// <summary>
    /// 潰し処理
    /// </summary>
    public void Crush(int seNum)
    {
        EnemyDeadSE(seNum);
        //speed = Mathf.Min(speed + addSpeed, speedLimit);

        ControllerShake.Shake(1.0f, 1.0f);
        s_Cnt = shakeCnt;

        SpawnDrain();

        //if (speed >= stopSpeed)
        //{
        //    main.GetComponent<Main>().SetStop();
        //}

        isJudge = true;
    }

    /// <summary>
    /// ドレインエフェクト生成
    /// </summary>
    private void SpawnDrain()
    {
        Instantiate(drain, gameObject.transform);
    }

    /// <summary>
    /// ダメージ表現
    /// </summary>
    private void DamageEffect()
    {
        if (!isDamage) return;

        damageCnt += 1;
        int changeCnt = (damageCnt - 1) / flashInterval;

        if (changeCnt % 2 == 0 || changeCnt == 0)
        {
            SetSprite(1.0f);
        }
        else
        {
            SetSprite(0.0f);
        }

        if (damageCnt >= damageTime || SceneManager.GetActiveScene().name != "Main")
        {
            SetSprite(1.0f);
            isDamage = false;
        }

    }

    /// <summary>
    /// 停止
    /// </summary>
    private void Stop()
    {
        GameObject main = GameObject.Find("MainManager");

        if (main == null) return;

        isStop = main.GetComponent<Main>().IsStop();
    }

    /// <summary>
    /// プレイヤー全体の透明度設定
    /// </summary>
    /// <param name="alpha">画像の透明度</param>
    private void SetSprite(float alpha)
    {
        if (alpha == 1.0f)
        {
            for (int i = 0; i < spriteObjs.Length; i++)
            {
                if (i < 4)
                {
                    spriteObjs[i].GetComponent<SpriteRenderer>().sprite = sprites[i];
                }
                else
                {
                    spriteObjs[i].GetComponent<SpriteRenderer>().sprite = sprites[i - 3];
                }
            }

            foreach (var trail in trails)
            {
                trail.GetComponent<TrailRenderer>().material.color = iniTrailColor;
            }
        }
        else
        {
            for (int i = 0; i < spriteObjs.Length; i++)
            {
                if (i < 4)
                {
                    spriteObjs[i].GetComponent<SpriteRenderer>().sprite = sprites[i + 4];
                }
                else
                {
                    spriteObjs[i].GetComponent<SpriteRenderer>().sprite = sprites[i + 1];
                }
            }

            foreach (var trail in trails)
            {
                trail.GetComponent<TrailRenderer>().material.color = Color.white;
            }
        }
    }

    /// <summary>
    /// 移動効果音
    /// 速度に合わせて音が変わるように
    /// </summary>
    private void MoveSE()
    {
        //効果音が生成してなければ効果音生成
        if (moveSE == null)
        {
            moveSE = Instantiate(SE[2]);
        }
        else
        {
            AudioSource m_SE = moveSE.GetComponent<AudioSource>();
            float velocity = rigid.velocity.magnitude;
            m_SE.volume = velocity / speedLimit;//速度に合わせて音量を変える
            m_SE.pitch = (velocity * 1.5f) / speedLimit;//速度に合わせてピッチを変える
        }

    }

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.gameObject.tag == "Enemy" || col.gameObject.tag == "Shield") && !isDamage)//EnemyとShieldとぶつかった時
        {

            GameObject main = GameObject.Find("MainManager");

            if (main.GetComponent<Main>().GetWave() == 0) return;//チュートリアル中はダメージなし

            //敵にぶつかったら通常のダメージ音再生
            if (col.gameObject.name.Contains("Enemy"))
            {
                Instantiate(SE[1]);
            }
            //それ以外に当たったら特殊ダメージ音再生
            else
            {
                Instantiate(SE[5]);
            }
            ControllerShake.Shake(1.0f, 1.0f);//コントローラーを振動させる
            GameObject.Find("Main Camera").GetComponent<MainCamera>().SetShake();//カメラを振動させる
            s_Cnt = shakeCnt;//振動時間設定

            //ダメージを受ける
            speed = Mathf.Max(speed - damage, 0.0f);
            GameObject p_Effect = Instantiate(damageEffect, transform.position, transform.rotation);
            p_Effect.GetComponent<PTimeEffectSpawner>().SetAddTime(damage, main.GetComponent<Main>().lifeTime, 1);

            damageCnt = 0;
            BlowOff();
            isDamage = true;//ダメージフラグtrue
        }
    }

    /// <summary>
    /// 吹き飛ぶ
    /// </summary>
    private void BlowOff()
    {
        float value = 1.0f;
        if (isReturn)
        {
            value = -1.0f;
        }
        ReturnForce(1.0f);
        rigid.drag = 1.5f;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(vec * blowSpeed * value, ForceMode2D.Impulse);//後ろに吹き飛ぶ
        value = 1.0f;
    }

    /// <summary>
    /// スティック入力差の計算判定取得
    /// </summary>
    /// <returns>スティック入力差の計算判定</returns>
    public bool IsJudge()
    {
        return isJudge;
    }

    /// <summary>
    /// スティック入力差の計算判定設定
    /// </summary>
    /// <param name="isJudge">スティック入力差の計算判定</param>
    public void SetIsJudge(bool isJudge)
    {
        this.isJudge = isJudge;
    }

    /// <summary>
    /// コアと翼の色を変更
    /// </summary>
    public void ChangeColor()
    {
        if (isDamage) return;

        if (SceneManager.GetActiveScene().name != "Main")
        {
            LifeScript.IsGreen = true;
            LifeScript.IsRed = false;
            LifeScript.IsYellow = false;
        }
        if (LifeScript.IsGreen == true)
        {
            GetComponent<SpriteRenderer>().sprite = CoreSprite[0];
            WingTrail[0].material = WingSprite[0];
            WingTrail[1].material = WingSprite[0];
        }
        if (LifeScript.IsRed == true)
        {
            GetComponent<SpriteRenderer>().sprite = CoreSprite[1];
            WingTrail[0].material = WingSprite[1];
            WingTrail[1].material = WingSprite[1];
        }

        if (LifeScript.IsYellow == true)
        {
            GetComponent<SpriteRenderer>().sprite = CoreSprite[2];
            WingTrail[0].material = WingSprite[2];
            WingTrail[1].material = WingSprite[2];
        }
        sprites[0] = GetComponent<SpriteRenderer>().sprite;

    }

    /// <summary>
    /// ダメージフラグ
    /// </summary>
    /// <returns></returns>
    public bool IsDamage()
    {
        return isDamage && blowSpeed != 0;
    }
}
