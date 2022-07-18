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
    public float MouseSensitivity = 1.5f;
    public float ScrollSensitvity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;
    public float movementSpeed;
    public bool CameraDisabled = false;

    void Start()
    {
        this.CameraMain = this.transform;
        this.CameraParent = this.transform.parent;
    }
    void Update()
    {
        //Stops the camera from going out of bounds
        if (this.transform.position.x < -110)
        {
            this.CameraMain.localPosition += new Vector3(1f, 0f, 0f);
        }
        else if (this.transform.position.x > 110)
        {
            this.CameraMain.localPosition += new Vector3(-1f, 0f, 0f);
        }
        if (this.transform.position.z < -110)
        {
            this.CameraMain.localPosition += new Vector3(0f, 0f, 1f);
        }
        else if (this.transform.position.z > 110)
        {
            this.CameraMain.localPosition += new Vector3(0f, 0f, -1f);
        }
        
    }

    void LateUpdate()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 playermovement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 movementRotated = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * playermovement;
        
        transform.Translate(movementRotated * movementSpeed * Time.deltaTime, Space.World);

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
                CameraRotation.y += Input.GetAxis("Mouse X") * MouseSensitivity;
                CameraRotation.x += Input.GetAxis("Mouse Y") * MouseSensitivity;

                //Clamp the x Rotation to horizon and not flipping over at the top
                if (CameraRotation.x < -10f)
                {
                    CameraRotation.x = -10f;
                }
                else if (CameraRotation.x > 45f)
                {
                    CameraRotation.x = 45f;
                }
            }

            Quaternion QT = Quaternion.Euler(CameraRotation.x, CameraRotation.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, QT, Time.deltaTime * 25f);

        }
        //if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        //{//Zoom using the scroll wheel
        //    float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;

        //    ScrollAmount *= (this.CameraDistance * 0.3f);

        //    this.CameraDistance += ScrollAmount * -1f;

        //    this.CameraDistance = Mathf.Clamp(this.CameraDistance, 1.5f, 150f);

        //}
        //Camera rig Transformations
        //Quaternion QT = Quaternion.Euler(CameraRotation.y, CameraRotation.x, this.CameraParent.rotation.z);
        //this.CameraParent.rotation = Quaternion.Lerp(this.CameraParent.rotation, QT, Time.deltaTime * 20f);

        //if (this.CameraMain.localPosition.z != this.CameraDistance * -1f)
        //{
        //    this.CameraMain.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this.CameraMain.localPosition.z, this.CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        //}

    }
}