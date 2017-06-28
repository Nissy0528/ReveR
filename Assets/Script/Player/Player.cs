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
    public float speed;//移動速度
    public float addSpeed;//加速速度
    public float subSpeed;//減速速度
    public float subMin;//減速速度の最小値
    public float speedLimit;//速度上限
    public float speedMin;
    public float stopSpeed;//画面が停止する速度
    public float drag;//摩擦力
    public float rotateSpeed;//回転速度
    public float returnSpeed;//切り替えし加速度
    public int damageTime;//ダメージ表現の長さ
    public int flashInterval;//点滅間隔
    public int damage;//lifeをマイナスする値
    public int maxexp;//経験値
    public int shakeCnt;//コントローラーの振動時間

    public List<TrailRenderer> WingTrail;//両翼のオブジェクトを取る
    public List<Sprite> CoreSprite;//コアの画像
    public List<Material> WingSprite;//翼のエフェクトの画像
    private Rigidbody2D rigid;//リジッドボディ
    private Vector3 lookPos;//見る座標
    private Ray2D ray;//レイ
    private RaycastHit2D hit;//レイが当たったオブジェクトの情報
    private GameObject[] trails;
    private Color iniTrailColor;
    private GameObject moveSE;
    private int direc;//方向
    private int s_Cnt;//コントローラー振動時間カウント
    private int damageCnt;//ダメージ表現カウント
    private float vx;//横スティック入力値
    private float vy;//縦スティック入力値
    private float r_Speed;//切り替えし加速度
    private float iniSpeed;//初期速度
    private float rayLength;//レイの長さ
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
        else
        {
            vx = inputManager.GetComponent<AI_Input>().GetInput().x;
            vy = inputManager.GetComponent<AI_Input>().GetInput().y;
        }

        if (Time.timeScale == 0.0f) return;
        Move();//移動
        ReturnForce();//切り替えし慣性
        Rotate();//回転
        Ray();//レイのあたり判定
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
        else
        {
            if (inputManager.GetComponent<AI_Input>().GetIsBoost())
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
    /// 移動（慣性あり）
    /// </summary>
    private void ReturnForce()
    {
        if (!isReturn) return;

        if (direc > 0.0f)
        {
            r_Speed -= returnSpeed;
            if (r_Speed <= -1.0f)
            {
                r_Speed = -1.0f;
                isReturn = false;
            }
        }
        if (direc < 0.0f)
        {
            r_Speed += returnSpeed;
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
    private void EnemyDeadSE()
    {
        Instantiate(SE[0]);
    }

    /// <summary>
    /// 潰し処理
    /// </summary>
    public void Crush()
    {
        EnemyDeadSE();
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
    /// レイ生成
    /// </summary>
    private void Ray()
    {
        if (GameObject.Find("L_Wing").tag != "Untagged" && GameObject.Find("R_Wing").tag != "Untagged")
        {
            if (!isRecession)
            {
                ray = new Ray2D(transform.position, -transform.up);
            }
            else
            {
                ray = new Ray2D(transform.position, transform.up);
            }
        }
        else
        {
            ray = new Ray2D(Vector2.zero, Vector2.zero);
        }
        hit = Physics2D.Raycast(ray.origin, ray.direction * rayLength);
        UnityEngine.Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
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
            SetAlpha(1.0f);
        }
        else
        {
            SetAlpha(0.0f);
        }

        if (damageCnt >= damageTime || SceneManager.GetActiveScene().name != "Main")
        {
            SetAlpha(1.0f);
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
    private void SetAlpha(float alpha)
    {
        foreach (var sp in spriteObjs)
        {
            sp.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }

        if (alpha == 1.0f)
        {
            foreach (var trail in trails)
            {
                trail.GetComponent<TrailRenderer>().material.color = iniTrailColor;
            }
        }
        else
        {
            foreach (var trail in trails)
            {
                trail.GetComponent<TrailRenderer>().material.color = new Color(iniTrailColor.r, iniTrailColor.g, iniTrailColor.b, 0.0f);
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
        if (col.gameObject.tag == "Enemy" && !isDamage || col.gameObject.tag == "Shield")//EnemyとShieldとぶつかった時
        {

            GameObject tutorial = GameObject.Find("Tutorial");
            GameObject main = GameObject.Find("MainManager");
            if ((tutorial != null && tutorial.GetComponent<TutoUISpawner>().IsDamage())
                || tutorial == null)
            {
                //tutorial.GetComponent<TutoUISpawner>().SetIsDamage();

                Instantiate(SE[1]);
                ControllerShake.Shake(1.0f, 1.0f);
                GameObject.Find("Main Camera").GetComponent<MainCamera>().SetShake();
                s_Cnt = shakeCnt;

                speed = Mathf.Max(speed - damage, 0.0f);
                GameObject p_Effect = Instantiate(damageEffect, transform.position, transform.rotation); ;
                p_Effect.GetComponent<PTimeEffectSpawner>().SetAddTime(damage, main.GetComponent<Main>().lifeTime, 1);

                damageCnt = 0;
                isDamage = true;
            }
        }
    }

    /// <summary>
    /// ジョイントを伸ばす
    /// </summary>
    public void ExtWing()
    {
        transform.FindChild("L_Joint").GetComponent<ExtendWing>().extendWing();
        transform.FindChild("R_Joint").GetComponent<ExtendWing>().extendWing();
    }

    /// <summary>
    /// 経験値加算
    /// </summary>
    public void LevelUp()
    {
        GetComponent<Exp>().LevelUp(maxexp);
    }

    /// <summary>
    /// 敵が正面にいるかの判定
    /// </summary>
    public bool RayHit()
    {
        if (hit.collider != null && hit.collider.tag == "Enemy")
        {
            return true;
        }
        return false;
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

    }
}
