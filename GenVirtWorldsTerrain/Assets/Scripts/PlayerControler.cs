using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
   
    //movement set up
    private GameObject player;
    private Rigidbody rigidbody;
    [SerializeField] float speed = 5.0f;
    [SerializeField] Transform camera;
    

    //for checking if player is toutching ground
    public float jumpHeight;
    public bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;

        rigidbody = player.GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
        camera.GetComponent<CameraFollow>().player = this.player.transform;
        camera.GetComponent<CameraFollow>().cameraOffset = new Vector3(0, 0, -20);
        
        
    }

    // Update is called once per frame
    void Update()
    {

        //get the camera facing direction
        Vector3 camForward = camera.forward;
        Vector3 camRight = camera.right;

        //the vertical rotation of camera should not impact movement
        camForward.y = 0;
        camRight.y = 0;

        //create relative camera direction
        Vector3 forwardsRelative = (Input.GetAxis("Vertical") * speed) * camForward;
        Vector3 rightRelative = (Input.GetAxis("Horizontal") * speed) * camRight;

        Vector3 movementDirection = forwardsRelative + rightRelative;

        //movement
        //rigidbody.AddForce(movementDirection, ForceMode.VelocityChange);

        float vertical = 0f;

        //jump check
        if (Input.GetKey(KeyCode.Space))
        {
            //rigidbody.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
            vertical = jumpHeight * speed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            //rigidbody.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
            vertical = -jumpHeight * speed;
        }

        rigidbody.linearVelocity = new Vector3(movementDirection.x, movementDirection.y + vertical, movementDirection.z);
    }

}


