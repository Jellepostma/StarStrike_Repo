using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Weapon_Star : WeaponProperties
{

    public override void Start()
    {
        base.Start();
    }

    public override void OnTouchStar(GameObject star)
    {
        if (GetComponent<Rigidbody>().velocity.magnitude > minBreakVelocity)
        {
            GameObject spawner = Instantiate(Resources.Load("Prefabs/StardustSpawner", typeof(GameObject))) as GameObject;
            spawner.GetComponent<StardustSpawnScript>().amount = 25;
            spawner.transform.position = star.transform.position;
            //spawner.GetComponent<StardustSpawnScript>().planet = transform.parent.parent.parent;
            spawner.GetComponent<StardustSpawnScript>().explosionForce = GetComponent<Rigidbody>().velocity.magnitude * 100;
            GameObject.Destroy(star);
            GameObject.Find("GUI").GetComponent<GUI_Game>().removeWeaponIcon("Weapon_" + slot.ToString());
        }
        else
        {
            star.GetComponent<Rigidbody>().AddExplosionForce(GetComponent<Rigidbody>().velocity.magnitude * 50, transform.position, 50);
        }

    }

    public override void Update()
    {
        base.Update();
    }
}
