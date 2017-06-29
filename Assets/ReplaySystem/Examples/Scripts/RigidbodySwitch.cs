using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Replayable))]
public class RigidbodySwitch : MonoBehaviour
{


	void Start ()
	{
		GetComponent<Rigidbody>().isKinematic = true;
	}

	public void RecordStarting ()
	{
		
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().WakeUp();
	}

	public void RecordStopped ()
	{
	}

	public void ReplayStarting ()
	{
		
		GetComponent<Rigidbody>().isKinematic = true;
	}

	public void ReplayStopped ()
	{
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().WakeUp();
		
	}


	public void ReplayRecording (int dataIdx)
	{
		
	}

	public void ReplayPlaying (int dataIdx)
	{
		GetComponent<Rigidbody>().isKinematic = true;
	}
	
	
}
