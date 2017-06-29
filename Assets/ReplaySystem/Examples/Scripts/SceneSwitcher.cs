using UnityEngine;
using System.Collections;

public class SceneSwitcher : MonoBehaviour
{

	public string[] scenes;
	
	public string description="";
	void OnGUI ()
	{
		
		GUILayout.BeginVertical(GUILayout.Height (Screen.height));
		GUILayout.Space(Screen.height-50);
		GUILayout.Label(description);
		
		GUILayout.BeginHorizontal ();
		foreach (string s in scenes) {
			if (GUILayout.Button (s)) {
				Application.LoadLevel (s);
			}
		}
		GUILayout.EndHorizontal();
		
		GUILayout.EndVertical();
	}
}
