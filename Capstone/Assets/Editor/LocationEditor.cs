using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocationBehaviour))]
public class LocationEditor : Editor
{
    LocationBehaviour LB;

    //set target of the this editor script to script
    private void OnEnable()
    {
        LB = (LocationBehaviour)target;
    }

    //runs base inspector GUI 
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        //help us visualize exactly where the location is
        LB.handleSize = Handles.RadiusHandle(Quaternion.identity, LB.transform.position, LB.handleSize);        
    }
}
