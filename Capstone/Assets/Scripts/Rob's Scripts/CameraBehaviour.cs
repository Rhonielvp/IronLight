using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cameraPivot;
    [SerializeField] private GameObject camera;

    [SerializeField] private float horizontalRotSpeed;
    [SerializeField] private float verticalRotSpeed;
    [SerializeField] private float followDistance;
    [SerializeField] private float followHeight;
    [SerializeField] private float maxFollowHeight;
    [SerializeField] private float minFollowHeight;
    [Range(0.0f, 40.0f)] [SerializeField] private float followRotation;

    private float rotX;
    private float rotY;
    private float rotZ;

    //enum to set camera controller
    private enum CameraController { Keyboard, Mouse };
    [SerializeField] private CameraController currentCamera;   
    
    //make it so players can decide the arrow directions
    private enum ControllerSetup { normal, inverted, invertedX, invertedY };
    [SerializeField] private ControllerSetup currentSetup;

    //various setups for controllers stored as dictionary
    private Dictionary<ControllerSetup, Vector2> setup = new Dictionary<ControllerSetup, Vector2>()
    {
        { ControllerSetup.normal, new Vector2(1f, -1f) },
        { ControllerSetup.inverted, new Vector2(-1f, 1f) },
        { ControllerSetup.invertedX, new Vector2(-1f, -1f) },
        { ControllerSetup.invertedY, new Vector2(1f, 1f) },

    };
    
    // Start is called before the first frame update
    void Start()
    {
        Setup();

        //makes cursor invisible
        Cursor.lockState = CursorLockMode.Locked;
    }


    // Update is called once per frame
    void Update()
    {
        //use enum to control camera settings
        if(currentCamera == CameraController.Keyboard)
        {
            //Debug.Log("Keyboard");
            SetCamera("CameraVertical", "CameraHorizontal", setup[currentSetup]);
        }
        else if(currentCamera == CameraController.Mouse)
        {
            //Debug.Log("Mouse");
            SetCamera("MouseVertical", "MouseHorizontal", setup[currentSetup]);
        }       
    }

    //set camera
    private void Setup()
    {
        //set camera pivot identical to player character
        cameraPivot.transform.position = player.transform.position;
        cameraPivot.transform.rotation = player.transform.rotation;

        //get rotations for quick access later
        rotX = cameraPivot.transform.rotation.eulerAngles.x;
        rotY = cameraPivot.transform.rotation.eulerAngles.y;
        rotZ = cameraPivot.transform.rotation.eulerAngles.z;

        //set starting camera position
        SetCamera();        
    }

    //pass in the input strings for whatever you want to use, keyboard or mouse
    //setup h/v will be for effecting invert or not
    private void SetCamera(string vertical, string horizontal, Vector2 setup)
    {
        //keep pivot on player position but with seperate rotation
        cameraPivot.transform.position = player.transform.position;

        //if camera Hor movement
        if (Input.GetAxisRaw(horizontal) != 0)
        {
            //get rotation change
            rotY += Input.GetAxisRaw(horizontal) * Time.deltaTime * horizontalRotSpeed * setup.x;

            //change pivot rotation
            cameraPivot.transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
        }
        //if camera vert movement
        if (Input.GetAxisRaw(vertical) != 0)
        {            
            //adjust height
            followHeight += Input.GetAxisRaw(vertical) * Time.deltaTime * verticalRotSpeed * setup.y;

            //fix if out of range
            if (followHeight < minFollowHeight)
            {
                followHeight = minFollowHeight;
            }
            else if (followHeight > maxFollowHeight)
            {
                followHeight = maxFollowHeight;
            }


            //adjust camera as new follow height
            SetCamera();            
        }
    }
    
    
    //code sugar
    private void SetCamera()
    {
        camera.transform.position = cameraPivot.transform.position + cameraPivot.transform.up * followHeight + 
                                    cameraPivot.transform.forward * -1 * followDistance;
        camera.transform.LookAt(cameraPivot.transform.position);
        camera.transform.Rotate(-followRotation, 0, 0);
    }
}
