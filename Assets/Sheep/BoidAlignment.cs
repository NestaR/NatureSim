using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Sheep))]
public class BoidAlignment : MonoBehaviour
{
    private Sheep boid;
    public float radius;

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
            {//Find the velocity of the sheep and add it to the average
                average += boid.velocity;
                //1 more sheep is found
                found += 1;
            }

        }
        if (found > 0)
        {//If any objects are found get the average of the difference and the number of sheep found
            average = average / found;
            //Lerp towards the average velocity so everything goes in the same direction
            boid.velocity += Vector3.Lerp(boid.velocity, average, Time.deltaTime);
        }
    }
}
