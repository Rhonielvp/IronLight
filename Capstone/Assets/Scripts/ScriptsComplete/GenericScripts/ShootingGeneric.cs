using UnityEngine;
using System.Collections;

//Generic shooting script that can be placed on player or enemies
public class ShootingGeneric : MonoBehaviour
{
    //Spawn Point
    [SerializeField] private GameObject bulletEmitter;

    //Object 
    [SerializeField] private GameObject bullet;

    //Force
    [SerializeField] private float bulletForce;

    //Bullet Damage
    [SerializeField] private float bulletDmg;


    private void Update()
    {
        //mouse click right
        if (Input.GetMouseButtonDown(1))
        {
            Shooting();
        }
    }


    public virtual void Shooting()
    {
        //The Bullet instantiation happens here.
        GameObject bulletTemp = Instantiate(bullet, bulletEmitter.transform.position, bulletEmitter.transform.rotation) as GameObject;

        //Sometimes bullets may appear rotated incorrectly due to the way its pivot was set from the original modeling package.
        //This is EASILY corrected here, you might have to rotate it from a different axis and or angle based on your particular mesh.
        bulletTemp.transform.Rotate(Vector3.left * 90);

        //Retrieve the Rigidbody component from the instantiated Bullet and control it.
        Rigidbody rbTemp;
        rbTemp = bulletTemp.GetComponent<Rigidbody>();

        //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
        rbTemp.AddForce(transform.forward * bulletForce);

        //Basic Clean Up, set the Bullets to self destruct after 1 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
        Destroy(bulletTemp, 1.0f);
    }    
}
