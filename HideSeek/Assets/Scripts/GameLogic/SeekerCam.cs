using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerCam : MonoBehaviour
{
    private float RotationSpeed;
    private float mouseX, mouseY;
    private Quaternion playerInitRotation;

    private GameObject seeker;
    private Vector3 offset;
    private bool seekerFound = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerInitRotation = transform.rotation;
        offset = new Vector3(0, 6.2f, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // find the player object
        if (!seekerFound)
        {
            seeker = GameObject.Find("Seeker(Clone)");
            if (seeker == null)
            {
                print("seeker not found");
                return;
            }
            else
            {
                RotationSpeed = seeker.GetComponent<SeekerMovement>().RotationSpeed;
                seekerFound = true;
            }
        }
        // player instanciated in room, enable cam movemnt
        else
        {
            mouseX += Input.GetAxis("Mouse X") * RotationSpeed;
            mouseY -= Input.GetAxis("Mouse Y") * RotationSpeed;
            mouseY = Mathf.Clamp(mouseY, -35, 60);

            transform.rotation = Quaternion.Euler(mouseY, playerInitRotation.eulerAngles.y + mouseX, 0);
            transform.position = seeker.transform.position + offset + transform.rotation * Vector3.forward ;
        }
    }
}
