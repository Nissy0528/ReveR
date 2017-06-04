using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class Player : MonoBehaviour
{
    public GameObject inputManager;//スティック入力
    public LifeScript lifeScript;//LifeScript
    public GameObject drain;//エネルギー吸収エフェクト
    public GameObject[] spriteObjs;//スプライトレンダラーが入ってるオブジェクト
    public AudioClip[] seClip;//効果音
    public float speed;//移動速度
    public float addSpeed;//加速速度
    public float subSpeed;//減速速度
    public float subMin;//減速速度の最小値
    public float speedLimit;//速度上限
    public float drag;//摩擦力
    public float rotateSpeed;//回転速度
    public float returnSpeed;//切り替えし加速度
    public float rayLength;//レイの長さ
    public int damageTime;//ダメージ表現の長さ
    public int flashInterval;//点滅間隔
    public int damage;//lifeをマイナスする値
    public int maxexp;//経験値
    public int shakeCnt;//コントローラーの振動時間

    private Rigidbody2D rigid;//リジッドボディ
    private Vector3 size;//オブジェクトのサイズ
    private Vector3 lookPos;//見る座標
    private AudioSource se;//効果音
    private Ray2D ray;//レイ
    private RaycastHit2D hit;//レイが当たったオブジェクトの情報
    private GameObject[] trails;
    private Color iniTrailColor;
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
    private bool isDamage;//ダメージ判定

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();//リジッドボディ取得
        se = GetComponent<AudioSource>();
        trails = GameObject.FindGameObjectsWithTag("Trail");
        iniTrailColor = trails[0].GetComponent<TrailRenderer>().material.color;

        isRecession = false;
        isStart = false;
        isStop = false;
        isJudge = false;

        iniSpeed = speed;
        direc = 1;
        r_Speed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        vx = Input.GetAxis("Horizontal");
        vy = Input.GetAxis("Vertical");

        if (isStop) return;

        Move();//移動
        ReturnForce();//切り替えし慣性
        Rotate();//回転
        Ray();//レイのあたり判定
        DamageEffect();//ダメージ表現（点滅）

        if (s_Cnt > 0)
        {
            s_Cnt -= 1;
        }
        else
        {
            ControllerShake(0.0f, 0.0f);
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
            return;//何もしない
        }

        float subRatio = (speed / iniSpeed);

        if (subRatio <= subMin)
        {
            subRatio = subMin;
        }

        if (speed > 0.0f)
        {
            speed -= subSpeed * subRatio;
            if (speed <= 0.0f)
            {
                speed = 0.0f;
            }
        }

        rigid.drag = 0;
        rigid.velocity = transform.up * (speed * r_Speed);
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
        se.PlayOneShot(seClip[0]);
    }

    /// <summary>
    /// 潰し処理
    /// </summary>
    public void Crush()
    {
        EnemyDeadSE();
        if (speed <= speedLimit)
        {
            speed += addSpeed;
        }

        ControllerShake(1.0f, 1.0f);
        s_Cnt = shakeCnt;

        SpawnDrain();

        isJudge = true;
    }

    /// <summary>
    /// ドレインエフェクト生成
    /// </summary>
    private void SpawnDrain()
    {
        GameObject L_Tip = GameObject.Find("L_WingTip");
        GameObject R_Tip = GameObject.Find("R_WingTip");

        GameObject L_drain = Instantiate(drain, L_Tip.transform.position, L_Tip.transform.rotation, L_Tip.transform);
        GameObject R_drain = Instantiate(drain, R_Tip.transform.position, R_Tip.transform.rotation, R_Tip.transform);

        L_drain.GetComponent<Drain>().SetHingeName("L_Hinge");
        R_drain.GetComponent<Drain>().SetHingeName("R_Hinge");
    }

    /// <summary>
    /// コントローラー振動
    /// </summary>
    private void ControllerShake(float left, float right)
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerIndex pI = (PlayerIndex)i;
            GamePadState state = GamePad.GetState(pI);
            if (state.IsConnected)
            {
                GamePad.SetVibration(pI, left, right);
            }
        }
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

        if (damageCnt >= damageTime)
        {
            SetAlpha(1.0f);
            isDamage = false;
        }
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
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && !isDamage)//Enemyとぶつかった時
        {

            se.PlayOneShot(seClip[1]);

            speed = Mathf.Max(speed - damage, 0, 0f);

            GameObject tutorial = GameObject.Find("Tutorial");
            if (tutorial != null)
            {
                tutorial.GetComponent<TutoUISpawner>().SetIsDamage();
            }

            damageCnt = 0;
            isDamage = true;
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
}
