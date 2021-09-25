using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Sheep))]
public class BoidContainment : MonoBehaviour
{
    private Sheep boid;
    public float radius;
    public float boundaryForce;
    void Start()
    {
        boid = GetComponent<Sheep>();
    }

    void Update()
    {
        if (boid.transform.position.magnitude > radius)
        {//Keep the sheep within a certain radius
            boid.velocity += this.transform.position.normalized * (radius - boid.transform.position.magnitude) * boundaryForce * Time.deltaTime;
        }
    }
}
