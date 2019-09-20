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

    //keep null until interaction
    private PlayerHealthBehaviour playerHealthScript;    

    
    private void Start()
    {
        Setup();
        ResetLight(transform.position);
    }


    //grab references for object
    private void Setup()
    {
        light = GetComponent<Light>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    //reposition light source at specified destination and begin setting everything up
    private void ResetLight(Vector3 newPos)
    {
        //set light to new position
        transform.position = newPos;

        //set active
        gameObject.SetActive(true);

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
    
    //raycast to ground to find distance which effects all other variables
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

    //turn on light
    private void TurnOn()
    {
        gameObject.SetActive(true);
    }

    //turn of light for various reasons
    private void TurnOff()
    {
        gameObject.SetActive(false);
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
                //stop healing coroutine
                playerHealthScript.StopLightSourceHeal();

                //set light inactive
                Debug.Log("Spotlight Inactive");
                TurnOff();
                return;
            }
            
            //shrink the light
            ShrinkLightAlt();
            Debug.Log("Shrink Light");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //start light source healing
        if(other.tag == "Player")
        {
            playerHealthScript = other.GetComponent<PlayerHealthBehaviour>();
            playerHealthScript.StartLightSourceHeal();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //stop light source healing
        if(other.tag == "Player")
        {
            playerHealthScript.StopLightSourceHeal();
        }
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
    //IEnumerator ShrinkLight()
    //{
    //    while(true)
    //    {
    //        light.spotAngle -= (light.spotAngle / sphereCollider.radius) * Time.deltaTime;
    //        sphereCollider.radius = (light.spotAngle / angleStartSize) * radiusStartSize;

    //        yield return null;
    //    }
    //}



}
