using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
     
            //debugRay();
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                //if(EventSystem.current.IsPointerOverGameObject())
                //{
                //    Debug.Log("WELP");
                //    return;
                //}
                if(hit.transform.tag == "Player")
                {
                    //selected = hit.transform.gameObject;
                    //chooseMenu.SetActive(true);
                    //playerHUD.SetActive(true);
                    //getPlayerInfo();
                    //playerSelected = true;
                    //this.transform.position = new Vector3(selected.transform.position.x,20,selected.transform.position.z - 10);
                }
                else if(hit.transform.tag == "Enemy")
                {

                }
                else if(hit.transform.tag == "Destructible")
                {

                }

            }
            

        }
    }
}
