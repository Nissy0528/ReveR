using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ReplayManager))]
public class ReplayCameraGUI : MonoBehaviour
{

	private ReplayManager man;

	private bool recorded = false;

	private int mode = 0;

	private string text = "Start Recording!";

	private float frame = 0;

	public Camera[] cameras;

	private int cameraNum = 0;


	// Use this for initialization
	void Start ()
	{
		man = GetComponent<ReplayManager> ();
		
		for (int i = 1; i < cameras.Length; i++) {
			cameras[i].enabled = false;
		}
		cameras[0].enabled = true;
		
	}

	// Update is called once per frame
	void OnGUI ()
	{
		
		if (GUILayout.Button ("ChangeCamera")) {
			cameras[cameraNum].enabled = false;
			cameraNum = (cameraNum + 1) % cameras.Length;
			cameras[cameraNum].enabled = true;
		}
		
		if (mode == 0) {
			if (GUILayout.Button ("StartRecording")) {
				man.Initialize ();
				man.StartRecording ();
				text = "Recording...";
				mode = 1;
			}
		}
		if (mode == 1) {
			if (GUILayout.Button ("StopRecording")) {
				man.StopRecording ();
				text = "Stopped Recording";
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
			} else {
				bool flag = false;
				foreach (Replayable r in man.replayables) {
					flag = (r.replayCount + 3 < r.recordCount);
					if (flag)
						break;
				}
				if (!flag)
					man.StartReplay ();
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
