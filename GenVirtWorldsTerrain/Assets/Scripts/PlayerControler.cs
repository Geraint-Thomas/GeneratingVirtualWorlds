using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    /*
    [SerializeField] Transform camera;
    [SerializeField] Transform player;
    private Vector3 playerPos;
    public float speed = 1;


    void Start()
    {
        playerPos = player.position;
    }

    void Update()
    {
        if(Keyboard.current.wKey.isPressed)
        {
            playerPos.x += 0.01f * speed * Time.deltaTime;
        }



        if(player.position != playerPos)
        {
            player.position = playerPos;
            camera.position = new Vector3(playerPos.x, playerPos.y + 0.5f, playerPos.z);
        }
    }

    */

    
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
        speed = 0.11f;
        
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
        rigidbody.AddForce(movementDirection, ForceMode.VelocityChange);
        //rigidbody.velocity = new Vector3(movementDirection.x * speed, rigidbody.velocity.y * speed, movementDirection.z * speed);

       
        //jump check
        if (Input.GetKeyDown(KeyCode.Space))//&& grounded)
        {
            rigidbody.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        }

        

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            grounded = true;
        }
       

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            grounded = false;
        }
        

    }
    
}


