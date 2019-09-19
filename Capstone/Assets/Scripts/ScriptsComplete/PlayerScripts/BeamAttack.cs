using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject lanturnEmitter;
    [SerializeField] private float beamDamage;
    [SerializeField] private float beamRange;

    [SerializeField] private LayerMask enemyLayer;

    private EnemyHealth enemyHealthScript;


    
    void Update ()
    {
        {
            if (Input.GetButton("Fire1"))
            {
                Shoot();
            }
            else
            {
                enemyHealthScript.StopHealing();
            }
        }
    }

    //Original Shoot
    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, beamRange))
        {
            enemyHealthScript = hit.transform.GetComponent<EnemyHealth>();

            if(hit.collider)
            {
                Debug.Log(hit.transform.name);
            }
            
            Debug.Log("raycast working");
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("raycast hitting");
                enemyHealthScript.StartHealing(-3);
            }
            Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.green);
        }
    }

    //alternate shoot
    private void ShootBeam()
    {
        //turn on particle effect
    }
}
