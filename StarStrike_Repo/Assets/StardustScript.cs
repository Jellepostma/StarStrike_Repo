using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StardustScript : PoolableObject {

    public int setCollisionTime = 5;
    Collider c;
    private int timer = 0;

    protected override void Awake()
    {
        base.Awake();
        c = GetComponent<Collider>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        c.isTrigger = true;
        timer = 0;
    }

    void Update()
    {
        timer++;
        if (timer >= setCollisionTime)
        {
            c.isTrigger = false;
        }
    }
}
