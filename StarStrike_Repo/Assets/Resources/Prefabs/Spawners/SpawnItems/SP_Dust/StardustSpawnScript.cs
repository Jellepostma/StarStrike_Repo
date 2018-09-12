using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StardustSpawnScript : MonoBehaviour {

	public GameObject stardustObject;
	public int amount = 10;
	public float explosionForce = 500.0f;
	public Color[] starColors;
	// Use this for initialization
	void Start () {
		for (int j = 0; j < amount; j++) {
            GameObject stardust = ObjectPooler.sharedInstance.GetPooledObject("Dust");
            stardust.transform.position = transform.position;
            stardust.SetActive(true);
			Mesh starMesh = stardust.GetComponent<MeshFilter> ().mesh;
			Vector3[] vertices = starMesh.vertices;
			Color[] colors = new Color[vertices.Length];
			Color starColor = starColors [Random.Range (0, starColors.Length)];
			stardust.GetComponent<TrailRenderer> ().startColor = stardust.GetComponent<TrailRenderer> ().endColor = starColor;
			for (int i = 0; i < vertices.Length; i++) {
					colors [i] = starColor;
			}
			starMesh.colors = colors;
			stardust.GetComponent<Rigidbody> ().AddExplosionForce (explosionForce, transform.position, 10);
		}
        Destroy(gameObject);
	}
}