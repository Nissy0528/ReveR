using UnityEngine;
using System.Collections;

public class MousePickSwitch : MonoBehaviour
{

	public MousePick pick = null;

	void Start ()
	{
		pick.enabled = false;
	}

	public void RecordStarting ()
	{
		pick.enabled = true;
	}

	public void RecordStopped ()
	{
		pick.enabled = true;
	}
	
	
}
