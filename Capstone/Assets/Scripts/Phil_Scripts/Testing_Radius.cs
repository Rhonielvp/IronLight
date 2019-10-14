// Phil James - Capstone 2019
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class Testing_Radius: MonoBehaviour
{

    [Range(0, 200)]
    public int segments = 128;
    [Range(0, 200)]
    public float xradius = 80;
    [Range(0, 200)]
    public float yradius = 80;
    LineRenderer line;

    // -------------------------------------------------------------------
    // Phil James - Capstone 2019
    // DESC		:	This is for Testing Purposes Only! 
    //				It draw/render a circle base from the Radius 
    //				Chase Distance.  If the Player Enter in that radius the (IDLE)Enemy will be allerted
    // ------------------------------------------------------------------
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();

        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        CreatePoints();
    }

    void CreatePoints()
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / segments);
        }
    }
}
