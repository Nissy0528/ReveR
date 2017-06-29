/*
 This file is part of Replay Framework for Unity .

    Mindset reader for Unity is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Foobar is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
*/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Replayable))]
public class RigidbodyRecover : MonoBehaviour
{

	private Vector3 velocity;
	private Vector3 angularVelocity;
	//private float drag;
	//private float angularDrag;
	private bool useGravity;
	private bool isKinematic;

	private bool resetting = false;

	void Start ()
	{
		if (!GetComponent<Rigidbody>()) {
			Destroy (this);
		} else {
			velocity = GetComponent<Rigidbody>().velocity;
			angularVelocity = GetComponent<Rigidbody>().angularVelocity;
			//drag = rigidbody.drag;
			//angularDrag = rigidbody.angularDrag;
			useGravity = GetComponent<Rigidbody>().useGravity;
			isKinematic = GetComponent<Rigidbody>().isKinematic;
		}
	}

	void FixedUpdate ()
	{
		if (resetting) {
			DestroyImmediate (GetComponent<Rigidbody>());
			
			gameObject.AddComponent<Rigidbody> ();
			GetComponent<Rigidbody>().useGravity = false;
			
			bool k = GetComponent<Rigidbody>().isKinematic;
			if (k) {
				GetComponent<Rigidbody>().isKinematic = false;
			}
			GetComponent<Rigidbody>().velocity = velocity;
			GetComponent<Rigidbody>().angularVelocity = angularVelocity;
			GetComponent<Rigidbody>().useGravity = useGravity;
			GetComponent<Rigidbody>().isKinematic = isKinematic;
			
			resetting = false;
		}
	}

	public void ReplayReset ()
	{
		resetting = true;
	}
	
	
	
	
}
