using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rob
//Script for crystal, contains list that references all other crystals in its group
//Controls rotation and making sure crystal rotates to point at correct crystals
//
//Make sure all crystals have the puzzle tag...
public class CrystalBehaviour : MonoBehaviour
{
    //sets what puzzle group its in
    public string crystalGroup;

    //determines its order in the puzzle... not implemented yet
    [SerializeField] private int order;
    [SerializeField] private List<Transform> otherCrystals = new List<Transform>();


    //get list
    public List<Transform> GetList()
    {
        return otherCrystals;
    }

    //change list to one passed in... make more secure...
    public void SetList(List<Transform> newList)
    {
        //add each element from newlist to list here
        foreach (Transform trans in newList)
        {
            otherCrystals.Add(trans);
        }

        //set list count for rotation, see below
        crystalListCount = otherCrystals.Count;
    }

    //return the transform at the provided index
    public Transform GetList(int index)
    {
        if (index > otherCrystals.Count - 1)
        {
            index = 0;
            Debug.Log("that crystal index is too large");
        }
        return otherCrystals[index];
    }

    //return total number of objects in list
    public int GetCount()
    {
        return otherCrystals.Count;
    }

    //allow crystal to rotate
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canRotate = true;
            //other.GetComponent<PlayerIndicator>().ActivateIndicator(gameObject, true);            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canRotate = false;
            //other.GetComponent<PlayerIndicator>().ActivateIndicator(gameObject, false);
        }
    }


    //*********************
    //combining rotation script into behaviour

    //invisible gameobject used to do lookat
    public Transform seeker;

    //rotation to lerp to
    public Quaternion targetRotation;

    public int crystalListCount = 0;
    public int crystalToLook = 0;

    [SerializeField] private bool canRotate = false;


    // Update is called once per frame
    void Update()
    {
        //player must be within trigger of crystal to rotate it
        if (Input.GetKeyDown(KeyCode.R) && canRotate == true)
        {
            //get the rotation that would make the object rotate so that it was 
            //looking at the given transform
            //check to insure that we are not going out of bounds on list
            if (crystalToLook == crystalListCount)
            {
                crystalToLook = 0;
            }

            Debug.Log("rotated crystal");
            seeker.LookAt(GetComponent<CrystalBehaviour>().GetList(crystalToLook));
            targetRotation = seeker.rotation;
            crystalToLook++;
        }

        if (transform.rotation != targetRotation)
        {
            //the rotation speed will stay relatively consistent regardless of whether far or near            
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
        }
    }
}
