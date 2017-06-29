using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Replayable))]
public class ReplayReceiverExample : MonoBehaviour
{
	 
	private ColorChanger cc = null;

	void Start ()
	{	
		cc = gameObject.GetComponent<ColorChanger> ();
	}

	public void RecordStarting ()
	{
		GetComponent<Rigidbody>().useGravity = true;
		if (cc)
			cc.enabled = true;
	}

	public void RecordStopped ()
	{
	}

	public void ReplayStarting ()
	{
		GetComponent<Rigidbody>().isKinematic = true;
		ColorChanger cc = gameObject.GetComponent<ColorChanger> ();
		if (cc)
			cc.enabled = false;
	}

	public void ReplayStopped ()
	{
		GetComponent<Rigidbody>().isKinematic = false;
		if (cc)
			cc.enabled = true;
	}


	public void ReplayRecording (int dataIdx)
	{
		
	}

	public void ReplayPlaying (int dataIdx)
	{
		if (cc)
			cc.enabled = false;
	}
	
	
}
