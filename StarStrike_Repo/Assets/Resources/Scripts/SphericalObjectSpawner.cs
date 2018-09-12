using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalObjectSpawner : MonoBehaviour
{

    public string objectTag;
    public float radius;
    public bool spawnFromSphere = false;
    public bool parentToSpawner = false;
    public float spawnDist = 10.0f;
    public int timeScale = 40;
    private int Timer = 0;
    private int spawnTime;

    private void OnDrawGizmos()
    {
        if (spawnFromSphere)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

    void Awake()
    {
        spawnTime = Random.Range(1 * timeScale, 4 * timeScale);
    }

    Vector2 GetUnitOnCircle(float angleDegrees, float radius)
    {

        // initialize calculation variables
        float _x = 0;
        float _y = 0;
        float angleRadians = 0;
        Vector2 _returnVector;

        // convert degrees to radians
        angleRadians = angleDegrees * Mathf.PI / 180.0f;

        // get the 2D dimensional coordinates
        _x = radius * Mathf.Cos(angleRadians);
        _y = radius * Mathf.Sin(angleRadians);

        // derive the 2D vector
        _returnVector = new Vector2(_x, _y);

        // return the vector info
        return _returnVector;
    }

    // Update is called once per frame
    void Update()
    {
        Timer++;
        if (Timer > spawnTime)
        {
            GameObject spawn = ObjectPooler.sharedInstance.GetPooledObject(objectTag);
            Vector3 spawnPos;
            if (spawnFromSphere)
            {
                spawnPos = transform.position + Random.onUnitSphere * radius;
                
            } else
            {
                
                Vector2 circle = GetUnitOnCircle(Random.Range(0, 360), radius) + new Vector2(0.5f, 0.5f);
                spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(circle.x, circle.y, spawnDist));
            }

            spawn.transform.position = spawnPos;
            if (parentToSpawner) spawn.transform.parent = transform;
            spawn.SetActive(true);

            Timer = 0;
            spawnTime = Random.Range(1 * timeScale, 4 * timeScale);
        }
    }
}
