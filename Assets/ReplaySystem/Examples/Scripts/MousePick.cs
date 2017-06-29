using UnityEngine;
using System.Collections;

public class MousePick : MonoBehaviour
{
	private Transform grabbed;
	private float grabDistance = 10.0f;

	void Update ()
	{
		if (Input.GetMouseButton (0))
			if (grabbed)
				Drag ();
			else
				Grab ();
		else
			grabbed = null;
	}

	private void Grab ()
	{
		if (grabbed)
			grabbed = null;
		else {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit))
				grabbed = hit.transform;
		}
	}

	private void Drag ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Vector3 position = transform.position + transform.forward * grabDistance;
		Plane plane = new Plane (-transform.forward, position);
		float distance;
		if (plane.Raycast (ray, out distance)) {
			grabbed.position = ray.origin + ray.direction * distance;
			grabbed.rotation = transform.rotation;
		}
	}
	
	
}
