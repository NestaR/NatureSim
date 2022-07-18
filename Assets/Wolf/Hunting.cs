using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Wolf))]
public class Hunting : MonoBehaviour
{
    private Wolf predator;
    public float radius;
    Vector3 startP;
    private Animator animator;
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    DayNightCycle timeOfDay;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    private float chaseTimer, rechargeTimer;
    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    void Start()
    {
        animator = GetComponent<Animator>();
        predator = GetComponent<Wolf>();
        startP = this.transform.position;
        //StartCoroutine("FindTargetsWithDelay", 0f);
        timeOfDay = GetComponent<DayNightCycle>();
    }

    void Update()
    {
        //Debug.Log(chaseTimer);
        //Get all sheep in the scene
        var boids = FindObjectsOfType<Sheep>();
        //var average = new Vector3(0f, 0f, 0f);
        var oldestPos = startP;
        //Count how many sheep were found
        var found = 0;
        if (!GameObject.Find("Day/Night").GetComponent<DayNightCycle>().dayTime)
        {
            FindVisibleTargets();
            if (visibleTargets.Count == 0)
            {
                predator.hunting = false;
                predator.maxVelocity = 2f;
                rechargeTimer += Time.deltaTime;
            }
            else
            {
                predator.hunting = true;
                predator.maxVelocity = 10f;

            }
            if (chaseTimer > 7f)
            {
                animator.SetFloat("Speed", 2f);
                predator.maxVelocity = 2f;
                rechargeTimer += Time.deltaTime;
            }
            if (rechargeTimer > 10f)
            {
                chaseTimer = 0f;
                rechargeTimer = 0f;
                predator.maxVelocity = 6f;
            }
            if (predator.hunting)
            {
                foreach (Transform visibleTarget in visibleTargets)
                {

                    Sheep s2 = visibleTarget.gameObject.GetComponent<Sheep>();
                    oldestPos = s2.transform.position;
                    found = 1;
                    transform.LookAt(visibleTarget);
                    s2.hunted = true;
                    var diff = visibleTarget.transform.position - this.transform.position;
                    //Debug.Log(diff.magnitude);
                    s2.maxVelocity = 10f;
                    //s2.velocity = predator.velocity;
                    //if (diff.magnitude < 3.5f && attackTimer > 5f)
                    //{               
                    //    //s2.maxVelocity = 10f;
                    //    //s2.velocity = predator.velocity * s2.maxVelocity;
                    //    //s2.sheepHealth.health -= 15;
                    //    //Debug.Log("HIT");
                    //    //attackTimer = 0f;
                    //}
                }
                if (found > 0 && predator.hunting && chaseTimer < 7f)
                {//Move every sheep towards the position of the oldest sheep
                    chaseTimer += Time.deltaTime;

                    predator.transform.position = Vector3.Lerp(predator.transform.position, oldestPos, Time.deltaTime * 1f);
                    animator.SetFloat("Speed", 3.5f);
                    //Moves with more force the further away the sheeps are
                }
            }
        }
        else
        {
            predator.hunting = false;
            predator.maxVelocity = 2f;
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
