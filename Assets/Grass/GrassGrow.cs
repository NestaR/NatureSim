using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class GrassGrow : MonoBehaviour
{
    //Instantiate itself for spreading
    public GameObject prefab;
    public ParticleSystem deathEffect;
    public Material GrassBase;
    public Material GrassFinal;
    public int grassAmount, grassDensity;


    float t, timer, growtimer, spreadtimer;
    bool lerping = false, old = false, spread = false, onPlane;
    float lerpTime = 0;

    Renderer grassRenderer;
    Vector3 center;
    //Size for the grass to lerp through
    Vector3 start = new Vector3(0.1f, 0.1f, 0.1f);
    Vector3 end = new Vector3(4f, 4f, 4f);
    float radius;
    void Start()
    {
        grassRenderer = GetComponent<Renderer>();
        center = grassRenderer.bounds.center;
        radius = grassRenderer.bounds.extents.magnitude;
        this.gameObject.name = "Grass";
        //Sets the material for better lerping
        this.grassRenderer.material = GrassBase;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Collider[] planeCheck = Physics.OverlapSphere(center, 0.5f);
        foreach (var check in planeCheck)
        {//Check to see if the grass is on the plane
            if (check.tag == "Plane")
            {
                onPlane = true;
            }
        }
        if(!onPlane)
        {//If the grass is not on the plane it gets removed
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        grassAmount = 0;
        //Run timers for each stage of grasses life
        t += Time.deltaTime * 0.05f;
        timer += Time.deltaTime;
        lerpTime += Time.deltaTime;
        //Lerp the size of the grass to display growth
        transform.localScale = Vector3.Lerp(start, end, lerpTime * 0.05f);
        Collider[] hitColliders = Physics.OverlapSphere(center, 4f);
        foreach (var hitCollider in hitColliders)
        {//Check for the density of grass around its area
            if (hitCollider.tag == "Grass")
            {
                grassAmount += 1;
            }
        }
        

        
        if (!lerping)
        {
            spreadtimer += Time.deltaTime;
            if (spreadtimer >= Random.Range(9.0f, 13.0f))
            {//If a certain amount of time has passed the grass is allowed to spread
                spreadtimer = 0;
                spread = true;
            }

            if (spread && grassAmount <= grassDensity)
            {//Gets a random point within the area of the grass and spawns another piece of grass
                Vector2 pos = Random.insideUnitCircle * 5f;
                Vector3 spot = grassRenderer.transform.position + new Vector3(pos.x, 0, pos.y);
                Instantiate(prefab, spot, Quaternion.identity);
                spread = false;
            }
            if (timer >= 25f)
            {//After a certain amount of time the grass will grow old and change colour
                lerping = true;
                t = 0;
                Debug.Log("Lerping!");
                timer = 0;
            }
        }

        if (lerping)
        {//Changes the colour of the grass to signify age
            grassRenderer.material.Lerp(GrassBase, GrassFinal, t);
            if (t >= 1.0f)
            {//After changing colour it prepares to be destroyed
                Debug.Log("Stopped lerping");
                old = true; 
            }
            if(old && timer >= 3f)
            {//Destroy the grass and activates the death particles              
                Destroy(this.gameObject);
                Destroy(Instantiate(deathEffect.gameObject, transform.position, Quaternion.identity) as GameObject, deathEffect.startLifetime);
            }
        } 
    }

}
