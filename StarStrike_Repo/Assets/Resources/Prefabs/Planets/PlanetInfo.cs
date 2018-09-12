using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo : MonoBehaviour {

	public string planetName;
	public Color planetColor;
	public bool locked;
    public int unlockScore = 0;
    public string nextLevel;
	public int highscore;
	private GameObject planet;
	public Material lockMat;
	private Material[] planetMats;

	// Use this for initialization
	void Start () {
        if (PlayerPrefs.GetInt(gameObject.name + "_Unlocked") == 1) locked = false;
        if(transform.parent.GetComponent<MeshRenderer>())planetColor = transform.parent.GetComponent<MeshRenderer>().material.color;
        planet = transform.Find ("Planet").gameObject;
		planetMats = planet.GetComponent<MeshRenderer> ().materials;
		Material[] tempMats = planetMats;
		/*if (locked) {
			for (int i = 0; i < planetMats.Length; i++) {
				tempMats [i] = lockMat;
			}
			planet.GetComponent<MeshRenderer> ().materials = tempMats;
		}*/
		highscore = PlayerPrefs.GetInt (planetName + "_Highscore");
        GetComponent<RotateAround>().enabled = !GetComponent<RotateAround>().enabled;
        GetComponent<RotateAround>().enabled = !GetComponent<RotateAround>().enabled;
    }

	public void unlock(){
		locked = false;
		planet.GetComponent<MeshRenderer> ().materials = planetMats;
	}
}
