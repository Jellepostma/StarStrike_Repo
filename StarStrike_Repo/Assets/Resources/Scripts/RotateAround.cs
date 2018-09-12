using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

	public Transform target;
	public float speed = 1.0f;
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (target.transform.position, target.up, speed);
	}
}
