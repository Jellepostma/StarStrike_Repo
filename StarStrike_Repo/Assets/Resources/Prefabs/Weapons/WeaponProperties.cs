using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class WeaponProperties : MonoBehaviour {

    LineRenderer line;
    Transform playerTransform;
    ParticleSystem particles;
    ParticleSystem.MainModule particleSettings;

    public int slot;
    public float strength;
    public float mass;
    public float scoreBoost = 0.1f;
    public float minBreakVelocity = 120;

    // Use this for initialization
    public virtual void Start () {
        line = GetComponent<LineRenderer>();
        playerTransform = GameManager.instance.player.transform;
        particles = GetComponentInChildren<ParticleSystem>();
        particleSettings = particles.main;
    }

    public virtual void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Dust")
        {
            if (other.isTrigger)
                return;
            particleSettings.startColor = other.gameObject.GetComponent<MeshFilter>().mesh.colors[0];
            particles.Emit(5);
            other.GetComponent<StardustScript>().Deactivate();
            GameManager.instance.AddScore(1);
            playerTransform.parent.GetComponent<PlayerScript>().Fuel += scoreBoost;
        }
        if (other.gameObject.tag == "Star")
        {
            OnTouchStar(other.gameObject);
        }
    }

    abstract public void OnTouchStar(GameObject star);

    // Update is called once per frame
    public virtual void Update () {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, playerTransform.position);
    }
}
