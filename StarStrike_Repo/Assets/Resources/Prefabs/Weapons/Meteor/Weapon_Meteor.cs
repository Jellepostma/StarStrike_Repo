using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Meteor : WeaponProperties {

    public override void Start()
    {
        base.Start();
    }

    public override void OnTouchStar(GameObject star)
    {
        if (GetComponent<Rigidbody>().velocity.magnitude > minBreakVelocity)
        {
            Debug.Log("meteorstrike!");
            GameObject explosion = Instantiate(Resources.Load("Prefabs/Explosion", typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
            explosion.transform.parent = transform.parent.parent.parent;
            explosion.transform.localScale = new Vector3(2, 2, 2);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 30);
            foreach (Collider col in hitColliders)
            {
                if(col.tag == "Star")
                {
                    GameObject spawner = Instantiate(Resources.Load("Prefabs/StardustSpawner", typeof(GameObject))) as GameObject;
                    spawner.GetComponent<StardustSpawnScript>().amount = 25;
                    spawner.transform.position = star.transform.position;
                    //spawner.GetComponent<StardustSpawnScript>().planet = transform.parent.parent.parent;
                    spawner.GetComponent<StardustSpawnScript>().explosionForce = GetComponent<Rigidbody>().velocity.magnitude * 100;
                    Destroy(col.gameObject);
                }
            }
            
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
