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
        //        var diff = hitCollider.transform.position - this.transform.position;
        //        if (diff.magnitude < radius)
        //        {//Calculate the difference between the sheep and add it to the average
        //            if (diff != Vector3.zero)
        //            {
        //                average += diff;
        //                found += 1;
        //            }
        //        }
        //    }
        //}
        if (!boid.oldest)
        {
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
            if (found > 0 && !boid.oldest)
            {//If any objects are found get the average of the difference and the number of sheep found
                average = average / found;
                //If another sheep is too close then slowly move away
                boid.velocity -= Vector3.Lerp(startP, average, average.magnitude / radius) * repulsionForce;
            }
        }

    }
}
