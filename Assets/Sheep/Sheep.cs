using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Sheep : MonoBehaviour
{
    public float maxVelocity;
    public Vector3 velocity;
    public GameObject prefab;
    public ParticleSystem deathEffect;

    private Transform target;
    private UnityEngine.AI.NavMeshAgent agent;
    private float timer, hungertimer, healthtimer, agetimer, lerptimer, birthtimer;
    bool sleeping = false;
    public bool follower = false, oldest = true, hungry = false;
    bool hasCollided = false, oldestExists = false;
    float m_MaxDistance;
    bool m_HitDetect;
    Vector3 flocktarget;
    Vector3 center;
    Vector3 randTurn;
    Collider sheepCollider;
    RaycastHit m_Hit;
    HealthScript sheepHealth;
    DayNightCycle timeOfDay;
    BoidCohesion leader;
    //Size for the sheep to lerp through
    Vector3 child = new Vector3(0.5f, 0.5f, 0.5f);
    Vector3 adult = new Vector3(2.5f, 2.5f, 2.5f);
    Renderer rend;
    void Start()
    {
        //Set the velocity so that the sheep is moving forward
        velocity = this.transform.forward * maxVelocity;
        //Choose the distance the Box Cast can reach to
        m_MaxDistance = 1f;
        sheepCollider = GetComponent<Collider>();
        //Get the health from the health script to update the slider
        sheepHealth = this.GetComponent<HealthScript>();
        leader = this.GetComponent<BoidCohesion>();
        //Check what time of day it is
        timeOfDay = GetComponent<DayNightCycle>();
        //Rename the prefab to Sheep for easier access
        this.gameObject.name = "Sheep";
        rend = GetComponent<Renderer>();
        center = rend.bounds.center;      
    }
    void OnEnable()
    {
        //Let the sheep move around the nav mesh and not fall off the map
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!hasCollided)
        {//If there are no sheep nearby it becomes the oldest
            oldest = true;
            follower = false;
        }
        if(!oldest && !hungry)
        {
            follower = true;
        }    
        if (velocity.magnitude > maxVelocity)
        {//Stops sheep from going past max velocity
            velocity = velocity.normalized * maxVelocity;
        }
        if (sheepHealth.health <= 20)
        {//If the sheeps health is low enough it will start looking for food
            hungry = true;
        }
        else
        {
            hungry = false;
        }

        if (GameObject.Find("Day/Night").GetComponent<DayNightCycle>().dayTime)
        {//Make the sheep only move during the day time
            sleeping = false;
            this.transform.position += velocity * Time.deltaTime;
            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), 0.05F);
            //this.transform.rotation = Quaternion.LookRotation(velocity);
            timer += Time.deltaTime;
            birthtimer += Time.deltaTime;
            healthtimer += Time.deltaTime;
            agetimer += Time.deltaTime;
            lerptimer += Time.deltaTime;
            //Have the animal grow in size over time
            transform.localScale = Vector3.Lerp(child, adult, lerptimer * 0.03f);
            oldestExists = false;
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 20f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.tag == "Sheep")
                {                  
                    var diff = hitCollider.transform.position - this.transform.position;
                    Sheep s2 = hitCollider.gameObject.GetComponent<Sheep>();
                    if (diff.magnitude < 20f)
                    {//Check if there is an older sheep nearby
                        if (diff != Vector3.zero)
                        {
                            hasCollided = true;
                            if (this.agetimer < s2.agetimer)
                            {//Check if this sheep is younger than another sheep
                                oldest = false;                             
                            }
                            if (s2.oldest || oldest)
                            {//Check if the oldest sheep is in the herd
                                oldestExists = true;
                            }
                            if (!oldestExists && this.agetimer > s2.agetimer)
                            {//If the oldest sheep leaves the herd it will make the next oldest the leader
                                oldest = true;
                                follower = false;
                            }
                        }
                    }
                }
                else
                {
                    hasCollided = false;
                }
            }

            if (healthtimer >= 7f)
            {//Lower the sheeps health every 5 seconds
                sheepHealth.health -= 9;
                healthtimer = 0;
                if(maxVelocity < 4f)
                {//Increase the sheep speed slightly as time goes on
                    maxVelocity += 0.05f;
                }
            }
            if (birthtimer >= 30f && sheepHealth.health >= 65)
            {//If a certain amount of time has passed and the sheep has enough health it will reproduce
                var position = new Vector3(transform.position.x, 0, transform.position.z);
                Instantiate(prefab, position, Quaternion.identity);
                sheepHealth.health -= 35;
                birthtimer = 0;

            }
        }
        else
        {//At night the sheep stop moving and gain small amounts of health
            sleeping = true;
            healthtimer += Time.deltaTime;
            if (healthtimer >= 10f)
            {
                sheepHealth.health += 3;
                healthtimer = 0;
            }
        }
        if(sheepHealth.health <= 0)
        {//If the sheep dies then activate its death particle effect
            Destroy(Instantiate(deathEffect.gameObject, transform.position, Quaternion.identity) as GameObject, deathEffect.startLifetime);
        }

    }
    void LateUpdate()
    {
        if (timer >= Random.Range(5f, 9f) && (oldest || hungry))
        {//Make the sheep turn every random seconds
            randTurn = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
            velocity = Vector3.Lerp(velocity, randTurn, 0.8f);
            maxVelocity += 0.005f;
            timer = 0;
        }
    }

    void FixedUpdate()
    {
        //Test to see if there is a hit using a BoxCast and what the hit was
        m_HitDetect = Physics.BoxCast(sheepCollider.bounds.center, transform.localScale, transform.forward, out m_Hit, transform.rotation, m_MaxDistance);

        if (m_HitDetect && m_Hit.collider.tag == "Grass" && sheepHealth.health < 65 && !sleeping)
        {//If the box collides with grass the grass is destroyed and the animal gains health
            //Output the name of the Collider the box hit
            //Debug.Log("Hit : " + m_Hit.collider.name);
            
            if (m_Hit.collider.name == "Old Grass")
            {//If the grass os older the sheep gain less health from eating it
                sheepHealth.health += 5;
            }
            else
            {
                sheepHealth.health += 20;
            }          
            Destroy(m_Hit.collider.gameObject);

            if (maxVelocity > 2f)
            {//Lower the sheep speed once they eat
                maxVelocity -= 0.3f;
            }
            
        }
    }
    void OnDrawGizmos()
    {//Show the box cast
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, transform.forward * m_Hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * m_Hit.distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * m_MaxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * m_MaxDistance, transform.localScale);
        }
    }
}
