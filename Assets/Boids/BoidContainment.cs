using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Sheep))]
public class BoidContainment : MonoBehaviour
{
    private Sheep boid;
    //public float radius;
    //public float boundaryForce;
    bool area1, area2, area3;
    void Start()
    {
        boid = GetComponent<Sheep>();
        if (boid.transform.position.x < -5 && boid.transform.position.x > -95 && boid.transform.position.z > 15 && boid.transform.position.z < 95)
        {//Check which area the boid is in
            area1 = true;
            area2 = false;
            area3 = false;
        }
        else if (boid.transform.position.x < -25 && boid.transform.position.x > -95 && boid.transform.position.z > -95 && boid.transform.position.z < -25)
        {
            area1 = false;
            area2 = true;
            area3 = false;
        }
        else if (boid.transform.position.x < 95 && boid.transform.position.x > 30 && boid.transform.position.z > -95 && boid.transform.position.z < 95)
        {
            area1 = false;
            area2 = false;
            area3 = true;
        }
    }

    void Update()
    {
        if(area1)
        {//If a boid is in a certain area of the map it will stay within those bounds
            if (boid.transform.position.x < -95)
            {
                boid.velocity += new Vector3(10f, 0f, 0f);
            }
            else if (boid.transform.position.x > -5)
            {
                boid.velocity += new Vector3(-10f, 0f, 0f);
            }
            if (boid.transform.position.z < 15)
            {
                boid.velocity += new Vector3(0f, 0f, 10f);
            }
            else if (boid.transform.position.z > 95)
            {
                boid.velocity += new Vector3(0f, 0f, -10f);
            }
        }
        else if (area2)
        {
            if (boid.transform.position.x < -95)
            {
                boid.velocity += new Vector3(10f, 0f, 0f);
            }
            else if (boid.transform.position.x > -25)
            {
                boid.velocity += new Vector3(-10f, 0f, 0f);
            }
            if (boid.transform.position.z < -95)
            {
                boid.velocity += new Vector3(0f, 0f, 10f);
            }
            else if (boid.transform.position.z > -25)
            {
                boid.velocity += new Vector3(0f, 0f, -10f);
            }
        }
        else if (area3)
        {
            if (boid.transform.position.x < 30)
            {
                boid.velocity += new Vector3(10f, 0f, 0f);
            }
            else if (boid.transform.position.x > 95)
            {
                boid.velocity += new Vector3(-10f, 0f, 0f);
            }
            if (boid.transform.position.z < -95)
            {
                boid.velocity += new Vector3(0f, 0f, 10f);
            }
            else if (boid.transform.position.z > 95)
            {
                boid.velocity += new Vector3(0f, 0f, -10f);
            }
        }
    }
}
