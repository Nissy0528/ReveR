using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarCtrl : MonoBehaviour {

    public static int hpbar;//hpの値
    private Slider slider;//スライダー
	// Use this for initialization
	void Start ()
    {
        slider = GameObject.Find("HpBar").GetComponent<Slider>();//HpBar取得
        slider.maxValue = hpbar;//sliderの最大値を設定
    }
	
	// Update is called once per frame
	void Update ()
    {
        slider.value = hpbar;//sliderに値を設定
	}
}
