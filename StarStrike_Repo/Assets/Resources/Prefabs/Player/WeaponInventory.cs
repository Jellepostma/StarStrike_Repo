using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour {

    public GameObject[] weapons;
    private int size = 0;

    private void Start()
    {
        weapons = new GameObject[3];
    }

    public void addWeapon(string type)
    {
        GameObject weapon = Instantiate(Resources.Load("Prefabs/Weapons/" + type, typeof(GameObject))) as GameObject;
        weapon.transform.parent = transform;
        weapon.transform.position = GameObject.Find("TractorBH").transform.position;
        weapon.GetComponent<SpringJoint>().connectedBody = transform.Find("Player").GetComponent<Rigidbody>();
        weapons[size++] = weapon;
        weapon.GetComponent<WeaponProperties>().slot = size;
        if(type != "Orb")
        GameObject.Find("GUI").GetComponent<GUI_Game>().addWeaponIcon(size);
    }

    public void removeWeapon(int slot)
    {
        
        Destroy(weapons[slot - 1]);
        weapons[slot - 1] = null;
        size--;
    }

    public bool isFull()
    {
        return (size == 3);
    }

    public int getSize()
    {
        return size;
    }
}
