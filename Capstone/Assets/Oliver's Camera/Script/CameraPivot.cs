using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    public GameObject target;
    private PlayerCamera _camera;
    public float offSetUp = 0.6f;

    public bool runFunc = false;
    public float DemoVarOffup = 0.6f;
    public float DemoVarOffleft = 0;
    public float DemoVarSensi = 3;
    public float DemoVarDamp = 20;
    public float DemoVarDis = 6;
    public float DemoVarMiny = -8;
    public float DemoVarMaxy = 70;
    public float DemoVarTrnsSpd = 1;

    void Start()
    {
        //Getting Reference to the camera
        _camera = GetComponentInChildren<PlayerCamera>();
    }

    void Update()
    {
        //Position the camera pivot on the player
        if (target != null)
            transform.position = target.transform.position + (Vector3.up * offSetUp);

        //Call Camera Transition
        if (runFunc)
        {
            ChangePlayerCamera(DemoVarOffup, DemoVarOffleft, DemoVarSensi, DemoVarDamp, DemoVarDis, DemoVarMiny, DemoVarMaxy, DemoVarTrnsSpd);
            runFunc = false;
        }
    }


    


    // Camera Transition //////////////

    public void ChangePlayerCamera(float pOffSetUp, float pOffSetLeft, float pMouseSensitivity, float pTurnDampening, float pCameraDistance, float pCameraMinHeight, float pCameraMaxHeight, float pTransitionSpeed)
    {
        StopCoroutine("CameraOffSetTransition");
        StartCoroutine(CameraOffSetTransition(pOffSetUp, pTransitionSpeed));
        _camera.ChangePlayerCamera(pOffSetLeft, pMouseSensitivity, pTurnDampening, pCameraDistance, pCameraMinHeight, pCameraMaxHeight, pTransitionSpeed);
    }

    private IEnumerator CameraOffSetTransition(float pOffSetUp, float pTransitionSpeed)
    {
        while (offSetUp != pOffSetUp)
        {
            offSetUp = Mathf.Lerp(offSetUp, pOffSetUp, pTransitionSpeed * Time.deltaTime);
            if (Mathf.Abs(offSetUp - pOffSetUp) <= 0.01f)
                offSetUp = pOffSetUp;

            yield return null;
        }
    }
}
