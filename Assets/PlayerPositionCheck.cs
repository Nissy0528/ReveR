using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionCheck : MonoBehaviour {

    public GameObject player;
    public GameObject check;

    private Vector3 topLeft;//画面左上座標
    private Vector3 bottomRight;//画面右下座標
    private Vector3 playerSize;//プレイヤーのサイズ
    private Vector2 playerPos;//プレイヤーの座標
    private Camera mainCamera;//カメラ
    void Start () {
        player = GameObject.Find("Player");

        mainCamera = GetComponent<Camera>();//カメラ取得
        playerSize = player.transform.localScale;//プレイヤーのサイズ取得
    }
	
	// Update is called once per frame
	void Update () {
        check.transform.position =
            new Vector3(player.transform.position.x,
                        player.transform.position.y,
                        0);
	}
}
