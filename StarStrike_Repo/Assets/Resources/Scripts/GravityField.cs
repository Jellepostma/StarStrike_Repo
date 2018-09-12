using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour {
	public float radius = 1000;
	public float force = -1000f;
    public string afflictedTag;
	// Use this for initialization
	void Start () {
		
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
	
	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere (transform.position, radius);
		foreach (Collider col in hitColliders) {
			if (col.tag == afflictedTag) {
				col.GetComponent<Rigidbody> ().AddExplosionForce (force / Vector3.Magnitude(transform.position - col.transform.position), transform.position, radius);
			}
		}
	}
}
