using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	private AudioSource bgm;
	private AudioSource sfx;

	// Use this for initialization
	void Start () {
		bgm = transform.GetChild (0).GetComponent<AudioSource> ();
		sfx = transform.GetChild (0).GetComponent<AudioSource> ();
	}

	public void playBGM(AudioClip clip){
		bgm.clip = clip;
		bgm.Play ();
	}




}
