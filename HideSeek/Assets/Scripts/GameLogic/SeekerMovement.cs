using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SeekerMovement : MonoBehaviour
{
    public float RotationSpeed;
    public float moveSpeed;
    private float mouseX;
    private Quaternion playerInitRotation;

    [SerializeField] private bool isDebug;
    private Animator seekerAnim;
    private AudioSource footstep;

    
    // Start is called before the first frame update
    void Start()
    {
        playerInitRotation = transform.rotation;
        transform.forward = -transform.right;
        seekerAnim = GetComponentInChildren<Animator>();
        footstep = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<PhotonView>().IsMine && !isDebug)
            return;

        mouseX += Input.GetAxis("Mouse X") * RotationSpeed;
        transform.rotation = Quaternion.Euler(0, playerInitRotation.eulerAngles.y + mouseX, 0);

        // translation player
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.S))
            transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D))
            transform.Translate(-Vector3.left * moveSpeed * Time.deltaTime);

        // walking anim
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            seekerAnim.SetBool("walk", true);
            footstep.volume = .3f;
        }
        else
        {
            seekerAnim.SetBool("walk", false);
            footstep.volume = 0;
        }
    }
}
