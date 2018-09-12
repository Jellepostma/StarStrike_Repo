using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCollisionTimer : MonoBehaviour {

	public int setCollisionTime = 5;
	private int timer = 0;

	void Update () {
		timer++;
		if (timer >= setCollisionTime) {
			GetComponent<Collider> ().isTrigger = false;
			GameObject.Destroy (this);
		}
	}
}
