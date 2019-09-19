using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rob
//Moves any object from a start to end position
//To be used with objects that move or get triggered with puzzles
//can toggle isStart to indicate that the object either starts in build position or will move
//there after trigger
public class MoveObject : MonoBehaviour
{
    //used to compare and find what triggers this object
    public string triggerGroup;

    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private bool isStart;
    [SerializeField] private Vector3 displacement;
    [SerializeField] private float movementTime;

    private bool isCoroutine = false;


    // Use this for initialization
    void Start()
    {
        SetPosition();
    }


    //public method for outside shit to call 
    public void Activate()
    {
        StartCoroutine(Move(startPosition, endPosition, movementTime));
    }

    //moves and object from pointA to point B over a amount of seconds
    IEnumerator Move(Vector3 pointA, Vector3 pointB, float movementTime)
    {
        //is is coroutine is true than it means it's already running
        //don't allow call again
        if (!isCoroutine)
        {
            Debug.Log("Moving");
            isCoroutine = true;
            float count = 0.0f;

            //until count reaches specified time keep moving closer and closer
            //note that we keep the same start and end pos so to constantly get a closer to 1 value
            while (count < movementTime)
            {
                count += Time.deltaTime;
                transform.position = Vector3.Lerp(pointA, pointB, count / movementTime);

                yield return null;
            }

            //when pops out of while loop end coroutine
            StopCoroutine("Move");
            isCoroutine = false;
            Debug.Log("Stop Coroutine");
        }
        else
        {
            //exit
            Debug.Log("Already active coroutine");
            yield break;
        }

    }

    //can be used to have objects start where the designers want them to end after moving
    private void SetPosition()
    {
        //allows designers to toggle where the object starts or finishes after trigger
        if (isStart)
        {
            startPosition = transform.position;
            endPosition = startPosition + displacement;
        }
        else
        {
            endPosition = transform.position;
            startPosition = endPosition + displacement;
            //move to new starting spot
            transform.position = startPosition;
        }

        //Debug.Log("Set Up");
    }
}
