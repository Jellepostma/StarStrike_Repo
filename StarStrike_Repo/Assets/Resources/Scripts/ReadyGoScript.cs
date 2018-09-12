using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyGoScript : MonoBehaviour {


	public void emitStars()
    {
        GameObject.Find("StarParts_RG").GetComponent<ParticleSystem>().Emit(20);
    }
}
