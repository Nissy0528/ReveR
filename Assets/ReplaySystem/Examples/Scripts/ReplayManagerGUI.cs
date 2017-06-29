using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ReplayManager))]
public class ReplayManagerGUI : MonoBehaviour
{

	private ReplayManager man;

	private bool recorded = false;

	private int mode = 0;

	private string text = "Start Recording";

	private float frame = 0;

	// Use this for initialization
	void Start ()
	{
		man = GetComponent<ReplayManager> ();
		
	}

	// Update is called once per frame
	void OnGUI ()
	{
		
		if (mode == 0) {
			man.Initialize ();
			if (GUILayout.Button ("StartRecording")) {
				man.StartRecording ();
				text = "Recording...";
				mode = 1;
			}
		}
		if (mode == 1) {
			if (GUILayout.Button ("StopRecording")) {
				man.StopRecording ();
				text = "Recording Stopped";
				mode = 2;
				recorded = true;
			}
		}
		
		if (mode == 2) {
			if (GUILayout.Button ("StartReplay")) {
				man.StartReplay ();
				text = "Replaying...";
				mode = 3;
			}
		}
		
		if (mode == 3) {
			if (GUILayout.Button ("StopReplay")) {
				man.StopReplay ();
				text = "Replay Stopped";
				mode = 2;
			}
		}
		
		GUILayout.Label (text);
		
		if (recorded) {
			int max = man.MaxRecordCount ();
			GUILayout.Label ("" + max + " Recorded");
			
			float f = GUILayout.HorizontalSlider (frame, 0, max);
			if (((int)f) != ((int)frame)) {
				
				
				if (mode == 3) {
					man.StopReplay ();
					mode = 2;
				}
				
				frame = f;
				man.Playback ((int)frame);
			}
		}
		
		if (GUILayout.Button ("Reset")) {
			Application.LoadLevel (Application.loadedLevel);
		}
		
	}
	
	
}
