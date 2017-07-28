using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour {

    public GameObject ret;

    public GameObject FadeOut;//フェードアウト

    private bool IsLoadTitle = false;//クレジットシーンフラグ
    private bool IsLoadMain = false;//メインシーンフラグ
    private bool IsLoadEnd = false;//ゲーム終了フラグ


    void Update () {
      

        LoadScene();
    }
    private void LoadScene()
    {
        if (FadeOut.GetComponent<Fade_Effect>().GetBool())
        {
            if (IsLoadTitle) SceneManager.LoadScene("Title");
            if (IsLoadEnd) Application.Quit();
        }
        else
        {
            if (!FadeOut.activeSelf)
            {

                if (Input.GetKeyDown(KeyCode.JoystickButton1) || ret == null)
                {
                    FadeOut.SetActive(true);
                    IsLoadTitle = true;
                    ControllerShake.Shake(0.0f, 0.0f);
                }

            }
        }
    }
}
