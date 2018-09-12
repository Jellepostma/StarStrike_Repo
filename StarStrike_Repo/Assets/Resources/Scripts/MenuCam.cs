using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCam : MonoBehaviour {

	public bool freeRotate = false;
	public GameObject player;
	public Transform target;
	public float speed = 1f;
	private Vector3 playerPos;

	public void AttachPlayerCam(Transform obj){
		Camera.main.transform.parent = obj;
	}

	public void DetachPlayerCam(){
		Camera.main.transform.parent = null;
	}

	public void SetFreeRotate(bool rotate){
		GetComponent<Rigidbody> ().isKinematic = !rotate;
		freeRotate = rotate;
	}
	
	// Update is called once per frame
	void Update () {
		if(freeRotate){
			if (Input.touchCount == 1) {
				// GET TOUCH 0
				Touch touch0 = Input.GetTouch (0);

				if (touch0.phase == TouchPhase.Stationary) {
					GetComponent<Rigidbody> ().angularVelocity = Vector3.Slerp (GetComponent<Rigidbody> ().angularVelocity, Vector3.zero, Time.deltaTime * 10);
				}

				// APPLY ROTATION
				if (touch0.phase == TouchPhase.Moved) {
					GetComponent<Rigidbody> ().AddTorque (new Vector3 (0, touch0.deltaPosition.x * speed, 0));
				}
			}
		}
	}
}