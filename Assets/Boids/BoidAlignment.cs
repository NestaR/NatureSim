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

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask predatorMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    public List<Transform> visiblePredators = new List<Transform>();
    void Start()
    {
        boid = GetComponent<Sheep>();
        startP = this.transform.position;
        StartCoroutine("FindTargetsWithDelay", 1f);
    }

    void Update()
    {
        //Get all sheep in the scene
        var boids = FindObjectsOfType<Sheep>();
        var oldestVel = startP;
        var found = 0;
        float oldestMaxVel = 0;
        //foreach (Transform visibleP in visiblePred)
        //{
        //    var diff = visibleP.transform.position - this.transform.position;
        //    Debug.Log(diff.magnitude);
        //    if (diff.magnitude < 3.5f)
        //    {
        //        boid.maxVelocity = 3.5f;
        //        boid.sheepHealth.health -= 15;
        //        Debug.Log("HIT");
        //    }
        //}

        if (boid.follower)
        {//Follower sheep look for the oldest sheep nearby to gets its velocity

            foreach (Transform visibleTarget in visibleTargets)
            {
                Sheep s2 = visibleTarget.gameObject.GetComponent<Sheep>();
                if (s2.oldest)
                {
                    oldestMaxVel = s2.maxVelocity;
                    oldestVel = s2.velocity;
                    found = 1;
                }
            }

            if (found > 0 && boid.follower)
            {//Lerp towards the velocity of the oldest sheep so they go in the same direction
                boid.velocity += Vector3.Lerp(boid.velocity, oldestVel, Time.deltaTime * 1f);
                boid.maxVelocity = oldestMaxVel - 0.5f;
            }
        }
        foreach (Transform visiblePredator in visiblePredators)
        {
            boid.hunted = true;
        }
        if(visiblePredators.Count == 0)
        {
            boid.hunted = false;
        }
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {//If a target is within a sheeps view radius and centred at the correct viewing angle its added to the list of visible targets
                    visibleTargets.Add(target);

                }
            }
        }

        visiblePredators.Clear();
        Collider[] pInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, predatorMask);

        for (int i = 0; i < pInViewRadius.Length; i++)
        {
            Transform target = pInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {//If a target is within a sheeps view radius and centred at the correct viewing angle its added to the list of visible targets
                    visiblePredators.Add(target);

                }
            }
        }
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
