using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float mouseSpeed = 3.0f;
    public float orbitingDamping = 10.0f;
    public Vector3 cameraOffset;
    Vector3 localRotation;
    private Vector3 position;
    private void Start()
    {
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1.0f)
        {
            localRotation.x += Input.GetAxis("Mouse X") * mouseSpeed;
            localRotation.y -= Input.GetAxis("Mouse Y") * mouseSpeed;

            localRotation.y = Mathf.Clamp(localRotation.y, 0f, 70f);

            Quaternion qt = Quaternion.Euler(localRotation.y, localRotation.x, 0f);

            // Calculate the camera's position based on rotation
            if (player != null)
            {
                //position = qt * cameraOffset + player.position;
                position = player.position;
            }



            transform.rotation = Quaternion.Lerp(transform.rotation, qt, Time.deltaTime * orbitingDamping);
            transform.position = position;

            //transform.rotation = Quaternion.Lerp(transform.rotation, qt, Time.deltaTime * orbitingDamping);
        }//if
    }//update
}//followscript

