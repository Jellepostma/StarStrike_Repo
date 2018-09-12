using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorScript : MonoBehaviour {

	public GameObject impactParticles;
	public GameObject craterPrefab;

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Planet") {
			GameObject crater = GameObject.Instantiate(impactParticles, transform.position, Quaternion.identity);
			GameObject impact = GameObject.Instantiate (craterPrefab, crater.transform);
			Quaternion rot = Quaternion.FromToRotation (crater.transform.forward, other.contacts [0].normal);
			crater.transform.rotation *= rot;
			Collider[] hitColliders = Physics.OverlapSphere (transform.position, 5);
			foreach (Collider col in hitColliders) {
				if (col.tag == "Player") {
					col.GetComponent<PlayerScript> ().fuel -= 20.0f;
				} else {
					if(col.GetComponent<Rigidbody>())col.GetComponent<Rigidbody> ().AddExplosionForce (10, transform.position, 5);
				}
			}
			Destroy (impact, 4);
			Destroy (crater, 8);
			Destroy (gameObject);
		}
	}
}
