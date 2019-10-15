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
        //sphere is 0, box is 1
        if((int)LB.currentCollider == 0)
        {
            //help us visualize exactly where the location is
            LB.radiusSize = Handles.RadiusHandle(Quaternion.identity, LB.transform.position, LB.radiusSize);
        }
        else
        {           
            DrawRectangle();
        }        
    }


    private void DrawWireCube()
    {
        Vector3 rectangle = new Vector3(LB.width, LB.depth, LB.length);
        Handles.color = Color.red;
        //Handles.matrix = LB.transform.localToWorldMatrix;
        Handles.DrawWireCube(LB.transform.position, rectangle);
    }

    //draws rectangle that can resize and rotate
    private void DrawRectangle()
    {        
        //get forward and right vectors
        Vector3 forward = LB.transform.forward;
        Vector3 right = LB.transform.right;

        //make size handles for length and width
        LB.length = Handles.ScaleValueHandle(LB.length, LB.transform.position + forward * LB.length/2,
                                             Quaternion.LookRotation(LB.transform.forward), LB.length, 
                                             Handles.ArrowHandleCap, 1f);

        LB.width = Handles.ScaleValueHandle(LB.width, LB.transform.position + right * LB.width/2,
                                             Quaternion.LookRotation(LB.transform.right), LB.width, 
                                             Handles.ArrowHandleCap, 1f);
    
        //calculate vector points to make rectangle
        Vector3 topRight = LB.transform.position + (forward * LB.length/2) + (right * LB.width/2);
        Vector3 bottomRight = LB.transform.position - (forward * LB.length/2) + (right * LB.width/2);
        Vector3 bottomLeft = LB.transform.position - (forward * LB.length/2) - (right * LB.width/2);
        Vector3 topLeft = LB.transform.position + (forward * LB.length/2) - (right * LB.width/2);

        //put them into vector array
        Vector3[] verts = new Vector3[]
        {
                topRight, bottomRight, bottomLeft, topLeft
        };

        //draw rectangle
        Handles.DrawSolidRectangleWithOutline(verts, Color.clear, Color.red);

    }
}
