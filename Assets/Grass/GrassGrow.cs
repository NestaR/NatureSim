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

        checkArea(center, 0.5f);

        if (!onPlane)
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
        checkArea(center, 10f);


        if (!lerping)
        {
            if (grassAmount <= grassDensity)
            {//Gets a random point within the area of the grass and spawns another piece of grass
                spreadtimer += Time.deltaTime;
                Vector2 pos = Random.insideUnitCircle * 30f;
                Vector3 spot = grassRenderer.transform.position + new Vector3(pos.x, 0, pos.y);

                if (spreadtimer >= Random.Range(5.0f, 11.0f))
                {//If a certain amount of time has passed the grass is allowed to spread
                    spread = false;
                    checkArea(center, 0.5f);
                    if (spread)
                    {
                        Instantiate(prefab, spot, Quaternion.identity);
                        spreadtimer = 0;
                    }
                }
            }
            if (timer >= 45f)
            {//After a certain amount of time the grass will grow old and change colour
                lerping = true;
                t = 0;
                Debug.Log("Lerping!");
                timer = 0;
            }
        }

        

        if (lerping)
        {//Changes the colour of the grass to signify age
            this.gameObject.name = "Old Grass";
            grassRenderer.material.Lerp(GrassBase, GrassFinal, t);
            if (t >= 4.0f)
            {//After changing colour it prepares to be destroyed
                Debug.Log("Stopped lerping");
                old = true;
                
            }
            if(old)
            {//Destroy the grass and activates the death particles              
                Destroy(this.gameObject);
                Destroy(Instantiate(deathEffect.gameObject, transform.position, Quaternion.identity) as GameObject, deathEffect.startLifetime);
            }
        } 
    }
    void LateUpdate()
    {//Makes sure the grass spawns above the terrain
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        transform.position = pos;
    }

    public void checkArea(Vector3 middle, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(middle, radius);
        foreach (var hitCollider in hitColliders)
        {//Check for the density of grass around its area
            if (hitCollider.tag == "Grass")
            {
                grassAmount += 1;
            }
            if (hitCollider.tag == "Plane")
            {
                onPlane = true;
                spread = true;
            }
        }
    }
}
