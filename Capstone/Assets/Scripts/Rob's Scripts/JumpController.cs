using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ROb
//script to control the jumping, make it seperate from the movement for cleaner code and
//more control.
public class JumpController : MonoBehaviour
{
    private Rigidbody rb;
    private RaycastHit hit;
    
    [SerializeField] private bool isGrounded;

    //allow devs to determine total amount of jumps
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDuration;
    private float count = 0.0f;

    [SerializeField] private int maxJumps;
    private int jumps;
    private bool isJump = false;


    // Start is called before the first frame update
    void Start()
    {
        //set rigidbody to rb on gameobject
        rb = GetComponent<Rigidbody>();

        //set jumps equal to total jumps
        jumps = maxJumps;        
    }

    
    private void FixedUpdate()
    {
        //can only jump if jumps available and if count is less than jump duration
        if (Input.GetButton("Jump") && jumps > 0 && count < jumpDuration && isGrounded == true)
        {
            //if player is not in the state of jumping than set their y velocity to zero
            //this will enable double jumping smoothly 
            if(isJump == false)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            }

            isJump = true;

            //jump         
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);

            //increment count
            count += Time.deltaTime;            
        }

        //when button comes up it means that 1 jump has occured
        if (Input.GetButtonUp("Jump") && isJump == true)
        {
            //ensures this only runs once
            isJump = false;

            //reset count
            count = 0.0f;

            //if player is in the air than decrement jumps to indicate jump used
            if(isGrounded == false)
            {
                //decrement jumps
                jumps--;
            }
        }
    }

    //take input from ground check objects
    public void SetGrounded(bool set)
    {
        isGrounded = set;

        //if player gets grounded than reset jumps 
        if(isGrounded)
        {
            jumps = maxJumps;
        }
    }


    //**********************************
    //FROM OTHER SCRIPT MIGHT NOT USE
    //make player same angle as ground
    private void OrientatePlayer()
    {
        //constantly get raycast
        Physics.Raycast(transform.position, Vector3.down, out hit, 2);

        //update grounded
        if (hit.collider)
        {
            isGrounded = true;
            jumps = 0;

            //adjust players angle to imitate the angle of the platform that it is on
            transform.rotation = Quaternion.Lerp(transform.rotation, hit.transform.rotation, 0.1f);
        }
        else
        {
            isGrounded = false;
        }
    }
}
