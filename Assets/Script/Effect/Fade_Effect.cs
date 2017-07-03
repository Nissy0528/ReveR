using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade_Effect : MonoBehaviour {

    private Animator anim;
    public float EndTime;

    private bool IsOver=false;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Dead();
    }

    /// <summary>
    /// 消滅処理
    /// </summary>
    private void Dead()
    {
        var animState = anim.GetCurrentAnimatorStateInfo(0);
        if (animState.normalizedTime >= EndTime)
        {
            IsOver = true;
        }
    }
    public bool GetBool()
    {
        return IsOver;
    }
}
