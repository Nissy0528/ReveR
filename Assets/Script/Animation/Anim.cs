using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    private Animator anim;
    private AnimatorStateInfo animState;

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
    }

    /// <summary>
    /// アニメーションが終了したら消滅
    /// </summary>
    private void AnimDestroy()
    {
        animState = anim.GetCurrentAnimatorStateInfo(0);

        if (animState.normalizedTime>=1.0f)
        {
            Destroy(gameObject);
        }
    }
}
