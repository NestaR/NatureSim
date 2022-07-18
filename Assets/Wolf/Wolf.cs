using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Wolf : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    public float maxVelocity;
    public Vector3 velocity;
    Vector3 randTurn, box;
    public float speed = 1f;
    public float movementSpeed = 2;
    private Animator animator;
    public ParticleSystem deathEffect;
    private float timer;
    private float attackTimer, healthtimer;
    public bool hunting = false;
    Hunting attTimer;
    Collider wolfCollider;
    float m_MaxDistance;
    bool m_HitDetect;
    RaycastHit m_Hit;
    private Vector3 currentAngle;
    public HealthScript wolfHealth;
    Renderer wolfRenderer;
    Vector3 center;
    public int wolfAmount, maxWolf = 1;
    // Start is called before the first frame update
    void Start()
    {
        wolfRenderer = GetComponent<Renderer>();
        center = wolfRenderer.bounds.center;

        this.gameObject.name = "Wolf";

        wolfCollider = GetComponent<Collider>();
        wolfHealth = this.GetComponent<HealthScript>();
        velocity = this.transform.forward * maxVelocity;
        animator = GetComponent<Animator>();
        //this.transform.position += new Vector3(0f,0f,20f);
        hunting = true;
        m_MaxDistance = 3f;
        box = new Vector3(2f, 6f, 1.5f);
        currentAngle = transform.eulerAngles;

        Collider[] hitColliders = Physics.OverlapSphere(center, 60f);
        foreach (var hitCollider in hitColliders)
        {//Check for the density of grass around its area
            if (hitCollider.tag == "Wolf")
            {
                wolfAmount += 1;
            }
        }
        if(wolfAmount > maxWolf)
        {
            Destroy(this.gameObject);
        }
    }
    void OnEnable()
    {
        //Let the sheep move around the nav mesh and not fall off the map
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
    void Update()
    {
        wolfAmount = 0;
        if(wolfHealth.health < 40)
        {
            hunting = true;
        }
        else
        {
            hunting = false;
        }
        //velocity = this.transform.forward * maxVelocity;
        this.transform.position += velocity * Time.deltaTime;
        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), 0.05F);
        timer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        healthtimer += Time.deltaTime;
        if (velocity.magnitude > maxVelocity)
        {//Stops wolf from going past max velocity
            velocity = velocity.normalized * maxVelocity;
        }

        if (healthtimer >= 12f)
        {//Lower the wolf's health every 5 seconds
            wolfHealth.health -= 10;
            healthtimer = 0;
        }
        if (wolfHealth.health <= 0)
        {//If the sheep dies then activate its death particle effect
            Destroy(Instantiate(deathEffect.gameObject, transform.position, Quaternion.identity) as GameObject, deathEffect.startLifetime);
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {

        if (timer >= Random.Range(5f, 9f) && !hunting)
        {//Make the wolf turn every random seconds

            randTurn = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
            velocity = Vector3.Lerp(velocity, randTurn, 1f);
            timer = 0;
        }


        animator.SetFloat("Speed", 2f);
    }
    void FixedUpdate()
    {
        //Test to see if there is a hit using a BoxCast and what the hit was
        m_HitDetect = Physics.BoxCast(wolfCollider.bounds.center, box, transform.forward, out m_Hit, transform.rotation, m_MaxDistance);

        if (m_HitDetect && m_Hit.collider.tag == "Sheep" && attackTimer > 5f)
        {//If the wolf collides with the sheep it attacks it
            Sheep s2 = m_Hit.collider.gameObject.GetComponent<Sheep>();
            if(s2.sheepHealth.health > 25)
            {//Wolf gains health when it kills the sheep
                s2.sheepHealth.health -= 25;
            }
            else
            {
                s2.sheepHealth.health -= 25;
                wolfHealth.health += 50;
            }
            attackTimer = 0f;


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
            Gizmos.DrawWireCube(transform.position + transform.forward * m_Hit.distance, box);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * m_MaxDistance);
            //Draw a cube at the maximum distance
            
            Gizmos.DrawWireCube(transform.position + transform.forward * m_MaxDistance, box);

        }
    }
}
