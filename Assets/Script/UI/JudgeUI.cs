using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeUI : MonoBehaviour
{
    private Animator anim;
    private RectTransform rect;
    private GameObject target;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rect = GetComponent<RectTransform>();
        target = GameObject.FindGameObjectWithTag("EDEffect");

        rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);

        target.tag = "Untagged";
    }

    // Update is called once per frame
    void Update()
    {
        Dead();//消滅処理
    }

    /// <summary>
    /// 消滅処理
    /// </summary>
    private void Dead()
    {
        var animState = anim.GetCurrentAnimatorStateInfo(0);
        if (animState.normalizedTime >= 1)
        {
            Destroy(gameObject);
        }
    }
}
