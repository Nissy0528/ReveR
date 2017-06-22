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
    private int flashCnt;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        animState = anim.GetCurrentAnimatorStateInfo(0);
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
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 点滅
    /// </summary>
    private void Flash()
    {
        if (!isFlash) return;

        flashCnt += (int)(1 * Time.timeScale);
        Image image = GetComponent<Image>();

        if ((flashCnt - 1) / flashInterval % 2 == 0)
        {
            image.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
        else
        {
            image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}
