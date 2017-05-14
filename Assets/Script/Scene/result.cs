using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class result : MonoBehaviour {

    public void OnClick(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}
