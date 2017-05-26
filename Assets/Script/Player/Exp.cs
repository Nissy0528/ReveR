using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exp : MonoBehaviour
{

    private int exp;

    public int maxExp;
    public Player player;
    public Slider ex;

    // Use this for initialization
    void Start()
    {

    }

    public void EXP(int Exp)
    {
        exp = exp + Exp;
    }

    public void LevelUp(int maxexp)
    {
        if (exp >= maxexp)
        {
            player.GetComponent<Player>().ExtWing();
            exp = 0;
            maxexp = maxexp + 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        LevelUp(maxExp);
        ex.value = exp;
    }
}
