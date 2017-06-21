using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    public float boostlWidth;
    public Material[] material;

    private float iniWidht;
    private float setWidth;
    private TrailRenderer trailRenderer;

    // Use this for initialization
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        iniWidht = trailRenderer.startWidth;
        setWidth = iniWidht;
    }

    // Update is called once per frame
    void Update()
    {
        Normal();
        Boost();
        trailRenderer.startWidth = setWidth;
    }

    /// <summary>
    /// 通常時
    /// </summary>
    private void Normal()
    {
        if (Input.GetKey(KeyCode.Joystick1Button0)) return;
        setWidth = Mathf.Max(setWidth - Time.deltaTime, iniWidht);
        trailRenderer.material = material[0];
    }

    /// <summary>
    /// 加速時
    /// </summary>
    private void Boost()
    {
        if (!Input.GetKey(KeyCode.Joystick1Button0)) return;
        setWidth = Mathf.Min(setWidth + Time.deltaTime, boostlWidth);
        trailRenderer.material = material[1];
    }
}
