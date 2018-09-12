using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Weapon_Orb : WeaponProperties {
    private float size;


    public override void Start()
    {
        base.Start();
        size = transform.lossyScale.x;
    }

    public override void OnTouchStar(GameObject star)
    {
        if (GetComponent<Rigidbody>().velocity.magnitude > minBreakVelocity)
        {
            GameObject spawner = Instantiate(Resources.Load("Prefabs/Spawners/StardustSpawner", typeof(GameObject))) as GameObject;
            spawner.transform.position = star.transform.position;
            spawner.GetComponent<StardustSpawnScript>().explosionForce = GetComponent<Rigidbody>().velocity.magnitude * 100;
            star.GetComponent<StarScript>().Deactivate();
            CameraEffects.instance.ShakeCamera(0.3f, 1f);
        }
        else
        {
            star.GetComponent<Rigidbody>().AddExplosionForce(GetComponent<Rigidbody>().velocity.magnitude * 50, transform.position, 50);
        }
        
    }

    public override void Update()
    {
        base.Update();
        transform.localScale = new Vector3(size, size, size + GetComponent<Rigidbody>().velocity.magnitude / 20);
    }
}
