using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_Meteor_Script : PoolableObject {

    public int impactRadius = 20;
    public int impactForce = 5000;

    private SphereCollider sc;
    private MeshRenderer mr;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
        
    }

    protected override void Awake()
    {
        base.Awake();
        sc = GetComponent<SphereCollider>();
        mr = GetComponent<MeshRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        transform.Find("Explosion").GetComponent<ParticleSystem>().Emit(25);
        sc.enabled = mr.enabled = true;
        foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
        {
            p.Play();
        }
    }

    // Use this for initialization
    private void OnCollisionEnter(Collision collision)
    {
        transform.Find("Explosion").GetComponent<ParticleSystem>().Emit(25);
        sc.enabled = mr.enabled = false;
        foreach(ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
        {
            p.Stop();
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, impactRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.tag == "Star" || col.tag == "Dust")
            {
                col.GetComponent<Rigidbody>().AddExplosionForce(impactForce, transform.position, impactRadius);
            }
            if(col.tag == "Player")
            {
                CameraEffects.instance.ShakeCamera(0.3f, 8f);
                CameraEffects.instance.fadeColor(1f, Color.red);
                Debug.Log(Vector3.Distance(transform.position, col.transform.position));
                col.GetComponentInParent<PlayerScript>().Fuel -= Vector3.Distance(transform.position, col.transform.position) / 2;
            }
        }
        Deactivate(2.0f);
    }
    
}
