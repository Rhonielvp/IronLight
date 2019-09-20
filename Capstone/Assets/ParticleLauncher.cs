using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rob
//Followed a tutorial to get some more advanced particles
public class ParticleLauncher : MonoBehaviour
{
    [SerializeField] private float beamAttackDrain;
    [SerializeField] private ParticleSystem mainParticle;
    [SerializeField] private ParticleSystem collisionParticle;

    //player health script
    PlayerHealthBehaviour playerHealthScript;

    public Gradient particleColourGradiant;

    //list to store collision event information for physics purposes
    private List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();

    private bool isFiring = false;

    //coroutine
    Coroutine emitterFollowCoroutine = null;


    private void Start()
    {
        //need access to player health so can adjust shit, is parent so okay
        playerHealthScript = GetComponentInParent<PlayerHealthBehaviour>();

        //prevent playing on start
        mainParticle.Stop();
    }


    // Update is called once per frame
    void Update()
    {
        //left shift
        if(Input.GetButtonDown("Fire3") && isFiring == false)
        {
            isFiring = true;            

            //start particles
            mainParticle.Play();

            //gets a reference to the main module of the particle system
            ParticleSystem.MainModule psMain = mainParticle.main;

            //NOT WORKING
            //gets a random colour from the gradiant attached 
            float temp = Random.Range(0.0f, 1.0f);
            Debug.Log("Random: " + temp);
            psMain.startColor = particleColourGradiant.Evaluate(temp);

            //allows emitting number of particles per frame
            //mainParticle.Emit(1);

            //start follow coroutine
            emitterFollowCoroutine = StartCoroutine(EmitterFollowPlayer());

            //lower health
            playerHealthScript.StartBeamAttackDrain();
            Debug.Log("START beam attack");
        }

        if(Input.GetButtonUp("Fire3"))
        {
            //stop particle emmision
            mainParticle.Stop();

            //stop emitter follow
            StopCoroutine(emitterFollowCoroutine);

            //stop drain coroutine
            playerHealthScript.StopBeamAttackDrain();
            Debug.Log("STOP beam attack");

            isFiring = false;
        }
    }


    IEnumerator EmitterFollowPlayer()
    {
        while(true)
        {
            //set particle system active at current location
            mainParticle.transform.position = transform.position;
            mainParticle.transform.rotation = Quaternion.LookRotation(transform.forward);

            yield return null;
        }
    }

    
    


    //FOR ADDING EXTRA EFFECTS LATER ON
    //just like on trigger/collision enter but for particles
    private void OnParticleCollision(GameObject other)
    {
        //will store in our list a bunch of collision data
        ParticlePhysicsExtensions.GetCollisionEvents(mainParticle, other, events);

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
