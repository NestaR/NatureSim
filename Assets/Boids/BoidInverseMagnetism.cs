using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Sheep))]
public class BoidInverseMagnetism : MonoBehaviour
{
    private Sheep boid;
    public float radius;
    public float repulsionForce;
    Vector3 startP;
    BoidCohesion leader;

    void Start()
    {
        boid = GetComponent<Sheep>();
        startP = this.transform.position;
        leader = this.GetComponent<BoidCohesion>();
    }

    void Update()
    {
        //Get all sheep in the scene
        var boids = FindObjectsOfType<Wolf>();
        //var average = new Vector3(0f, 0f, 0f);
        var average = startP;
        //Count how many sheep were found
        var found = 0;


        foreach (var boid in boids.Where(b => b != boid))
        {//Go through every boid except itself
            var diff = boid.transform.position - this.transform.position;
            if (diff.magnitude < radius)
            {//Calculate the difference between the sheep and add it to the average
                average = diff;
                //1 more sheep is found
                found += 1;
            }

        }
        if (found > 0)
        {//If any wolves are found the sheep will move away from them
            average = average / found;
            boid.velocity -= Vector3.Lerp(boid.velocity, average, average.magnitude / radius) * repulsionForce;
            boid.maxVelocity = 5f;
        }
        else
        {
            boid.maxVelocity = 2f;
        }

    }
}
