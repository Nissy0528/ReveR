using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public Text clearTime;
    public List<Text> timeRank;

    private List<float> times;
    private string saveKey = "TimesKey";

    // Use this for initialization
    void Start()
    {
        clearTime.text = Main.time.ToString();

        times = PlayerPrefsUtility.LoadList<float>(saveKey);
        times.Add(Main.time);
        times.Sort();

        if (times.Count == 4)
        {
            times.RemoveAt(3);
        }
        PlayerPrefsUtility.SavaList<float>(saveKey, times);

        for(int i = 0; i < times.Count; i++)
        {
            timeRank[i].text = times[i].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            SceneManager.LoadScene("Title");
        }
    }


}
