using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Tree : MonoBehaviour
{
    //Instantiate itself for spreading
    public GameObject prefab;
    public ParticleSystem deathEffect;
    public Material TreeBase;
    public Material TreeFinal;
    public int treeAmount, treeDensity;

    float t, timer, growtimer, spreadtimer;
    bool lerping = false, old = false, spread = false, onPlane, nearOtherTree;
    float lerpTime = 0;

    Renderer treeRenderer;
    Vector3 center;
    //Size for the grass to lerp through
    Vector3 start = new Vector3(0.1f, 0.1f, 0.1f);
    Vector3 end = new Vector3(3f, 3f, 3f);
    float radius;
    void Start()
    {
        treeRenderer = GetComponent<Renderer>();
        center = treeRenderer.bounds.center;
        radius = treeRenderer.bounds.extents.magnitude;
        this.gameObject.name = "Tree";
        //Sets the material for better lerping
        this.treeRenderer.material = TreeBase;
        transform.localScale = start;

        checkArea(center, 3f);

        checkArea(center, 10f);

        if (nearOtherTree || !onPlane)
        {//If the grass is not on the plane it gets removed
            Destroy(this.gameObject);
            Debug.Log("Too close to another tree!");
        }
    }

    void Update()
    {
        treeAmount = 0;
        
        //Run timers for each stage of grasses life
        t += Time.deltaTime * 0.05f;
        timer += Time.deltaTime;
        lerpTime += Time.deltaTime;
        //Lerp the size of the grass to display growth
        transform.localScale = Vector3.Lerp(start, end, lerpTime * 0.02f);
        checkArea(center, 20f);




        if (!lerping)
        {
            if (treeAmount <= treeDensity)
            {//Gets a random point within the area of the grass and spawns another piece of grass
                Vector2 pos = Random.insideUnitCircle * 25f;
                Vector3 spot = treeRenderer.transform.position + new Vector3(pos.x, 0, pos.y);
                var diff = spot - this.transform.position;
                spreadtimer += Time.deltaTime;
                //Debug.Log(spreadtimer);
                //Collider[] planeCheck = Physics.OverlapSphere(spot, 3f);
                //foreach (var check in planeCheck)
                //{//Check to see if the grass is on the plane

                //    if (check.tag == "Plane")
                //    {
                //        if (diff.magnitude >= 0f)
                //        {

                //            if (spreadtimer >= Random.Range(30f, 50f))
                //            {//If a certain amount of time has passed the grass is allowed to spread
                //                //Debug.Log(check);
                //                Instantiate(prefab, spot, Quaternion.identity);
                //                spreadtimer = 0;

                //            }
                //        }
                //    }
                //}


                if (spreadtimer >= Random.Range(25f, 40f))
                {//If a certain amount of time has passed the grass is allowed to spread
                 //Debug.Log(check);
                    spread = false;
                    checkArea(center, 3f);
                    if (spread)
                    {
                        Instantiate(prefab, spot, Quaternion.identity);
                        spreadtimer = 0;
                    }

                }

            }
            if (timer >= 120f)
            {//After a certain amount of time the grass will grow old and change colour
                lerping = true;
                t = 0;
                Debug.Log("Lerping!");
                timer = 0;
            }
        }


            //if (spread && treeAmount <= treeDensity)
            //{//Gets a random point within the area of the grass and spawns another piece of grass
            //    Vector2 pos = Random.insideUnitCircle * 40f;
            //    Vector3 spot = treeRenderer.transform.position + new Vector3(pos.x, 0, pos.y);
            //    var diff = spot - this.transform.position;
            //    if (diff.magnitude >= 13f)
            //    {

            //        Instantiate(prefab, spot, Quaternion.identity);
            //        spread = false;
            //        spreadtimer = 0;
            //    }

            //}

        


        if (lerping)
        {//Changes the colour of the grass to signify age
            treeRenderer.material.Lerp(TreeBase, TreeFinal, t);
            if (t >= 4f)
            {//After changing colour it prepares to be destroyed
                Debug.Log("Stopped lerping");
                old = true;
            }
            if (old)
            {//Destroy the grass and activates the death particles              
                Destroy(this.gameObject);
                Destroy(Instantiate(deathEffect.gameObject, transform.position, Quaternion.identity) as GameObject, deathEffect.startLifetime);
            }
        }
    }

    void LateUpdate()
    {//Makes sure the tree spawns above the terrain
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        transform.position = pos;
    }
    public void checkArea(Vector3 middle, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(middle, radius);
        foreach (var hitCollider in hitColliders)
        {//Check for the density of grass around its area
            if (hitCollider.tag == "Tree" && hitCollider.transform.position != this.transform.position)
            {
                nearOtherTree = true;
                treeAmount += 1;
            }
            if (hitCollider.tag == "Plane")
            {
                onPlane = true;
                spread = true;
            }
        }
    }
}
