using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rob
//Generic follow object script, attach to the object you want to be the follower
public class FollowGeneric : MonoBehaviour
{
    public Camera camera;
    public Transform target;
    public float followDistance = 5;
    public float followHeight = 3;

    public float followRotation = 20.0f;
    private Vector3 rotationVector;

    private Vector3 offset = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        //set the initial camera rotation downward on start
        rotationVector = new Vector3(followRotation, 0.0f, 0.0f);
        camera.transform.Rotate(rotationVector);  

        FollowStart();
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    private void FollowStart()
    {
        //set the camera to the targets position minus a set distance
        transform.position = target.transform.position - (target.transform.forward * followDistance);
        //do the same for height
        transform.position += (target.transform.up * followHeight);

        //store the offset to keep the camera at this distance
        offset = transform.position - target.transform.position;        
    }

    //keep the camera at exact vector from player until camera rotates
    private void Follow()
    {
        transform.position = target.transform.position + offset;
    }
}
