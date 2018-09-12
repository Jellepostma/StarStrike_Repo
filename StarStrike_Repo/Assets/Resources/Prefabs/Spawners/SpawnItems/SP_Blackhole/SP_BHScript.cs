using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_BHScript : PoolableObject {

    public string[] destroyTags;

	// Use this for initialization
	protected override void OnEnable () {
        float randomScale = Random.Range(10, 50);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        iTween.ScaleFrom(gameObject, iTween.Hash("scale", Vector3.zero, "time", 3, "easetype", iTween.EaseType.easeOutElastic));
        Invoke("Implode", 5);
	}

    void Implode()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.zero, "time", 3, "easetype", iTween.EaseType.easeInElastic, "oncomplete", "Deactivate", "oncompletetarget", gameObject, "oncompleteparams", 2.0f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach(string t in destroyTags)
        {
            if(collision.collider.tag == t)
            {
                collision.gameObject.GetComponent<PoolableObject>().Deactivate();
            }
        }
    }
}
