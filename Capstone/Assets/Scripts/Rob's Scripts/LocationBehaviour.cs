using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Rob
//Custom editor gives a handle to any object that has this script
//Than on game start it will create a sphere collider the same size of the handler
public class LocationBehaviour : MonoBehaviour
{    
    [SerializeField] public float radiusSize = 1f;
    [SerializeField] public float length = 1f;
    [SerializeField] public float width = 1f;
    [SerializeField] public float depth = 1f;

    [SerializeField] public enum ColliderType { sphere, box };
    [SerializeField] public ColliderType currentCollider;

    [SerializeField] private bool isTrigger;

    private SphereCollider sphereCollider;
    private BoxCollider boxCollider;

    private void Start()
    {
        if(currentCollider == ColliderType.sphere)
        {
            //create new sphere collider
            sphereCollider = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;

            //set sphere colliders radius to size of handleSize
            sphereCollider.radius = radiusSize;

            if(isTrigger)
            {
                //set to trigger
                sphereCollider.isTrigger = true;
            }            
        }
        else
        {
            //create new sphere collider
            boxCollider = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;

            //set box collider dimensions...
            boxCollider.size = new Vector3(width, depth, length);

            if(isTrigger)
            {
                //set to trigger
                boxCollider.isTrigger = true;
            }            
        }        
    }    
}
