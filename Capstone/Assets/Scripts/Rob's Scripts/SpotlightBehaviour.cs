using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rob
//Allow designers to place lights anywhere without having to worry about specific measurements
//Only have to worry about how big they want the light to be
public class SpotlightBehaviour : MonoBehaviour
{
    //size you want spotlight to be
    [SerializeField] private float radius;
    private float distanceToGround;

    //how bright you want spotlight to be
    [SerializeField] private float intensity;
    
    //record starting angle to keep radius the same
    private float angleStartSize;    

    //record start radius size so we can shrink according to anglestartsize
    private float radiusStartSize;

    //what radius size do we want to terminate the light    
    [SerializeField] private float inactiveRadius;

    //grab references to components 
    private Light light;
    private SphereCollider sphereCollider;
    

    
    private void Start()
    {
        light = GetComponent<Light>();
        sphereCollider = GetComponent<SphereCollider>();

        //get distance from light to ground
        distanceToGround = FindDistance();       
        
        //adjust range of light source to be 2:3 ratio        
        light.range = (distanceToGround) * 1.5f;

        //set position of sphere collider equal to distance to ground
        sphereCollider.center = new Vector3(0.0f, 0.0f, distanceToGround);
        
        //set radius of collider equal to the radius input by designers
        radiusStartSize = radius;
        sphereCollider.radius = radiusStartSize;

        //make angle of the light equal to the radius of the sphere collider        
        angleStartSize = Mathf.Atan(radius / distanceToGround) * (180 / Mathf.PI) * 2;
        light.spotAngle = angleStartSize;        

        //set intensity to intensity set... improve
        light.intensity = intensity;        
    }

    private float FindDistance()
    {
        //set a layermask to terrain layer so that raycast only hits that layer
        LayerMask canHit = LayerMask.GetMask("Ground");

        if(canHit != 0)
        {
            Debug.Log(canHit.value);
        }

        //shout raycast to determine distance from ground
        RaycastHit hit;
        Physics.Raycast(transform.position + Vector3.down, Vector3.down, out hit, Mathf.Infinity, canHit);
        
        //+1 to offset the vector.down
        return hit.distance + 1;
    }

    
    //OTHER LIGHT SHRINK SOLUTION, BUT EXIT TRIGGER ISN'T FIRING BECAUSE TRIGGER
    //IS SHRINKING AND PLAYER ISN'T TECHNICALLY EXITING TRIGGER
    //player enters spotlight trigger
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.tag == "Player")
    //    {
    //        //start coroutine
    //        StartCoroutine(ShrinkLight());
    //    }
    //}

    ////player exits spotlight trigger
    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.gameObject.tag == "Player")
    //    {
    //        //stop coroutine
    //        StopCoroutine(ShrinkLight());
    //    }
    //}

    //shrink the light
    IEnumerator ShrinkLight()
    {
        while(true)
        {
            light.spotAngle -= (light.spotAngle / sphereCollider.radius) * Time.deltaTime;
            sphereCollider.radius = (light.spotAngle / angleStartSize) * radiusStartSize;

            yield return null;
        }
    }



    //ALTERNATE SHRINK, I THINK IT'S LESS EFFECIENT 
    //shrinks the light at a consistant rate 
    private void ShrinkLightAlt()
    {
        light.spotAngle -= (light.spotAngle / sphereCollider.radius) * Time.deltaTime;
        sphereCollider.radius = (light.spotAngle / angleStartSize) * radiusStartSize;
    }

    private void OnTriggerStay(Collider other)
    {
        //player enters spotlight
        if (other.gameObject.tag == "Player")
        {
            //if light source has no intensity than deactivate
            if(sphereCollider.radius <= inactiveRadius)
            {
                Debug.Log("Spotlight Inactive");
                gameObject.SetActive(false);
                return;
            }

            //increase players health
            

            //shrink the light
            ShrinkLightAlt();
            Debug.Log("Shrink Light");
        }
    }    
}
