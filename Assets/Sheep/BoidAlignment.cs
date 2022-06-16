using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Sheep))]
public class BoidAlignment : MonoBehaviour
{
    private Sheep boid;
    public float radius;
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
        var boids = FindObjectsOfType<Sheep>();
        //var average = new Vector3(0f, 0f, 0f);
        var average = startP;
        //Count how many sheep were found
        var found = 0;
        //Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radius);
        //foreach (var hitCollider in hitColliders)
        //{//Check for the density of grass around its area
        //    if (hitCollider.tag == "Sheep")
        //    {
        //        Sheep s2 = hitCollider.gameObject.GetComponent<Sheep>();
        //        var diff = hitCollider.transform.position - this.transform.position;
        //        if (diff.magnitude < radius) 
        //        {//Calculate the difference between the sheep and add it to the average
        //            if (diff != Vector3.zero && s2.oldest)
        //            {
        //                average += boid.velocity;
        //                found += 1;
        //            }
        //        }
        //    }
        //}
        if(!boid.oldest && !boid.hungry)
        {
            foreach (var boid in boids.Where(b => b != boid))
            {//Go through every boid except itself
                var diff = boid.transform.position - this.transform.position;
                if (boid.oldest)
                {
                    average = boid.velocity;
                    found = 1;
                }
                //if (diff.magnitude < radius)
                //{//Find the velocity of the sheep and add it to the average
                //    average += boid.velocity;
                //    //1 more sheep is found
                //    found += 1;
                //}

            }

            if (found > 0 && !boid.oldest && !boid.hungry)
            {//If any objects are found get the average of the difference and the number of sheep found
                average = average / found;
                //Lerp towards the average velocity so everything goes in the same direction
                boid.velocity += Vector3.Lerp(boid.velocity, average, Time.deltaTime);
            }
        }
    }
}
