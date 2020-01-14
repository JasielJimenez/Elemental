using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
     public float speed = 30.0f;
     public float zoomFactor = 2.0f;
     public float maxZoom = 50.0f;
     public float minZoom = 10.0f;

     public bool allowMove = true;
     public Transform cam;
     public GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Gamemanager");
    }

    public void camMove()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        //Vector3 camF = cam.forward;
        //Vector3 camR = cam.right;
        //camF.y = 0;
        //camR.y = 0;
        //camF = camF.normalized;
        //camR = camR.normalized;
        //Vector3 groundForward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
        //Vector3 groundRight = new Vector3(cam.transform.right.x, 0, cam.transform.right.z).normalized;
        Vector3 movement = new Vector3(moveHorizontal,0,moveVertical);
        movement = movement.normalized;
        if (movement != Vector3.zero)
        {
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
        }
        else{
        }
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    public void camZoom()
    {
        if(transform.position.y > minZoom & transform.position.y < maxZoom)
        {
            transform.Translate(0,-Input.mouseScrollDelta.y * zoomFactor,0);
        }

        if(transform.position.y <= minZoom && Input.mouseScrollDelta.y < 0)
        {
            transform.Translate(0,-Input.mouseScrollDelta.y * zoomFactor,0);
        }
        else if(transform.position.y >= maxZoom && Input.mouseScrollDelta.y > 0)
        {
            transform.Translate(0,-Input.mouseScrollDelta.y * zoomFactor,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(allowMove == true)
        {
            camMove();
        }
        else if(gameManager.GetComponent<Gamemanager>().selectedPlayer != null)   //USELESS?
        {
            //FIXME: Grab only X and Z values, not Y
            //this.transform.position = new Vector3(gameManager.GetComponent<Gamemanager>().selected.transform.position.x,transform.position.y,gameManager.GetComponent<Gamemanager>().selected.transform.position.z - 10);
            //iTween.MoveTo(this.gameObject, iTween.Hash("position", temp,"time", 5.0f,"easetype", iTween.EaseType.easeInOutSine));
        }
        if(Input.mouseScrollDelta.y != 0)
        {
            camZoom();
        }
    }

    public void enableMove()
    {
        allowMove = true;
    }

    public void disableMove()
    {
        allowMove = false;
    }
}
