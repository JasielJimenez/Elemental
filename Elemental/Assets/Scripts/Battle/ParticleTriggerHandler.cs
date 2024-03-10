using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTriggerHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Set trigger to be attackrange hitbox
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnParticleTrigger()
    {
        //first parent hits attack particles, second parent gets character
        //this.transform.parent.parent
        Debug.Log("PARTICLE TRIGGER");
    }
}
