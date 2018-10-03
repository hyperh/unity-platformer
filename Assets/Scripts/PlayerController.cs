using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float walkSpeed;
    public float jumpForce;
    Rigidbody rb;
    bool pressedJump = false;
    Collider col;
    Vector3 size;
    public AudioSource coinSound;
    public float cameraDistZ = 6;
    float minY = -2.5f;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        size = col.bounds.size;
        CameraFollowPlayer();
    }

    // Update is called once per frame
    // Use FixedUpdate when you are working with RigidBody, physics etc
    void FixedUpdate() {
        WalkHandler();
        JumpHandler();
        CameraFollowPlayer();
        FallHandler();
    }

    private void FallHandler()
    {
        if (transform.position.y <= minY)
        {
            GameManager.instance.GameOver();
        }
    }

    void WalkHandler()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hAxis * walkSpeed * Time.deltaTime, 0, vAxis * walkSpeed * Time.deltaTime);
        Vector3 newPos = transform.position + movement;
        rb.MovePosition(newPos);

        if (hAxis != 0 || vAxis != 0)
        {
            Vector3 direction = new Vector3(hAxis, 0, vAxis);

            // Method 1
            // Best not to modify transform directly if we are dealing with a rigidbody.
            // Unless it is a kinematic rigidbody
            // transform.forward = direction;

            // Method 2
            rb.rotation = Quaternion.LookRotation(direction);

        }
    }

    void JumpHandler()
    {
        float jAxis = Input.GetAxis("Jump");
        if (jAxis > 0)
        {
            bool isGrounded = CheckGrounded();
            if (!pressedJump && isGrounded)
            {
                pressedJump = true;
                Vector3 jumpVector = new Vector3(0, jAxis * jumpForce, 0);
                rb.AddForce(jumpVector, ForceMode.VelocityChange);
            }
        }
        else
        {
            pressedJump = false;
        }
    }

    private bool CheckGrounded()
    {
        // Get location of 4 corners of collider
        // size.y/2 + 0.01f is correction distance since size.y/2 is exactly at the border of the collider and ground, so raycast will not find ground
        // raycast needs to start a little bit above the border
        Vector3 corner1 = transform.position + new Vector3(size.x / 2, -size.y / 2 + 0.01f, size.z / 2);
        Vector3 corner2 = transform.position + new Vector3(-size.x / 2, -size.y / 2 + 0.01f, size.z / 2);
        Vector3 corner3 = transform.position + new Vector3(size.x / 2, -size.y / 2 + 0.01f, -size.z / 2);
        Vector3 corner4 = transform.position + new Vector3(-size.x / 2, -size.y / 2 + 0.01f, -size.z / 2);

        // check if grounded
        bool grounded1 = Physics.Raycast(corner1, Vector3.down, 0.01f);
        bool grounded2 = Physics.Raycast(corner2, Vector3.down, 0.01f);
        bool grounded3 = Physics.Raycast(corner3, Vector3.down, 0.01f);
        bool grounded4 = Physics.Raycast(corner4, Vector3.down, 0.01f);

        return (grounded1 || grounded2 || grounded3 || grounded4);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.instance.IncreaseScore(1);
            coinSound.Play();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            GameManager.instance.GameOver();
        }
        else if (other.CompareTag("Goal"))
        {
            GameManager.instance.IncreaseLevel();
        }
    }

    void CameraFollowPlayer()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.z = transform.position.z - cameraDistZ;
        Camera.main.transform.position = cameraPos;
    }
}
    