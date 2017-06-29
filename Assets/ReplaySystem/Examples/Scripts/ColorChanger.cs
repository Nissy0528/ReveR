using UnityEngine;
using System.Collections;

public class ColorChanger : MonoBehaviour
{
	IEnumerator Start ()
	{
		while (true) {
			if (enabled) {
				GetComponent<Renderer>().material.color = new Color (Random.Range (0, 1f), Random.Range (0, 1f), Random.Range (0, 1f));
			}
			yield return new WaitForSeconds (0.25f);
		}
	}
}
