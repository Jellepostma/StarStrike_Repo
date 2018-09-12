using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToReferences : MonoBehaviour {

    private void Awake()
    {
        if(!GameManager.instance.references.ContainsKey(name))
        {
            GameManager.instance.references.Add(name, gameObject);
        }
    }
}
