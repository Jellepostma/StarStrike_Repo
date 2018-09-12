using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : PoolableObject {

	public float delay = 1.0f;
	public float fadeTime;
    private Renderer r;
    private TrailRenderer tr;
    [SerializeField] GameObject parts;

    protected override void Awake()
    {
        base.Awake();
        r = GetComponent<Renderer>();
        tr = GetComponent<TrailRenderer>();
    }

    protected override void OnEnable () {
        base.OnEnable();

        //Reset
        tr.Clear();
        transform.localScale = new Vector3(2, 2, 2);
        iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.zero, "delay", delay, "time", fadeTime, "oncomplete", "Deactivate", "oncompletetarget", gameObject, "oncompleteparams", 0.0f));
	}

    public override void Deactivate(float delay = 0)
    {
        GameObject.Instantiate(parts, transform.position, Quaternion.identity);
        base.Deactivate(delay);

    }
}
