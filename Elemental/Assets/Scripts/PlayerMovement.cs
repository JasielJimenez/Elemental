using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float speed = 6.0f;
    public float Jumpspeed = 8.0f;
    public float Rotatespeed = 3.0f;
    public float Gravity = 20.0f;
    public float Movementspeed = 1.0f;
    public bool walkActive = false;
    public GameObject cam;
    public GameObject walkCircle;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController Controller;
    // Start is called before the first frame update
    void Start()
    {
        //walkCircle.SetActive(false);
        Controller = GetComponent<CharacterController>();
        cam = GameObject.Find("CameraBody");
    }

    // Update is called once per frame
    void Update()
    {
        if(walkActive == true)
        {
            //playerMove();
            if (Controller.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= Movementspeed;
                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = Jumpspeed;
                }
            }
            moveDirection.y -= Gravity * Time.deltaTime;
            Controller.Move(moveDirection * Time.deltaTime);
            //transform.Rotate(0, Input.GetAxis("Horizontal"), 0);
        }
    }

    /*
    public void PlayerMove()
    {
        //float moveHorizontal = Input.GetAxisRaw("Horizontal");
        //float moveVertical = Input.GetAxisRaw("Vertical");
        Vector3 camF = cam.transform.forward;
        Vector3 camR = cam.transform.right;
        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;
        Vector3 groundForward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
        Vector3 groundRight = new Vector3(cam.transform.right.x, 0, cam.transform.right.z).normalized;
        Vector3 movement = groundForward * Input.GetAxisRaw("Vertical") + groundRight * Input.GetAxisRaw("Horizontal");
        movement = movement.normalized;
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
            //anim.SetFloat("MoveSpeed",1.0f);
        }
        //else{
            //anim.SetFloat("MoveSpeed", 0.0f);
        //}
        //transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
    */

    public void enableWalk()
    {
        walkActive = true;
        //walkCircle.SetActive(true);
    }

    public void disableWalk()
    {
        walkActive = false;
        //walkCircle.SetActive(false);
    }
}
