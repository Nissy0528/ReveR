using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeUI : MonoBehaviour
{
    private Animator anim;
    private RectTransform rect;
    private GameObject target;
    public static Vector3 TargetPos;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rect = GetComponent<RectTransform>();
        target = GameObject.FindGameObjectWithTag("EDEffect");

        //if (target != null)
        //    rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);

        rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, TargetPos);

        //target.tag = "Untagged";
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
