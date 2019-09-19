using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Mohamad
//Player Dash ability

public class Dashing : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public float m_Speed = 20.0f;

    public float dashTime = 1;
    public float timer = 0;


    void Start()
    {
        //Fetch the Rigidbody component you attach from your GameObject
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D) && timer >= dashTime)
        {
            Debug.Log("D");
            //Move the Rigidbody to the right constantly at speed you define (the red arrow axis in Scene view)
            m_Rigidbody.velocity = transform.right * m_Speed;
            timer = 0;

        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            m_Rigidbody.velocity = Vector3.zero;
        }

        
        // if (Input.GetKey(KeyCode.LeftArrow))
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.A) && timer >= dashTime)
        {
            Debug.Log("A");
            //Move the Rigidbody to the left constantly at the speed you define (the red arrow axis in Scene view)
            m_Rigidbody.velocity = -transform.right * m_Speed;
            timer = 0;

        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            m_Rigidbody.velocity = Vector3.zero;
        }

        
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W) && timer >= dashTime)
        {
            Debug.Log("W");
            //rotate the sprite about the Z axis in the positive direction
            // transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * m_Speed, Space.World);
            m_Rigidbody.velocity = transform.forward * m_Speed;
            timer = 0;

        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            m_Rigidbody.velocity = Vector3.zero;
        }

        
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S) && timer >= dashTime)
        {
            Debug.Log("S");
            m_Rigidbody.velocity = -transform.forward * m_Speed;
            timer = 0;

        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            m_Rigidbody.velocity = Vector3.zero;
        }
    }
}
