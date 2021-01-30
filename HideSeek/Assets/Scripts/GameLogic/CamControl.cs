using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    // parameter and refs
    private float RotationSpeed;
    private float mouseX, mouseY;
    private Quaternion playerInitRotation;

    private GameObject player;
    public PossessController possessController;
    // game logic
    private Vector3 camOffset;
    private bool hiderFound = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerInitRotation = transform.rotation;
        camOffset = new Vector3(0, 1.3f, 0);
    }

    void Update()
    {
        // find the player object
        if (!hiderFound)
        {
            player = GameObject.Find("Hider(Clone)");
            if (player == null)
            {
                print("player not found");
                return;
            }
            else
            {
                RotationSpeed = player.GetComponent<PlayerMovement>().RotationSpeed;
                hiderFound = true;
            }
        }
        // player instanciated in room
        else
        {
            // follow player
            if (possessController.hiderActive)
                movement();
            // follow poseess obj
            else
                movementPossess();
        }
    }

    // use this movement method before hider possess
    void movement()
    {
        // rotate
        mouseX += Input.GetAxis("Mouse X") * RotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * RotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);

        transform.rotation = Quaternion.Euler(mouseY, playerInitRotation.eulerAngles.y + mouseX, 0);

        // translate cam
        transform.position = player.transform.position + camOffset + transform.rotation * Vector3.forward;
    }

    // use this one when hider already possessed items
    void movementPossess()
    {
        // rotate
        mouseX += Input.GetAxis("Mouse X") * RotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * RotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);
        transform.rotation = Quaternion.Euler(mouseY, playerInitRotation.eulerAngles.y + mouseX, 0);

    }
}
