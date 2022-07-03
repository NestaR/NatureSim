using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Sheep))]
public class BoidCohesion : MonoBehaviour
{
    private Sheep boid;
    public float radius;
    Vector3 startP;

    void Start()
    {
        boid = GetComponent<Sheep>();
        startP = this.transform.position;

    }

    void Update()
    {
        //Get all sheep in the scene
        var boids = FindObjectsOfType<Sheep>();
        //var average = new Vector3(0f, 0f, 0f);
        var oldestPos = startP;
        //Count how many sheep were found
        var found = 0;

        if (boid.follower)
        {
            foreach (var boid in boids.Where(b => b != boid))
            {//Go through every boid except itself
                var diff = boid.transform.position - this.transform.position;
                if (diff.magnitude < radius)
                {
                    if (boid.oldest)
                    {//Get the position of the oldest sheep
                        oldestPos = boid.transform.position;
                        found = 1;
                    }
                }
            }
            if (found > 0 && boid.follower)
            {//Move every sheep towards the position of the oldest sheep
                boid.transform.position = Vector3.Lerp(boid.transform.position, oldestPos, Time.deltaTime * 0.1f);
                //Moves with more force the further away the sheeps are
            }
        }
    }
}
