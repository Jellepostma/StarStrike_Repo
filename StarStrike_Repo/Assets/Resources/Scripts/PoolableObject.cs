using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class PoolableObject : MonoBehaviour {

    Rigidbody rb;
    public Transform poolParent;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()
    {
        rb.velocity = rb.angularVelocity = Vector3.zero;
    }

    public virtual void Deactivate(float delay = 0.0f)
    {
        StartCoroutine("DeactivateTimer", delay);
    }
    
    IEnumerator DeactivateTimer(float d)
    {
        yield return new WaitForSeconds(d);
        transform.parent = poolParent;
        gameObject.SetActive(false);
    }
}
