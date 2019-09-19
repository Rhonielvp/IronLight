using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        //gather all puzzle pieces
        GameObject[] puzzleElements = GameObject.FindGameObjectsWithTag("Puzzle");

        List<GameObject> crystals = new List<GameObject>();
        List<GameObject> shrines = new List<GameObject>();
        List<GameObject> triggeredObj = new List<GameObject>();


        //divide them up into seperate lists depending on what scripts they contain
        for (int i = 0; i < puzzleElements.Length; i++)
        {
            GameObject obj = puzzleElements[i];

            //these objects should only contain 1 of these scripts so add to appropriate list
            if (obj.GetComponent<CrystalBehaviour>() != null)
            {
                crystals.Add(obj);
                Debug.Log("Added crystals");
            }
            else if (obj.GetComponent<ShrineBehaviour>() != null)
            {
                shrines.Add(obj);
                Debug.Log("Added shrines");
            }
            else if (obj.GetComponent<MoveObject>() != null)
            {
                triggeredObj.Add(obj);
                Debug.Log("Added triggeredObj");
            }
        }

        //send master list to other setup functions to get everything all organized
        SetupAllCrystals(crystals.ToArray(), shrines.ToArray());
        SetupAllTriggeredObj(triggeredObj.ToArray(), shrines.ToArray());

        Debug.Log("PuzzleSetupDone");
    }


    private void SetupAllCrystals(GameObject[] allCrystals, GameObject[] allShrines)
    {
        Debug.Log("setting up crystals");
        //create temp list to store crystals to add, than set it to corresponding crystal, reset list at end of loop
        List<Transform> tempList = new List<Transform>();
        string tempCrystalGroup;

        
        for (int i = 0; i < allCrystals.Length; i++)
        {
            //store puzzle name
            tempCrystalGroup = allCrystals[i].GetComponent<CrystalBehaviour>().crystalGroup;

            //check crystals
            //index through array and add any crystal with the same puzzle name to list
            for (int j = 0; j < allCrystals.Length; j++)
            {
                //skip self
                if (i == j)
                {
                    continue;
                }
                else
                {
                    //if the puzzle name matches the current stored one add it to the list
                    if (tempCrystalGroup == allCrystals[j].GetComponent<CrystalBehaviour>().crystalGroup)
                    {
                        //store the transform not the gameobject, less storage
                        tempList.Add(allCrystals[j].transform);
                    }
                }
            }

            //check shrines
            for (int j = 0; j < allShrines.Length; j++)
            {
                if(tempCrystalGroup == allShrines[j].GetComponent<ShrineBehaviour>().crystalGroup)
                {
                    tempList.Add(allShrines[j].transform);
                }
            }

            //set current crystal list to what is in tempList
            allCrystals[i].GetComponent<CrystalBehaviour>().SetList(tempList);

            //clear temp list and restart
            tempList.Clear();
        }
    }

    //have triggered objects add themselves to shrine lists so they can get triggered
    private void SetupAllTriggeredObj(GameObject[] allTriggeredObj, GameObject[] allShrines)
    {
        Debug.Log("setting up triggers");
        //move objects search through the shrine list
        //find matching group name and add itself to that shrines list of observers

        for (int i = 0; i < allTriggeredObj.Length; i++)
        {
            MoveObject trigger = allTriggeredObj[i].GetComponent<MoveObject>();

            for (int j = 0; j < allShrines.Length; j++)
            {
                ShrineBehaviour shrine = allShrines[j].GetComponent<ShrineBehaviour>();
                
                if (trigger.triggerGroup == shrine.triggerGroup)
                {
                    //add 
                    shrine.AddToTriggerList(allTriggeredObj[i]);                    
                }                
            }
        }
    }
}
