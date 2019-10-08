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
      
    [SerializeField] private float followDistanceMax;    
    [SerializeField] private float followDistanceMin;
    [SerializeField] private float followDistance;    
    [SerializeField] private float followHeightMax;
    [SerializeField] private float followHeightMin;
    [SerializeField] private float followHeight;

    [Range(1.0f, 100.0f)] [SerializeField] private float followRotation;
    [Range(1.0f, 100.0f)] [SerializeField] private float followRotationMax;
    [Range(1.0f, 100.0f)] [SerializeField] private float followRotationSpeed;
    private float followRotationDefault;

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

    //controller for moving camera updown/ inout
    private enum CameraMode { Height, Distance };
    private CameraMode currentCameraMode = CameraMode.Height;


    
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
        //set camera default height/distance
        followHeight = followHeightMax;
        followDistance = followDistanceMax;

        //store main rotation
        followRotationDefault = followRotation;

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
            //height mode
            if(currentCameraMode == CameraMode.Height)
            {
                followHeight += Input.GetAxisRaw(vertical) * Time.deltaTime * verticalRotSpeed * setup.y;
                Debug.Log(followHeight);

                //fix if out of range
                if (followHeight < followHeightMin)
                {
                    //clamp
                    followHeight = followHeightMin;

                    //change mode
                    currentCameraMode = CameraMode.Distance;
                }
                else if (followHeight > followHeightMax)
                {
                    followHeight = followHeightMax;
                }
            }
            //distance movement
            else if(currentCameraMode == CameraMode.Distance)
            {
                //alter distance by mouse input
                followDistance += Input.GetAxisRaw(vertical) * Time.deltaTime * verticalRotSpeed * setup.y;

                //adjust rotation relative to player
                followRotation -= Input.GetAxisRaw(vertical) * Time.deltaTime * followRotationSpeed * setup.y;
                

                //cap followrotation
                if(followRotation > followRotationMax)
                {
                    followRotation = followRotationMax;
                }

                //fix if out of range
                if (followDistance > followDistanceMax)
                {
                    //clamp
                    followDistance = followDistanceMax;

                    followRotation = followRotationDefault;
                    //change mode
                    currentCameraMode = CameraMode.Height;
                }
                else if (followDistance < followDistanceMin)
                {
                    followDistance = followDistanceMin;
                }
            }


            ////NORMAL WAY
            ////adjust height
            //followHeight += Input.GetAxisRaw(vertical) * Time.deltaTime * verticalRotSpeed * setup.y;

            ////fix if out of range
            //if (followHeight < followHeightMin)
            //{
            //    followHeight = followHeightMin;
            //}
            //else if (followHeight > followHeightMax)
            //{
            //    followHeight = followHeightMax;
            //}


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
