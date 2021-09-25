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
    void Start()
    {
        boid = GetComponent<Sheep>();
    }

    void Update()
    {
        //Get all sheep in the scene
        var boids = FindObjectsOfType<Sheep>();
        var average = Vector3.zero;
        //Count how many sheep were found
        var found = 0;

        foreach (var boid in boids.Where(b => b != boid))
        {//Go through every boid except itself
            var diff = boid.transform.position - this.transform.position;
            if (diff.magnitude < radius)
            {//Calculate the difference between the sheep and add it to the average
                average += diff;
                //1 more sheep is found
                found += 1;
            }

        }
        if (found > 0)
        {//If any objects are found get the average of the difference and the number of sheep found
            average = average / found;
            //If another sheep is too close then slowly move away
            boid.velocity -= Vector3.Lerp(Vector3.zero, average, average.magnitude / radius) * repulsionForce;
        }
    }
}
