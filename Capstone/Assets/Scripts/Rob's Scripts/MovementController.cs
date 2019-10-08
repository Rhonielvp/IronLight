using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject cameraPivot;

    //this value needs to between 0 and 1 used for rotation
    [Range(0.01f, 0.99f)] [SerializeField] private float axisControl;

    //store the most recent player rotation
    Quaternion mostRecentRotation;


    [SerializeField] private float maxSpeed;
    private float maxSpeedOriginal;
    //[SerializeField] private float forwardForce;
    //private float forceForwardOriginal;
    //[SerializeField] private float sidewaysForce;
    //private float sidewaysForceOriginal;

    [Range(0.01f, 0.5f)] [SerializeField] private float rotationSpeed;

    private float horizontalInput;
    private float verticalInput;


    [SerializeField] private bool canDash = true;
    [SerializeField] private float dashMultiplier;
    [SerializeField] private float dashResetTime;
    [SerializeField] private float accelerationTime;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //save the max speed being set, in the future this will get saved in
        //a seperate script for player attributes 
        maxSpeedOriginal = maxSpeed;
        //forceForwardOriginal = forwardForce;
        //sidewaysForceOriginal = sidewaysForce;
    }

    //this is for better handling physics movement, try placing move in here
    private void FixedUpdate()
    {
        Dash();
        Move();
    }

    private void Move()
    {
        //grab input speeds        
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //makes more effecient by skipping through
        if (Mathf.Abs(horizontalInput) <= axisControl && Mathf.Abs(verticalInput) <= axisControl)
        {
            //stop all velocity... in just x/z 
            rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);

            //set rotation to the most recent stored rotation
            transform.rotation = mostRecentRotation;

            return;
        }
        else
        {
            //MOVEMENT
            //move the body through force not transform, why??
            //be sure to use the main camera transform to ensure relevant direction
            Vector3 horizontalMovement = cameraPivot.transform.right * maxSpeed * horizontalInput;
            Vector3 verticalMovement = cameraPivot.transform.forward * maxSpeed * verticalInput;

            //guy recommended this as best movement 
            rb.MovePosition(transform.position + (horizontalMovement + verticalMovement) * Time.fixedDeltaTime);


            //place movement into 2d vector and check magnitude so to not effect jumping
            Vector2 check = new Vector2(rb.velocity.x, rb.velocity.z);


            //if magnitude greater than max speed than set magnitude to max speed
            //than set rb velocity to new speed but keep y velocity intact
            if (check.magnitude > maxSpeed)
            {
                check = check.normalized * maxSpeed;
                rb.velocity = new Vector3(check.x, rb.velocity.y, check.y);
            }

            //ROTATION            
            //Vector3 rotationDirection = new Vector3(horizontalInput, 0.0f, verticalInput);                
            Vector3 rotationDirection = (cameraPivot.transform.right * horizontalInput) + (cameraPivot.transform.forward * verticalInput);
            Rotate(rotationDirection);
        }
    }


    //ROTATION
    //rotate but maintain direction facing
    private void Rotate(Vector3 rotationDirection)
    {
        //rotate to face the direction you are moving 
        rotationDirection = new Vector3(rotationDirection.x, 0,rotationDirection.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotationDirection), rotationSpeed);
        //transform.rotation = Quaternion.LookRotation(rotationDirection);
        mostRecentRotation = transform.rotation;
    }


    //DASH
    //enable player speed boost
    private void Dash()
    {
        //right mouse click or left alt
        if (Input.GetButtonDown("Fire2") && canDash == true)
        {
            canDash = false;

            //forwardForce *= dashMultiplier;
            //sidewaysForce *= dashMultiplier;
            StartCoroutine(DashAcceleration());
        }

        //dash button comes up
        if (Input.GetButtonUp("Fire2"))
        {
            //forwardForce = forceForwardOriginal;
            //sidewaysForce = sidewaysForceOriginal;
            maxSpeed = maxSpeedOriginal;

            //start couroutine to set canDash back to true
            StartCoroutine(DashReset());
        }
    }

    //lerp from speed to dash speed
    IEnumerator DashAcceleration()
    {
        float count = 0.0f;
        while (count < accelerationTime)
        {
            //increment count slightly 
            count += Time.deltaTime;

            //lerp that percent 
            //max sure to be using the const original z
            maxSpeed = Mathf.Lerp(maxSpeedOriginal, maxSpeedOriginal * dashMultiplier, count / accelerationTime);
            //forwardForce = maxSpeed;
            //sidewaysForce = maxSpeed;

            yield return null;
        }
        maxSpeed = maxSpeedOriginal;
    }

    //timer to wait for dash to reset
    IEnumerator DashReset()
    {
        yield return new WaitForSecondsRealtime(dashResetTime);

        canDash = true;
    }
}
