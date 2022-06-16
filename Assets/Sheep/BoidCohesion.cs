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
    public bool leader = true;

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
        var average = startP;
        //Count how many sheep were found
        var found = 0;
        //Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radius);
        //foreach (var hitCollider in hitColliders)
        //{//Check for the density of grass around its area
        //    if (hitCollider.tag == "Sheep")
        //    {
        //        var diff = hitCollider.transform.position - this.transform.position;
        //        Sheep s2 = hitCollider.gameObject.GetComponent<Sheep>();
        //        if (diff.magnitude < radius)
        //        {//Calculate the difference between the sheep and add it to the average
        //            if (diff != Vector3.zero && s2.oldest)
        //            {
        //                average += diff;
        //                found += 1;
        //            }
        //            //average += diff;

        //            //1 more sheep is found
        //            //found += 1;
        //        }
        //    }
        //}
        if (!boid.oldest && !boid.hungry)
        {
            foreach (var boid in boids.Where(b => b != boid))//Go through every boid except itself
            {
                var diff = boid.transform.position - this.transform.position;
                if (diff.magnitude < radius)//Calculate the difference between the sheep and add it to the average
                {
                    if (boid.oldest)
                    {
                        //Debug.Log(diff.magnitude);
                        //Debug.Log("boid" + boid.transform.position.magnitude);
                        //Debug.Log("this" + this.transform.position.magnitude);
                        average = boid.transform.position;

                        //1 more sheep is found
                        found = 1;
                    }
                }
            }
            if (found > 0 && !boid.oldest && !boid.hungry)
            {//If any objects are found get the average of the difference and the number of sheep found
                average = average / found;
                //Move every sheep towards the average
                boid.velocity += Vector3.Lerp(this.transform.position, average, Time.deltaTime);
                //Moves with more force the further away the sheeps are
            }
        }
        //foreach (var boid in boids.Where(b => b != boid))
        //{
        //    var boiddiff = boid.transform.position - this.transform.position;
        //    var thisdiff = this.transform.position - average;
        //    if (boid.sheepleader && leader)
        //    {
        //        leader = false;
        //    }
        //    else
        //    {
        //        leader = true;
        //    }
        //}
        //if (boid.transform.position.x < -95)
        //{
        //    boid.velocity += new Vector3(10f, 0f, 0f);
        //}
        //else if (boid.transform.position.x > -2)
        //{
        //    boid.velocity += new Vector3(-10f, 0f, 0f);
        //}
        //if (boid.transform.position.z < 10)
        //{
        //    boid.velocity += new Vector3(0f, 0f, 10f);
        //}
        //else if (boid.transform.position.z > 95)
        //{
        //    boid.velocity += new Vector3(0f, 0f, -10f);
        //}
    }
}
