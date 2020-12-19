using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEditor;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Transform bulletSpawn;
    public GameObject bullet;
    public int fireRate;


    public BulletManager bulletManager;

    [Header("Movement")]
    public float speed;
    public bool isGrounded;


    public RigidBody3D body;
    public CubeBehaviour cube;
    public Camera playerCam;

    // new velocity
    private Vector3 velocity = Vector3.zero, horizontal = Vector3.zero, vertical = Vector3.zero, depth = Vector3.zero;

    void start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _Fire();
        _Move();
        _CheckForQButtonInput();
        //Debug.Log("Y velocity - " + body.velocity.y);
    }

    private void _Move()
    {
        // horizontal and vertical vector..
        horizontal = new Vector3(playerCam.transform.right.x, 0.0f, playerCam.transform.right.z);
        //Debug.Log("Horizontal x, 0 ,z = " + horizontal);
        vertical = new Vector3(playerCam.transform.forward.x, 0.0f, playerCam.transform.forward.z);
        //Debug.Log("Vertical x, 0 ,z = " + horizontal);


        // Debug
        //Debug.Log("Body Velocity = " + body.velocity.magnitude);
        //Debug.Log("Body Acceleration = " + body.acceleration);

        if (isGrounded)     //only works when on ground
        {
            // perform this check to avoid jumping when already in air...
            if (Input.GetAxisRaw("Horizontal") > 0.0f)
            {
                // move right
                velocity += horizontal.normalized * speed * 0.125f * Time.deltaTime;

            }
            else if (Input.GetAxisRaw("Horizontal") < 0.0f)
            {
                // move left
                velocity += -horizontal.normalized * speed * 0.125f * Time.deltaTime;
            }

            if (Input.GetAxisRaw("Vertical") > 0.0f)
            {
                // move forward
                velocity += vertical.normalized * speed * 0.125f * Time.deltaTime;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f)
            {
                // move Back
                velocity += -vertical.normalized * speed * 0.125f * Time.deltaTime;
            }


            body.velocity = Vector3.Lerp(body.velocity, Vector3.zero, 0.9f);
            body.velocity = new Vector3(body.velocity.x, 0.0f, body.velocity.z); // remove y

            // Trigger Jump!
            if (Input.GetAxisRaw("Jump") > 0.0f)
            {
                //Debug.Log("Jump Triggered!");
                body.velocity = transform.up * speed * 0.1f * Time.deltaTime;
                // fix.. update the transform position with jump independently
                transform.position += body.velocity;
            }
            // Debug.Log("Body Velocity = " + body.velocity);

        }
        else        // Works when in air
        {
            // perform this check to avoid jumping when already in air...
            if (Input.GetAxisRaw("Horizontal") > 0.0f)
            {
                // move right
                velocity += horizontal.normalized * speed * 0.1f * Time.deltaTime;
                //Debug.Log("In Air movement to right");
            }
            else if (Input.GetAxisRaw("Horizontal") < 0.0f)
            {
                // move left
                velocity += -horizontal.normalized * speed * 0.1f * Time.deltaTime;
                //Debug.Log("In Air movement to left");
            }

            if (Input.GetAxisRaw("Vertical") > 0.0f)
            {
                // move forward
                velocity += vertical.normalized * speed * 0.1f * Time.deltaTime;
                //Debug.Log("In Air movement to front");
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f)
            {
                // move Back
                velocity += -vertical.normalized * speed * 0.1f * Time.deltaTime;
                //Debug.Log("In Air movement to back");
            }
        }

        // Update position with the calculated velocity
        transform.position += velocity;

        // Reset velocity or else it will be added and will increment automatically
        velocity = Vector3.zero;
    }


    private void _Fire()
    {
        if (Input.GetAxisRaw("Fire1") > 0.0f)
        {
            // delays firing
            if (Time.frameCount % fireRate == 0)
            {

                var tempBullet = bulletManager.GetBullet(bulletSpawn.position, bulletSpawn.forward);
                tempBullet.transform.SetParent(bulletManager.gameObject.transform);
            }
        }
    }

    void FixedUpdate()
    {
        GroundCheck();
    }

    private void GroundCheck()
    {
        isGrounded = cube.isGrounded;
    }

    private void _CheckForQButtonInput()
    {
        //Take Q button for going back to Main Menu
        if (Input.GetKeyDown("q"))
        {
            Debug.Log("Q Button Pressed");
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
            }
        }
    }
}