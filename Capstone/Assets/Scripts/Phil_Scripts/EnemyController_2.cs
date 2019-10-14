//Name = Phil James
//Capstone 2019 - 
//Note : This script (EnemyController_2) is for Testing purposes Only ! to Test the Player 2.. Ignore this Script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// -----------------------------------------------------
// Programmer : Phil
// Desc	:	These are the basic AI States
//			dynamic design where this script can be use into
//			two Functionality , Patrol / Idle.
// -----------------------------------------------------
public enum EnemyState2
{
    PATROL,
    IDLE,
    CHASE,
    ATTACK
}
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class EnemyController_2 : MonoBehaviour
{

    private EnemyAnimator enemy_Anim;
    public NavMeshAgent navAgent;

    public EnemyState2 enemy_State;

    public float walk_Speed = 0.5f;
    public float run_Speed = 4f;

    public float chase_Distance = 7f;
    private float current_Chase_Distance;
    public float attack_Distance = 1.8f;
    public float chase_After_Attack_Distance = 2f;

    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_For_This_Time = 15f;
    private float patrol_Timer;

    public float wait_Before_Attack = 2f;
    private float attack_Timer;

    private Transform target;

    public GameObject attack_Point;

    private EnemyAudio enemy_Audio;

    //++++++++++++++++++++++++++++++++++++
    // Inspector Assigned Variable
    public AIWaypointNetwork WaypointNetwork = null;
    public int CurrentIndex = 0;
    public bool HasPath = false;
    public bool PathPending = false;
    public bool PathStale = false;
    public UnityEngine.AI.NavMeshPathStatus PathStatus = UnityEngine.AI.NavMeshPathStatus.PathInvalid;
    public AnimationCurve JumpCurve = new AnimationCurve();

    private float _originalMaxSpeed = 0;

   
    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    //Observer Call
    //Observer _observer;
    //delegate void AlertHandler();
    //AlertHandler _alertHandler;

    //Request By Brian - This changing Color Signify the Enemy is Attacking
    Material skinMaterial;
    Color originalColour;



    private Transform Enem_Col;

    void Awake()
    {
        enemy_Anim = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();

        skinMaterial = GetComponent<Renderer>().material;
        originalColour = skinMaterial.color;

        target = GameObject.FindWithTag("Player2").transform;

        enemy_Audio = GetComponentInChildren<EnemyAudio>();

        //for the sake imans requirement
        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

    }

    // Use this for initialization
    void Start()
    {

        enemy_State = EnemyState2.PATROL;

        patrol_Timer = patrol_For_This_Time;

        // when the enemy first gets to the player
        // attack right away
        attack_Timer = wait_Before_Attack;

        // memorize the value of chase distance
        // so that we can put it back
        current_Chase_Distance = chase_Distance;


        // If not valid Waypoint Network has been assigned then return
        if (WaypointNetwork == null) return;


        // Set first waypoint
        SetNextDestination(false);

        
        //Transferring to Class enemyHealth
        //_observer = new Observer();
        //_alertHandler += new AlertHandler(_observer.OnNotify);

    }

    // Update is called once per frame
    void Update()
    {

        SetNewRandomDestination();

        if (enemy_State == EnemyState2.IDLE)
        {
            Idle();
        }

        if (enemy_State == EnemyState2.PATROL)
        {
            Patrol();
        }

        if (enemy_State == EnemyState2.CHASE)
        {
            Chase();
        }

        if (enemy_State == EnemyState2.ATTACK)
        {
            Attack();
            //       _alertHandler();
        }
        //    Physics.IgnoreLayerCollision(Enemy.collider, Enemy.collider);

        //if (shaking)
        //{
        //    Vector3 newPos = Random.insideUnitSphere * (Time.deltaTime * shakeAmt);
        //    newPos.y = transform.position.y;
        //    newPos.z = transform.position.z;
        //    transform.position = newPos;
        //}
    }

    // -----------------------------------------------------
    // Programmer : Phil
    // Desc	:	NotFriendly , This Function Call
    //			set the behavior of the Enemy(Patrol)
    //			If they Collide into another Enemy Agent along their way, They will Bump each other and will be 
    //          seperted on each new Path destination.
    // -----------------------------------------------------
    public void EnemyFightEnemy()
    {
        StartCoroutine("NotFriendly");
    }
    //IEnumerator NotFriendly()
    //{
    //    OffMeshLinkData data = navAgent.currentOffMeshLinkData;
    //    Vector3 startPos = Enem_Col.transform.position;
    //    Vector3 endPos = data.endPos + Vector3.up * navAgent.baseOffset;
    //    float normalizedTime = 0.0f;
    //    while (normalizedTime < 1.0f)
    //    {
    //        float yOffset = 0.5f * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
    //        navAgent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
    //        normalizedTime += Time.deltaTime / 0.5f;
    //        yield return null;
    //    }
    //}
    IEnumerator NotFriendly()
    {

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (Enem_Col.position - transform.position).normalized;
        Vector3 attackPosition = Enem_Col.position - dirToTarget * (myCollisionRadius);

        float attackSpeed = 3; float percent = 0;

        //// Calculatehow much the current waypoint index needs to be incremented
        //int incStep = 1;
        //Transform nextWaypointTransform = null;

        //// Calculate index of next waypoint factoring in the increment with wrap-around and fetch waypoint 
        //int nextWaypoint = (CurrentIndex + incStep >= WaypointNetwork.Waypoints.Count) ? 0 : CurrentIndex + incStep;
        //nextWaypointTransform = WaypointNetwork.Waypoints[nextWaypoint];
        // PathStale = true;

        while (percent <= 1)
        {

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 10f;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);


            //// Assuming we have a valid waypoint transform
            //if (nextWaypointTransform != null)
            //{
            //    // Update the current waypoint index, assign its position as the NavMeshAgents
            //    // Destination and then return
            //    CurrentIndex = nextWaypoint;
            //    navAgent.destination = nextWaypointTransform.position;
            // }

            float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);

            Vector3 randDir = Random.insideUnitSphere * rand_Radius;
            randDir += transform.position;

            NavMeshHit navHit;

            NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1);

            navAgent.SetDestination(navHit.position);
            yield return null;
        }

        //  navAgent.destination = nextWaypointTransform.position;
        //enemy_State = EnemyState.PATROL;
        //Patrol();

    }

    // -----------------------------------------------------
    // Programmer : Phil
    // Desc	:	Optionally increments the current waypoint
    //			index and then sets the next destination
    //			for the agent to head towards.
    // -----------------------------------------------------
    void SetNextDestination(bool increment)
    {
        // If no network return
        if (!WaypointNetwork) return;

        // Calculatehow much the current waypoint index needs to be incremented
        int incStep = increment ? 1 : 0;
        Transform nextWaypointTransform = null;

        // Calculate index of next waypoint factoring in the increment with wrap-around and fetch waypoint 
        int nextWaypoint = (CurrentIndex + incStep >= WaypointNetwork.Waypoints.Count) ? 0 : CurrentIndex + incStep;
        nextWaypointTransform = WaypointNetwork.Waypoints[nextWaypoint];

        // Assuming we have a valid waypoint transform
        if (nextWaypointTransform != null)
        {
            // Update the current waypoint index, assign its position as the NavMeshAgents
            // Destination and then return
            CurrentIndex = nextWaypoint;
            navAgent.destination = nextWaypointTransform.position;
            return;
        }

        // We did not find a valid waypoint in the list for this iteration
        CurrentIndex = nextWaypoint;
    }

    private void OnCollisionEnter(Collision other)
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        //Precaution - if multiple Enemy Collide one another in very small path way(ex Door)
        //Stuck-up Agent send to a new Destination
        if (other.transform.tag == Tags.ENEMY_TAG)
        {
            if (enemy_State == EnemyState2.PATROL)
            {
                navAgent.velocity = Vector3.zero;
                navAgent.isStopped = true;

                Debug.Log("Collide Now");

                //Rotation
                float smoothRot = -1.0f; float smoothSpeed = -10.0f;

                Vector3 smoothedPosition = Vector3.Lerp(transform.position, other.transform.position, smoothSpeed * Time.deltaTime);
                transform.position = smoothedPosition;

                Quaternion smoothedRot = Quaternion.Slerp(transform.rotation, other.transform.rotation, smoothRot * Time.deltaTime);
                transform.rotation = smoothedRot;
                //---------------------------


                Enem_Col = other.transform;
                EnemyFightEnemy();  //We need this as Precaustion when the NavMesh Agent Stuck-up then we need to Force new Transform Position on both Enemy

                SetNextDestination(true);
            }
        }

    }
    void Idle()
    {


        // test the distance between the player and the enemy
        if (Vector3.Distance(transform.position, target.position) <= chase_Distance)
        {

            enemy_Anim.Walk(false);
            enemy_State = EnemyState2.CHASE;


            // Ready For Future Enhancement - We can put the Voice Narrator Ex. the Midnight Enemy "shouting " I Will Squash You!!"
            // play spotted audio
            enemy_Audio.Play_ScreamSound();

        }


    } // Idle

    void Patrol()
    {
        skinMaterial.color = originalColour;
        // tell nav agent that he can move
        navAgent.isStopped = false;
        navAgent.speed = walk_Speed;

        // add to the patrol timer
        patrol_Timer += Time.deltaTime;

        if (patrol_Timer > patrol_For_This_Time)
        {

            // If we don't have a path and one isn't pending then set the next
            // waypoint as the target, otherwise if path is stale regenerate path
            if ((navAgent.remainingDistance <= navAgent.stoppingDistance && !PathPending) || PathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid /*|| PathStatus==NavMeshPathStatus.PathPartial*/)
            {
                SetNextDestination(true);

            }
            else
            if (navAgent.isPathStale)
            {
                SetNextDestination(false);
            }
            patrol_Timer = 0f;

        }

        // Ready for Future Enhancement - to manage Animation Controller
        if (navAgent.velocity.sqrMagnitude > 0)
        {

            enemy_Anim.Walk(true);  //in-case need to put Animation Walking for a particular Enemy(Ex. a Fog/smoke at the bottom of Enemy)

        }
        else
        {

            enemy_Anim.Walk(false);

        }

        // test the distance between the player and the enemy
        if (Vector3.Distance(transform.position, target.position) <= chase_Distance)
        {

            enemy_Anim.Walk(false);
            enemy_State = EnemyState2.CHASE;

            // Debug.Log("AI Enemy State = CHASE!!");

            // Ready For Future Enhancement - We can put the Voice Narrator Ex. the Midnight Enemy "shouting " I Will Squash You!!"
            // play spotted audio
            enemy_Audio.Play_ScreamSound();
            skinMaterial.color = Color.red;
        }


    } // patrol

    public void Chase()
    {

        // enable the agent to move again
        navAgent.isStopped = false;
        navAgent.speed = run_Speed;
        enemy_State = EnemyState2.CHASE;

        //  Debug.Log("AI Enemy CHASING! the Player Now!");

        // set the player's position as the destination
        // because we are chasing(running towards) the player
        navAgent.SetDestination(target.position);

        skinMaterial.color = Color.red;  // This is means Enemy was Allerted


        // Ready For Future Enhancement - We can put the Voice Narrator Ex. the Midnight Enemy "shouting " I Will Squash You!!"
        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Run(true);

        }
        else
        {
            enemy_Anim.Run(false);
            skinMaterial.color = originalColour;
        }

        // if the distance between enemy and player is less than attack distance
        if (Vector3.Distance(transform.position, target.position) <= attack_Distance)
        {

            // stop the animations
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);
            enemy_State = EnemyState2.ATTACK;

            //  Debug.Log("AI Enemy ATTACK!!");

            // reset the chase distance to previous
            if (chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }

        }
        else if (Vector3.Distance(transform.position, target.position) > chase_Distance)
        {
            // player run away from enemy

            // stop running
            enemy_Anim.Run(false);

            if (WaypointNetwork == null)
            {
                enemy_State = EnemyState2.IDLE;
            }
            else
            {
                enemy_State = EnemyState2.PATROL;
            }


            //    Debug.Log("AI Enemy State = PATROL!!");

            // reset the patrol timer so that the function
            // can calculate the new patrol destination right away
            patrol_Timer = patrol_For_This_Time;

            // reset the chase distance to previous
            if (chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }
            skinMaterial.color = originalColour;

        } // else

    } // chase

    void Attack()
    {

        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attack_Timer += Time.deltaTime;

        if (attack_Timer > wait_Before_Attack)
        {

            enemy_Anim.Attack();  // Ready For Future Enhancement

            attack_Timer = 0f;
            enemy_State = EnemyState2.ATTACK;

            // Ready For Future Enhancement - We can put the Voice Narrator Ex. the Midnight Enemy "shouting " I Will Squash You!!"
            // play attack sound
            enemy_Audio.Play_AttackSound();

            skinMaterial.color = Color.red;

            // Request By - Brian Baran
            // Calculate the Distance - Then Apply Cooldown Attack "buffer" before the next Attack.!
            float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
            if (sqrDstToTarget < Mathf.Pow(attack_Distance + myCollisionRadius + targetCollisionRadius, 2))
            {
                nextAttackTime = Time.time + wait_Before_Attack;
                Debug.Log("Cooling Before next Attack.!");
                StartCoroutine(Attack_bouncy());
            }

        }
        // If the Player Runaway , then validate the chase_Distance
        if (Vector3.Distance(transform.position, target.position) > chase_Distance)
        {

            enemy_State = EnemyState2.PATROL;

            Patrol();
        }
        else if (Vector3.Distance(transform.position, target.position) >
           attack_Distance + chase_After_Attack_Distance)
        {

            enemy_State = EnemyState2.CHASE;
            Chase();
            //    Debug.Log("AI Enemy State = CHASE!!");
        }


    } // attack

    //Cooldown attack
    IEnumerator Attack_bouncy()
    {

        enemy_State = EnemyState2.ATTACK;
        // navAgent.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;

        // Phil Commented this, transferring this into the Class enemyHealth
        //   _alertHandler();

        while (percent <= 1)
        {

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        skinMaterial.color = originalColour;
        enemy_State = EnemyState2.CHASE;
        Chase();
        //   navAgent.enabled = true;
    }
    void SetNewRandomDestination()
    {


        enemy_State = EnemyState2.PATROL;


        //float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);

        //Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        //randDir += transform.position;

        //NavMeshHit navHit;

        //NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1);

        //navAgent.SetDestination(navHit.position);

        int turnOnSpot;

        // Copy NavMeshAgents state into inspector visible variables
        HasPath = navAgent.hasPath;
        PathPending = navAgent.pathPending;
        PathStale = navAgent.isPathStale;
        PathStatus = navAgent.pathStatus;

        // Perform corss product on forard vector and desired velocity vector. If both inputs are Unit length
        // the resulting vector's magnitude will be Sin(theta) where theta is the angle between the vectors.
        Vector3 cross = Vector3.Cross(transform.forward, navAgent.desiredVelocity.normalized);

        // If y component is negative it is a negative rotation else a positive rotation
        float horizontal = (cross.y < 0) ? -cross.magnitude : cross.magnitude;

        // Scale into the 2.32 range for our animator
        horizontal = Mathf.Clamp(horizontal * 2.32f, -2.32f, 2.32f);

        // If we have slowed down and the angle between forward vector and desired vector is greater than 10 degrees 
        if (navAgent.desiredVelocity.magnitude < 1.0f && Vector3.Angle(transform.forward, navAgent.steeringTarget - transform.position) > 10.0f)
        {
            // Stop the nav agent (approx) and assign either -1 or +1 to turnOnSpot based on sign on horizontal
            navAgent.speed = 10f;
            turnOnSpot = (int)Mathf.Sign(horizontal);
        }
        else
        {
            // Otherwise it is a small angle so set Agent's speed to normal and reset turnOnSpot
            navAgent.speed = _originalMaxSpeed;
            turnOnSpot = 0;
        }

        // Programmer :  Phil
        // This is for NPC
        // If agent is on an offmesh link then perform a jump
        /*if (_navAgent.isOnOffMeshLink)
		{
			StartCoroutine( Jump( 1.0f) );
			return;
		}*/


    }

    void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }

    void Turn_Off_AttackPoint()
    {
        if (attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }

    public EnemyState Enemy_State
    {
        get; set;
    }

    // For Future Enhancement - Still not Working
    // ---------------------------------------------------------
    // Programer  :  Phil
    // Function	  :	 Jump
    // Desc	      :	 Manual OffMeshLInk traversal using an Animation
    //			Curve to control agent height.
    // ---------------------------------------------------------
    IEnumerator Jump(float duration)
    {
        // Get the current OffMeshLink data
        UnityEngine.AI.OffMeshLinkData data = navAgent.currentOffMeshLinkData;

        // Start Position is agent current position
        Vector3 startPos = navAgent.transform.position;

        // End position is fetched from OffMeshLink data and adjusted for baseoffset of agent
        Vector3 endPos = data.endPos + (navAgent.baseOffset * Vector3.up);

        // Used to keep track of time
        float time = 0.0f;

        // Keeo iterating for the passed duration
        while (time <= duration)
        {
            // Calculate normalized time
            float t = time / duration;

            // Lerp between start position and end position and adjust height based on evaluation of t on Jump Curve
            navAgent.transform.position = Vector3.Lerp(startPos, endPos, t) + (JumpCurve.Evaluate(t) * Vector3.up);

            // Accumulate time and yield each frame
            time += Time.deltaTime;
            yield return null;
        }

        // NOTE : Added this for a bit of stability to make sure the
        //        Agent is EXACTLY on the end position of the off mesh
        //		  link before completeing the link.
        navAgent.transform.position = endPos;

        // All done so inform the agent it can resume control
        navAgent.CompleteOffMeshLink();
    }
} // class


































