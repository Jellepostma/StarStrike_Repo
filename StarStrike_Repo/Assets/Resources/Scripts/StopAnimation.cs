using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAnimation : MonoBehaviour {

    void releaseGUI()
    {
        GetComponent<Animator>().enabled = false;
    }
}
