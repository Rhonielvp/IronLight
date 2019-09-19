using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rob
//Followed a tutorial to get some more advanced particles
public class ParticleLauncher : MonoBehaviour
{
    [SerializeField] private ParticleSystem mainParticle;
    [SerializeField] private ParticleSystem collisionParticle;

    public Gradient particleColourGradiant;

    //list to store collision event information for physics purposes
    private List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();


    private void Start()
    {
        //prevent playing on start
        mainParticle.Stop();
    }


    // Update is called once per frame
    void Update()
    {
        //left shift
        if(Input.GetButton("Fire3"))
        {
            //set particle system active at current location
            mainParticle.transform.position = transform.position;
            mainParticle.transform.rotation = Quaternion.LookRotation(transform.forward);

            if(mainParticle.isPaused)
            {
                mainParticle.Play();
            }            

            //gets a reference to the main module of the particle system
            ParticleSystem.MainModule psMain = mainParticle.main;

            //gets a random colour from the gradiant attached 
            float temp = Random.Range(0.0f, 1.0f);
            Debug.Log("Random: " + temp);
            psMain.startColor = particleColourGradiant.Evaluate(temp);
                        
            //allows up to emit an amount of particles per frame
            mainParticle.Emit(1);
        }

        if(Input.GetButtonUp("Fire3"))
        {
            mainParticle.Stop();
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


}
