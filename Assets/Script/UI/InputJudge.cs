using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputJudge : MonoBehaviour
{
    public GameObject[] UI;
    public InputManager inpuManager;
    public Player player;

    private int u_Num;
    private float def;
    private bool isEnemyFront;
    private Animator anim;
    private GameObject judgeUI;

    // Use this for initialization
    void Start()
    {
        def = 0.0f;
        u_Num = 0;
        isEnemyFront = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("L_Wing").tag != "Untagged" && GameObject.Find("R_Wing").tag != "Untagged")
        {
            SetU_Num();
        }
        SpawnUI();
    }

    /// <summary>
    /// UI生成
    /// </summary>
    private void SpawnUI()
    {
        if (!player.IsJudge()) return;

        judgeUI = Instantiate(UI[u_Num]);
        judgeUI.transform.parent = transform.parent;
        player.SetIsJudge(false);
    }

    /// <summary>
    /// 表示するUIの指定
    /// </summary>
    private void SetU_Num()
    {
        if (!inpuManager.IsInvert()) return;

        def = inpuManager.GetDef();

        if (!isEnemyFront)
        {
            u_Num = 0;
        }
        else
        {
            if (def < 1.0f)
            {
                u_Num = 1;
            }
            else
            {
                u_Num = 2;
            }
        }
    }
}
