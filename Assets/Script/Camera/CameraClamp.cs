using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClamp : MonoBehaviour
{
    public GameObject player;//プレイヤー

    private Vector3 topLeft;//画面左上座標
    private Vector3 bottomRight;//画面右下座標
    private Vector3 playerSize;//プレイヤーのサイズ
    private Vector2 playerPos;//プレイヤーの座標
    private Camera mainCamera;//カメラ

    // Use this for initialization
    void Start()
    {
        mainCamera = GetComponent<Camera>();//カメラ取得
        playerSize = player.transform.localScale;//プレイヤーのサイズ取得
    }

    // Update is called once per frame
    void Update()
    {
        Clamp();//プレイヤーの移動制限
    }

    /// <summary>
    /// プレイヤーの移動制限
    /// </summary>
    private void Clamp()
    {
        topLeft = mainCamera.ScreenToWorldPoint(Vector3.zero);//画面左上の座標
        bottomRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));//画面右下の座標
        playerPos = player.transform.position;//プレイヤーの座標

        playerPos.x = Mathf.Clamp(playerPos.x, topLeft.x + playerSize.x / 10, bottomRight.x - playerSize.x / 10);//画面の横幅内に横移動を制限
        playerPos.y = Mathf.Clamp(playerPos.y, topLeft.y + playerSize.y / 10, bottomRight.y - playerSize.y / 10);//画面の縦幅内に縦移動を制限

        player.transform.position = playerPos;//プレイヤーの座標制限
    }
}
