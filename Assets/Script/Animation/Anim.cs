using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Anim : MonoBehaviour
{
    public int flashInterval;
    public bool isFlash;

    private Animator anim;
    private AnimatorStateInfo animState;
    private GameObject child;
    private int flashCnt;
    private int currentCnt;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        animState = anim.GetCurrentAnimatorStateInfo(0);
        flashCnt = 0;
        currentCnt = flashCnt + flashInterval / 2;
    }

    // Update is called once per frame
    void Update()
    {
        AnimDestroy();
        Flash();
    }

    /// <summary>
    /// アニメーションが終了したら消滅
    /// </summary>
    private void AnimDestroy()
    {
        animState = anim.GetCurrentAnimatorStateInfo(0);

        if (animState.normalizedTime >= 1.0f)
        {
            if (child != null)
            {
                Destroy(child);
            }
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 点滅
    /// </summary>
    private void Flash()
    {
        if (!isFlash || Time.timeScale == 0.0f) return;

        flashCnt += 1;
        Image image = GetComponent<Image>();

        if (flashCnt % flashInterval == 0)
        {
            currentCnt = flashCnt + flashInterval / 2;
            image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        if (flashCnt == currentCnt)
        {
            image.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
    }

    /// <summary>
    /// 子オブジェクト設定
    /// </summary>
    /// <param name="child"></param>
    public void SetChild(GameObject child)
    {
        this.child = child;
    }
}
