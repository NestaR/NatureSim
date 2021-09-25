using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwivel : MonoBehaviour
{ 
    private Transform CameraMain;
    private Transform CameraParent;

    private Vector3 CameraRotation;
    private float CameraDistance = 10f;
    private Vector3 lastDragPosition;
    public float MouseSensitivity = 2f;
    public float ScrollSensitvity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public bool CameraDisabled = false;

    void Start()
    {
        this.CameraMain = this.transform;
        this.CameraParent = this.transform.parent;
    }


    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {//Camera only moves whilst the RMB is held down
            CameraDisabled = !CameraDisabled;
        }
        if (Input.GetMouseButtonUp(1))
        { 
            CameraDisabled = !CameraDisabled;
        }
        if (!CameraDisabled)
        {
            //Rotation of the Camera based on which way the user moves the mouse
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                CameraRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                CameraRotation.y += Input.GetAxis("Mouse Y") * MouseSensitivity;

                //Clamp the y Rotation to horizon and not flipping over at the top
                if (CameraRotation.y < 0f)
                    CameraRotation.y = 0f;
                else if (CameraRotation.y > 90f)
                    CameraRotation.y = 90f;
            }

        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {//Zoom using the scroll wheel
            float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;

            ScrollAmount *= (this.CameraDistance * 0.3f);

            this.CameraDistance += ScrollAmount * -1f;

            this.CameraDistance = Mathf.Clamp(this.CameraDistance, 1.5f, 100f);
        }
        //Camera rig Transformations
        Quaternion QT = Quaternion.Euler(CameraRotation.y, CameraRotation.x, 0);
        this.CameraParent.rotation = Quaternion.Lerp(this.CameraParent.rotation, QT, Time.deltaTime * OrbitDampening);

        if (this.CameraMain.localPosition.z != this.CameraDistance * -1f)
        {
            this.CameraMain.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this.CameraMain.localPosition.z, this.CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }

    }
}