using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Rob
//Followed a tutorial to get some more advanced particles
public class PlayerAttackController : MonoBehaviour
{
    private enum Attacking { None, Beam, Explosion };
    private Attacking currentAttack = Attacking.None;

    [SerializeField] private Transform cameraPlayer;
    [SerializeField] private float beamAttackDrain;
    [SerializeField] private float explosionAttackDrain;
    [SerializeField] private ParticleSystem beamParticle;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private ParticleSystem collisionParticle;

    //Attack Charge Script
    AttackCharge charge;
        
    //player health script
    PlayerHealthBehaviour playerHealthScript;

    public Gradient particleColourGradiant;

    //list to store collision event information for physics purposes
    private List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();

    
    //coroutine
    Coroutine emitterFollowCoroutine = null;


    private void Start()
    {
        //get access to attack charge which is in parent
        charge = GetComponentInParent<AttackCharge>();

        //need access to player health so can adjust shit, is parent so okay
        playerHealthScript = GetComponentInParent<PlayerHealthBehaviour>();

        //prevent playing on start
        beamParticle.Stop();
        explosionParticle.Stop();
    }


    // Update is called once per frame
    void Update()
    {
        //left shift
        if(Input.GetButtonDown("Fire3") && currentAttack == Attacking.None)
        {
            currentAttack = Attacking.Beam;
            BeamAttack();
        }

        if(Input.GetButtonUp("Fire3"))
        {
            StopBeamAttack();
            currentAttack = Attacking.None;
        }

        //left ctrl
        if (Input.GetButtonDown("Fire1") && currentAttack == Attacking.None)
        {
            currentAttack = Attacking.Explosion;
            ExplosionAttack();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopExplosionAttack();
            currentAttack = Attacking.None;
        }        
    }

    private void BeamAttack()
    {        
        //start particles
        beamParticle.Play();

        //gets a reference to the main module of the particle system
        ParticleSystem.MainModule psMain = beamParticle.main;

        //NOT WORKING
        //gets a random colour from the gradiant attached 
        float temp = Random.Range(0.0f, 1.0f);
        //Debug.Log("Random: " + temp);
        psMain.startColor = particleColourGradiant.Evaluate(temp);

        //allows emitting number of particles per frame
        //mainParticle.Emit(1);

        //start follow coroutine
        emitterFollowCoroutine = StartCoroutine(EmitterFollowPlayer());

        //lower health
        playerHealthScript.StartBeamAttackDrain();
        //Debug.Log("START beam attack");
    }

    private void StopBeamAttack()
    {
        //stop particle emmision
        beamParticle.Stop();

        //stop emitter follow
        StopCoroutine(emitterFollowCoroutine);

        //stop drain coroutine
        playerHealthScript.StopBeamAttackDrain();
        Debug.Log("STOP beam attack");
    }

    private void ExplosionAttack()
    {
        //check if player has charged and focus is available
        Debug.Log("<color=orange>ChargePercentage: </color>" + charge.GetIsCharging());
        if(charge.GetIsCharging())
        {
            //set position and rotation of particle effect
            explosionParticle.transform.position = transform.position;
            explosionParticle.transform.rotation = Quaternion.LookRotation(transform.forward);

            //emit only 1 particle
            explosionParticle.Play();

            //adjust player health, send negative
            playerHealthScript.ModifyHealth(-(playerHealthScript.currentHealth * charge.GetFocusPercentage()));
        }              
    }

    private void StopExplosionAttack()
    {
        explosionParticle.Stop();
    }


    IEnumerator EmitterFollowPlayer()
    {
        while(true)
        {
            //set particle system active at current location
            beamParticle.transform.position = transform.position;
            beamParticle.transform.rotation = Quaternion.LookRotation(transform.forward);
            Debug.Log("<color=purple>Angle: </color>" + cameraPlayer.transform.eulerAngles.x);

            //adjust angle if negative, means the player is looking up
            if (cameraPlayer.transform.eulerAngles.x > 45f) 
            {
                float xRotation = cameraPlayer.transform.eulerAngles.x;
                transform.eulerAngles = new Vector3(xRotation, transform.eulerAngles.y, transform.eulerAngles.z);
            }  

            yield return null;
        }
    }

    
    
    //-------------------------------------------------------------

    //FOR ADDING EXTRA EFFECTS LATER ON
    //just like on trigger/collision enter but for particles
    private void OnParticleCollision(GameObject other)
    {
        //will store in our list a bunch of collision data
        ParticlePhysicsExtensions.GetCollisionEvents(beamParticle, other, events);

        for (int i = 0; i < events.Count; i++)
        {
            EmitAtLocation(events[i]);
        }
    }


    //render particles at collision location
    private void EmitAtLocation(ParticleCollisionEvent p)
    {
        //position and rotate the particle before it emits
        //intersection is just the world space location of the collision
        collisionParticle.transform.position = p.intersection;

        //DAM rotate to the rotation of the normal so it looks legit
        collisionParticle.transform.rotation = Quaternion.LookRotation(p.normal);

        //gets a reference to the main module of the particle system
        ParticleSystem.MainModule psMain = collisionParticle.main;

        //gets a random colour from the gradiant attached 
        psMain.startColor = particleColourGradiant.Evaluate(Random.Range(0f, 1f));

        //emits set number of particles
        collisionParticle.Emit(1);
    }
}
