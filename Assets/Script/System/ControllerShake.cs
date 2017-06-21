using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ControllerShake : MonoBehaviour
{
    private int shakeCnt = 0;

    // Update is called once per frame
    void Update()
    {
        Shake();
    }

    /// <summary>
    /// コントローラー振動
    /// </summary>
    private void Shake()
    {
        if (shakeCnt <= 0)
        {
            SetShake(0.0f, 0.0f, 0);
            return;
        }
        shakeCnt -= 1;
    }

    /// <summary>
    /// コントローラー振動設定
    /// </summary>
    public void SetShake(float left, float right, int shakeTime)
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerIndex pI = (PlayerIndex)i;
            GamePadState state = GamePad.GetState(pI);
            if (state.IsConnected)
            {
                GamePad.SetVibration(pI, left, right);
            }
        }

        shakeCnt = shakeTime;
    }
}
