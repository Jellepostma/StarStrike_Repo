using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorPull : MonoBehaviour {

    public bool pulling = false;
    private PlayerScript player;
    private GameObject pullObject;
    private GameObject UFO;
    private GameManager gm;
    private WeaponInventory inventory;
    public string[] pullTypes;


    // Use this for initialization
    void Start () {
        gm = GameObject.Find("System").GetComponent<GameManager>();
        inventory = GameObject.Find("PlayerRotator").GetComponent<WeaponInventory>();
        UFO = GameObject.Find("UFO");
        player = GameObject.Find("PlayerRotator").GetComponent<PlayerScript>();
	}

    public void Implode()
    {
        transform.parent = null;
        Camera.main.transform.parent = null;
        Destroy(GameObject.Find("Galaxy"));
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == pullObject && !inventory.isFull())
        {
            pullObject = null;
            pulling = false;
            player.canMove = true;
            GetComponent<Animator>().Play("CloseBH");
            UFO.GetComponent<Animator>().Play("CloseUFO");
            inventory.addWeapon(other.tag);
            Destroy(other.gameObject);
        }
    }

    public void GameOver()
    {
        gm.ShowGOScreen();
    }

    // Update is called once per frame
    void Update () {
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.rigidbody)
                {
                    foreach(string objTag in pullTypes)
                    {
                        if(hit.collider.tag == objTag)
                        {
                            player.canMove = false;
                            if (hit.rigidbody.isKinematic) hit.rigidbody.isKinematic = false;
                            hit.rigidbody.velocity = Vector3.zero;
                            GetComponent<Animator>().Play("OpenBH");
                            UFO.GetComponent<Animator>().Play("OpenUFO");
                            pullObject = hit.collider.gameObject;
                            pulling = true;
                            
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && pullObject != null)
        {
            player.canMove = true;
            GetComponent<Animator>().Play("CloseBH");
            UFO.GetComponent<Animator>().Play("CloseUFO");
            pulling = false;
        }
        if (pulling)
        {
            pullObject.GetComponent<Rigidbody>().AddExplosionForce(-300, transform.position, 5000);
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, pullObject.transform.position);
        }
    }
}
