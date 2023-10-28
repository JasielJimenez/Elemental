using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public GameObject HealthbarScale;
    public GameObject PotentialDamageScale;

    // Start is called before the first frame update
    void Start()
    {
        HealthbarScale = this.transform.GetChild(0).gameObject;
        PotentialDamageScale = this.transform.GetChild(0).GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Updates health bar to show current health after gaining/losing health
    public void DisplayPotentialDamage(float potentialDamage)
    {
        //For some reason, AddToTargetList() in CharacterCombat.cs reaches this before PotentialDamageScale is assigned?
        //For now, assigning it here as well as in Start()
        //Debug.Log("PotentialDamage: " + potentialDamage);
        PotentialDamageScale = this.transform.GetChild(0).GetChild(1).gameObject;
        PotentialDamageScale.transform.localScale = new Vector3(potentialDamage, PotentialDamageScale.transform.localScale.y, PotentialDamageScale.transform.localScale.z);
    }

    // Updates health bar to show current health after gaining/losing health
    public void UpdateHealthBar(float healthRemaining)
    {
        HealthbarScale = this.transform.GetChild(0).gameObject;
        HealthbarScale.transform.localScale = new Vector3(healthRemaining, HealthbarScale.transform.localScale.y, HealthbarScale.transform.localScale.z);
    }

    
    public void LateUpdate()
    {
        //Makes healthbar face camera
        transform.forward = Camera.main.transform.forward;
    }
}
