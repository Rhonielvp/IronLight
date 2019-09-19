using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cameraPivot;
    [SerializeField] private GameObject camera;  //???

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


    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }


    // Update is called once per frame
    void Update()
    {
        //keep pivot on player position but with seperate rotation
        cameraPivot.transform.position = player.transform.position;

        //if camera Hor movement
        if (Input.GetAxisRaw("CameraHorizontal") != 0)
        {
            //get rotation change
            rotY += Input.GetAxisRaw("CameraHorizontal") * Time.deltaTime * horizontalRotSpeed;

            //change pivot rotation
            cameraPivot.transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
        }
        //if camera vert movement
        if (Input.GetAxisRaw("CameraVertical") != 0)
        {
            //adjust followheight according to up or down
            if (Input.GetAxisRaw("CameraVertical") > 0 && followHeight < maxFollowHeight)
            {
                followHeight += Input.GetAxisRaw("CameraVertical") * Time.deltaTime * verticalRotSpeed;
            }
            else if (Input.GetAxisRaw("CameraVertical") < 0 && followHeight > minFollowHeight)
            {
                followHeight += Input.GetAxisRaw("CameraVertical") * Time.deltaTime * verticalRotSpeed;
            }

            //same as setup code
            camera.transform.position = SetCamera();
            camera.transform.LookAt(cameraPivot.transform.position);
            camera.transform.Rotate(-followRotation, 0, 0);
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
        camera.transform.position = SetCamera();

        //set camera rotation and add Dev offset
        camera.transform.LookAt(cameraPivot.transform.position);
        camera.transform.Rotate(-followRotation, 0, 0);
    }

    //code sugar
    private Vector3 SetCamera()
    {
        return cameraPivot.transform.position + cameraPivot.transform.up * followHeight + cameraPivot.transform.forward * -1 * followDistance;
    }
}
